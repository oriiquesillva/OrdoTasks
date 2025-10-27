import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { ModalCreateTaskComponent } from './components/modal-tasks/modal-task.component';
import { ApiService } from '@shared/service/api/api.service';
import {
  BehaviorSubject,
  combineLatest,
  firstValueFrom,
  startWith,
} from 'rxjs';
import { TasksDTO } from '@shared/service/api/DTOs/TasksDTO';
import { ModalService } from '@shared/service/modal/modal.service';
import { ToastService } from '@shared/service/utils/toast.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  providers: [ModalService, ApiService],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss'],
})
export class TasksComponent implements OnInit {
  private modalService = inject(ModalService);
  private apiService = inject(ApiService);
  private route = inject(ActivatedRoute);
  private toast = inject(ToastService);

  public projetoId!: number;
  public nomeProjeto = '';

  public dadosTarefas$ = new BehaviorSubject<TasksDTO[]>([]);
  public tarefasFiltradas$ = new BehaviorSubject<TasksDTO[]>([]);
  public tarefasPaginadas$ = new BehaviorSubject<TasksDTO[]>([]);

  public responsaveis: string[] = [];

  public paginaAtual = 1;
  public itensPorPagina = 5;
  public totalPaginas = 0;
  public totalItens = 0;
  public Math = Math;

  public filtroStatusControl = new FormControl<
    'todas' | 'pendentes' | 'andamento' | 'concluidas' | 'canceladas'
  >('todas', { nonNullable: true });

  public filtroPrioridadeControl = new FormControl<'todas' | '0' | '1' | '2'>(
    'todas',
    { nonNullable: true }
  );

  public filtroResponsavelControl = new FormControl<string>('todos', {
    nonNullable: true,
  });

  async ngOnInit() {
    this.projetoId = Number(this.route.snapshot.paramMap.get('id'));
    await this.buscarProjeto(this.projetoId);
    await this.buscarTarefas(this.projetoId);

    combineLatest([
      this.dadosTarefas$,
      this.filtroStatusControl.valueChanges.pipe(
        startWith(this.filtroStatusControl.value)
      ),
      this.filtroPrioridadeControl.valueChanges.pipe(
        startWith(this.filtroPrioridadeControl.value)
      ),
      this.filtroResponsavelControl.valueChanges.pipe(
        startWith(this.filtroResponsavelControl.value)
      ),
    ]).subscribe(([_, statusFiltro, prioridadeFiltro, responsavelFiltro]) => {
      this.paginaAtual = 1;
      this.aplicarFiltros(statusFiltro, prioridadeFiltro, responsavelFiltro);
    });

    this.tarefasFiltradas$.subscribe(() => {
      this.aplicarPaginacao();
    });
  }

  async buscarProjeto(projetoId: number) {
    try {
      const projeto = await firstValueFrom(
        this.apiService.getProjectById(projetoId.toString())
      );
      this.nomeProjeto = projeto.nome || 'Projeto sem nome';
    } catch (error) {
      console.error('Erro ao buscar projeto:', error);
      this.nomeProjeto = 'Projeto';
      this.showToast('error', 'Erro ao carregar informações do projeto');
    }
  }

  async buscarTarefas(projetoId: number) {
    const tarefas = await firstValueFrom(
      this.apiService.getTasks(projetoId.toString())
    );
    this.dadosTarefas$.next(tarefas);

    this.responsaveis = Array.from(
      new Set(
        tarefas
          .map((t) => t.responsavelId)
          .filter((r): r is string => typeof r === 'string' && r.trim() !== '')
      )
    ).sort();
  }

  private aplicarFiltros(
    statusFiltro:
      | 'todas'
      | 'pendentes'
      | 'andamento'
      | 'concluidas'
      | 'canceladas',
    prioridadeFiltro: 'todas' | '0' | '1' | '2',
    responsavelFiltro: string
  ) {
    const tarefas = this.dadosTarefas$.value;
    let filtradas = [...tarefas];

    switch (statusFiltro) {
      case 'pendentes':
        filtradas = filtradas.filter((t) => t.status === 0);
        break;
      case 'andamento':
        filtradas = filtradas.filter((t) => t.status === 1);
        break;
      case 'concluidas':
        filtradas = filtradas.filter((t) => t.status === 2);
        break;
      case 'canceladas':
        filtradas = filtradas.filter((t) => t.status === 3);
        break;
    }

    if (prioridadeFiltro !== 'todas') {
      const prioridadeNum = Number(prioridadeFiltro);
      filtradas = filtradas.filter((t) => t.prioridade === prioridadeNum);
    }

    if (responsavelFiltro !== 'todos') {
      filtradas = filtradas.filter(
        (t) => t.responsavelId === responsavelFiltro
      );
    }

    this.tarefasFiltradas$.next(filtradas);
  }

