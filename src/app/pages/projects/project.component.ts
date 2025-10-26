import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiService } from '@shared/service/api/api.service';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { ProjectDTO } from '@shared/service/api/DTOs/ProjectDTO';
import { ModalCriarProjetoComponent } from './components/modal-project/modal-project.component';
import { CommonModule } from '@angular/common';
import { ModalService } from '@shared/service/modal/modal.service';
import { TasksDTO, TasksStatus } from '@shared/service/api/DTOs/TasksDTO';

interface ProjectWithProgress extends ProjectDTO {
  progressoPercentual?: number;
}

@Component({
  selector: 'app-projects',
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
      }
    });
  }

  editarProjeto(project: ProjectDTO) {
    this.openProjectModal(project);
  }

  async deletarProjeto(id: string) {
    try {
      await firstValueFrom(this.apiService.deleteProject(id));
      this.recarregarProjetos();
    } catch (error: any) {
      if (error?.status === 400) {
        window.alert(error.error.message);
      }
    }
  }

  private async recarregarProjetos() {
    const dadosProjeto = await firstValueFrom(this.apiService.getProjects());

    const projetosComProgresso = await Promise.all(
      dadosProjeto.map(async (projeto) => {
        const progresso = await this.calcularProgresso(projeto.id.toString());
        console.log(progresso, projeto);
        return {
          ...projeto,
          progressoPercentual: progresso,
        };
      })
    );

    this.dadosProjeto$.next(projetosComProgresso);
  }

  private async calcularProgresso(projetoId: string): Promise<number> {
    try {
      const tasks = await firstValueFrom(this.apiService.getTasks(projetoId));
      console.log(tasks);

      if (!tasks || tasks.length === 0) {
        return 0;
      }

      const tarefasConcluidas = tasks.filter(
        (task: TasksDTO) => task.status === TasksStatus.Concluida
      ).length;
      console.log(tarefasConcluidas);
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
}
