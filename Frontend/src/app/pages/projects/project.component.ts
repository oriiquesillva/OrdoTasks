import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiService } from '@shared/service/api/api.service';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { ProjectDTO } from '@shared/service/api/DTOs/ProjectDTO';
import { ModalCriarProjetoComponent } from './components/modal-project/modal-project.component';
import { CommonModule } from '@angular/common';
import { ModalService } from '@shared/service/modal/modal.service';
import { TasksDTO, TasksStatus } from '@shared/service/api/DTOs/TasksDTO';
import Swal from 'sweetalert2';

interface ProjectWithProgress extends ProjectDTO {
  progressoPercentual?: number;
}

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [RouterLink, CommonModule],
  providers: [ModalService, ApiService],
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss',
})
export class ProjectComponent implements OnInit {
  private apiService = inject(ApiService);
  public dadosProjeto$ = new BehaviorSubject<ProjectWithProgress[]>([]);
  private modalService = inject(ModalService);

  ngOnInit() {
    this.recarregarProjetos();
  }

  openProjectModal(project?: ProjectDTO) {
    const { componentRef } = this.modalService.openModal(
      ModalCriarProjetoComponent
    );

    if (project) {
      componentRef.instance.dadoProjeto = project;
    }

    componentRef.instance.atualizaProjetos.subscribe((deveAtualizar) => {
      if (deveAtualizar) {
        this.recarregarProjetos();
        this.showToast('success', 'Projeto salvo com sucesso!');
      }
    });
  }

  editarProjeto(project: ProjectDTO) {
    this.openProjectModal(project);
  }

  async deletarProjeto(id: string) {
    const confirmacao = await Swal.fire({
      title: 'Excluir projeto?',
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
      await firstValueFrom(this.apiService.deleteProject(id));
      this.showToast('success', 'Projeto excluído com sucesso!');
      this.recarregarProjetos();
    } catch (error: any) {
      const msg =
        error?.error?.message || 'Ocorreu um erro ao excluir o projeto.';
      this.showToast('error', msg);
      console.error(error);
    }
  }

  private async recarregarProjetos() {
    try {
      const dadosProjeto = await firstValueFrom(this.apiService.getProjects());

      const projetosComProgresso = await Promise.all(
        dadosProjeto.map(async (projeto) => {
          const progresso = await this.calcularProgresso(
            projeto.id.toString()
          );
          return {
            ...projeto,
            progressoPercentual: progresso,
          };
        })
      );

      this.dadosProjeto$.next(projetosComProgresso);
    } catch (error) {
      this.showToast('error', 'Erro ao carregar os projetos.');
      console.error(error);
    }
  }

  private async calcularProgresso(projetoId: string): Promise<number> {
    try {
      const tasks = await firstValueFrom(this.apiService.getTasks(projetoId));

      if (!tasks || tasks.length === 0) {
        return 0;
      }

      const tarefasConcluidas = tasks.filter(
        (task: TasksDTO) => task.status === TasksStatus.Concluida
      ).length;
      return Math.round((tarefasConcluidas / tasks.length) * 100);
    } catch (error) {
      console.error('Erro ao calcular progresso:', error);
      return 0;
    }
  }

  getProgressOffset(percentage: number): number {
    const circumference = 283;
    return circumference - (percentage / 100) * circumference;
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