  private aplicarPaginacao() {
    const tarefasFiltradas = this.tarefasFiltradas$.value;
    this.totalItens = tarefasFiltradas.length;
    this.totalPaginas = Math.ceil(this.totalItens / this.itensPorPagina);

    if (this.paginaAtual > this.totalPaginas && this.totalPaginas > 0) {
      this.paginaAtual = this.totalPaginas;
    }

    const inicio = (this.paginaAtual - 1) * this.itensPorPagina;
    const fim = inicio + this.itensPorPagina;
    const tarefasPaginadas = tarefasFiltradas.slice(inicio, fim);

    this.tarefasPaginadas$.next(tarefasPaginadas);
  }

  mudarPagina(novaPagina: number) {
    if (novaPagina < 1 || novaPagina > this.totalPaginas) return;
    this.paginaAtual = novaPagina;
    this.aplicarPaginacao();
  }

  mudarItensPorPagina(quantidade: number) {
    this.itensPorPagina = quantidade;
    this.paginaAtual = 1;
    this.aplicarPaginacao();
  }

  get paginasVisiveis(): number[] {
    const paginas: number[] = [];
    const maxPaginasVisiveis = 5;
    let inicio = Math.max(
      1,
      this.paginaAtual - Math.floor(maxPaginasVisiveis / 2)
    );
    let fim = Math.min(this.totalPaginas, inicio + maxPaginasVisiveis - 1);

    if (fim - inicio < maxPaginasVisiveis - 1) {
      inicio = Math.max(1, fim - maxPaginasVisiveis + 1);
    }

    for (let i = inicio; i <= fim; i++) {
      paginas.push(i);
    }

    return paginas;
  }

  openTaskModal() {
    const { componentRef } = this.modalService.openModal(
      ModalCreateTaskComponent
    );
    componentRef.instance.atualizarTarefas.subscribe((deveAtualizar) => {
      if (deveAtualizar) this.buscarTarefas(this.projetoId);
    });
  }

  async editarTarefa(id: number) {
    const tarefa = await firstValueFrom(
      this.apiService.getTaskById(id.toString())
    );
    const { componentRef } = this.modalService.openModal(
      ModalCreateTaskComponent
    );
    componentRef.instance.dadosTarefa = tarefa;
    componentRef.instance.atualizarTarefas.subscribe((deveAtualizar) => {
      if (deveAtualizar) this.buscarTarefas(this.projetoId);
    });
  }

  async deletarTarefa(id: number) {
    const confirmacao = await Swal.fire({
      title: 'Excluir tarefa?',
      text: 'Esta ação não poderá ser desfeita!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, excluir',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#e63946',
      cancelButtonColor: '#6c757d',
      reverseButtons: true,
    });

    if (!confirmacao.isConfirmed) return;

    try {
      await firstValueFrom(this.apiService.deleteTask(id));
      this.showToast('success', 'Tarefa excluída com sucesso!');
      this.buscarTarefas(this.projetoId);
    } catch (err: any) {
      const msg = err?.error?.message ?? 'Erro ao excluir a tarefa.';
      this.showToast('error', msg);
      console.error(err);
    }
  }

  async atualizarStatus(
    tarefa: TasksDTO,
    acao: 'iniciar' | 'concluir' | 'cancelar'
  ) {
    let novoStatus = tarefa.status;
    let mensagem = '';
    let tipo: 'success' | 'info' | 'error' = 'info';

    if (acao === 'cancelar') {
      const confirmacao = await Swal.fire({
        title: 'Cancelar tarefa?',
        text: 'Tem certeza que deseja cancelar esta tarefa?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim, cancelar',
        cancelButtonText: 'Voltar',
        confirmButtonColor: '#e63946',
        cancelButtonColor: '#6c757d',
        reverseButtons: true,
      });

      if (!confirmacao.isConfirmed) return;
    }

    switch (acao) {
      case 'iniciar':
        if (tarefa.status !== 0) return;
        novoStatus = 1;
        mensagem = 'Tarefa iniciada com sucesso!';
        tipo = 'info';
        break;

      case 'concluir':
        if (tarefa.status !== 1) return;
        novoStatus = 2;
        mensagem = 'Tarefa concluída com sucesso!';
        tipo = 'success';
        break;

      case 'cancelar':
        if (![0, 1].includes(tarefa.status)) return;
        novoStatus = 3;
        mensagem = 'Tarefa cancelada com sucesso!';
        tipo = 'error';
        break;
    }

    try {
      await firstValueFrom(
        this.apiService.updateTaskStatus(tarefa.id, novoStatus)
      );
      await this.buscarTarefas(this.projetoId);

      this.showToast(tipo, mensagem);
    } catch (error: any) {
      const mensagemErro =
        error?.error?.message ?? 'Erro ao atualizar o status da tarefa.';
      this.showToast('error', mensagemErro);
      console.error(error);
    }
  }

  private showToast(
    icon: 'success' | 'error' | 'info' | 'warning',
    title: string
  ) {
    Swal.fire({
      toast: true,
      position: 'top-end',
      icon,
      title,
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      background: '#fff',
    });
  }
}
