import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { OverlayRef } from '@angular/cdk/overlay';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ApiService } from '@shared/service/api/api.service';
import { firstValueFrom } from 'rxjs';
import { Router } from '@angular/router';
import {
  TasksDTO,
  TaskRequestDTO,
  RequestUpdateTaskDTO,
  TasksStatus,
  TasksPriority,
} from '@shared/service/api/DTOs/TasksDTO';

@Component({
  selector: 'app-modal-create-task',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  providers: [ApiService],
  templateUrl: './modal-task.component.html',
  styleUrls: ['./modal-task.component.scss'],
})
export class ModalCreateTaskComponent implements OnInit {
  private overlayRef = inject(OverlayRef);
  private fb = inject(FormBuilder);
  private apiService = inject(ApiService);
  private router = inject(Router);

  @Input() dadosTarefa!: TasksDTO;
  @Output() atualizarTarefas = new EventEmitter<boolean>();

  form: FormGroup = this.fb.group({
    id: [],
    titulo: ['', [Validators.required, Validators.minLength(3)]],
    status: ['', [Validators.required]],
    prioridade: ['', [Validators.required]],
    projetoId: ['', [Validators.required]],
    projetoNome: [''], // Adicionar campo para exibição
    responsavelId: ['', [Validators.required]],
    dataPrazo: ['', [Validators.required]],
    descricao: ['', [Validators.required, Validators.minLength(5)]],
  });

  statusOptions = Object.entries(TasksStatus)
    .filter(([key]) => isNaN(Number(key)))
    .map(([label, value]) => ({ label, value }));

  priorityOptions = Object.entries(TasksPriority)
    .filter(([key]) => isNaN(Number(key)))
    .map(([label, value]) => ({ label, value }));

  ngOnInit(): void {
    const url = this.router.url;
    const partes = url.split('/').filter(Boolean);
    const ultimo = partes[partes.length - 1];
    const projectId = Number(ultimo);

    if (Number.isFinite(projectId)) {
      this.form.patchValue({ projetoId: projectId });
    }

    if (this.dadosTarefa?.id) {
      const dataFormatada = this.formatarDataParaInput(
        this.dadosTarefa.dataPrazo
      );

      this.form.patchValue({
        id: this.dadosTarefa.id,
        titulo: this.dadosTarefa.titulo,
        descricao: this.dadosTarefa.descricao,
        status: this.dadosTarefa.status,
        prioridade: this.dadosTarefa.prioridade,
        projetoId: this.dadosTarefa.projetoId,
        projetoNome: this.dadosTarefa.projetoNome,
        responsavelId: this.dadosTarefa.responsavelId,
        dataPrazo: dataFormatada,
      });
    } else {
      this.form.patchValue({
        status: TasksStatus.Pendente,
        prioridade: TasksPriority.Baixa,
      });
      this.form.get('status')?.disable();
    }
  }

  private formatarDataParaInput(data: string): string {
    if (!data) {
      return '';
    }

    if (data.includes('T')) {
      const resultado = data.split('T')[0];
      return resultado;
    }

    // Se já estiver no formato yyyy-MM-dd
    if (data.match(/^\d{4}-\d{2}-\d{2}$/)) {
      return data;
    }

    if (data.includes('/')) {
      const partes = data.split('/');
      if (partes.length === 3) {
        const [dia, mes, ano] = partes;
        const resultado = `${ano}-${mes.padStart(2, '0')}-${dia.padStart(
          2,
          '0'
        )}`;
        return resultado;
      }
    }

    // Tentar criar um Date object como último recurso
    try {
      const dataObj = new Date(data);
      if (!isNaN(dataObj.getTime())) {
        const ano = dataObj.getFullYear();
        const mes = String(dataObj.getMonth() + 1).padStart(2, '0');
        const dia = String(dataObj.getDate()).padStart(2, '0');
        const resultado = `${ano}-${mes}-${dia}`;
        return resultado;
      }
    } catch (e) {
      console.error('Erro ao converter data:', e);
    }

    console.warn('Formato de data não reconhecido:', data);
    return '';
  }

  fechar(): void {
    this.overlayRef.dispose();
  }

  async criarTarefa(task: TaskRequestDTO) {
    await firstValueFrom(this.apiService.createTask(task));
  }

  async atualizarTarefa(task: RequestUpdateTaskDTO) {
    await firstValueFrom(this.apiService.updateTask(task));
  }

  async salvar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const tarefa = this.form.getRawValue() as TasksDTO;

    if (!tarefa.id) {
      const requestDTO: TaskRequestDTO = {
        titulo: tarefa.titulo,
        descricao: tarefa.descricao,
        status: Number(tarefa.status),
        prioridade: Number(tarefa.prioridade),
        projetoId: Number(tarefa.projetoId),
        responsavelId: tarefa.responsavelId,
        dataPrazo: tarefa.dataPrazo,
      };

      await this.criarTarefa(requestDTO);
    } else {
      const requestUpdateDTO: RequestUpdateTaskDTO = {
        id: tarefa.id,
        titulo: tarefa.titulo,
        descricao: tarefa.descricao,
        status: Number(tarefa.status),
        prioridade: Number(tarefa.prioridade),
        projetoId: Number(tarefa.projetoId),
        responsavelId: tarefa.responsavelId,
        dataPrazo: tarefa.dataPrazo,
      };

      await this.atualizarTarefa(requestUpdateDTO);
    }

    this.atualizarTarefas.emit(true);
    this.fechar();
  }
}
