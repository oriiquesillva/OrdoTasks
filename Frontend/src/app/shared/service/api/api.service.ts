import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DashboardMetrics } from './DTOs/DashboardMetrics';
import { environment } from '../../../../environments/environment';
import {
  ProjectDTO,
  RequestProjectDTO,
  RequestUpdateProjectDTO,
} from './DTOs/ProjectDTO';
import {
  RequestUpdateTaskDTO,
  TaskRequestDTO,
  TasksDTO,
} from './DTOs/TasksDTO';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private httpCliente = inject(HttpClient);

  private readonly baseUrl = environment.apiUrl;

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  getMetricas(): Observable<DashboardMetrics> {
    return this.httpCliente.get<DashboardMetrics>(
      `${this.baseUrl}/metricas`,
      this.httpOptions
    );
  }

  getProjects(): Observable<ProjectDTO[]> {
    return this.httpCliente.get<ProjectDTO[]>(
      `${this.baseUrl}/api/Projetos`,
      this.httpOptions
    );
  }

  getProjectById(id: string): Observable<ProjectDTO> {
    return this.httpCliente.get<ProjectDTO>(
      `${this.baseUrl}/api/Projetos/${id}`
    );
  }

  createProject(payload: RequestProjectDTO): Observable<ProjectDTO> {
    return this.httpCliente.post<ProjectDTO>(
      `${this.baseUrl}/api/Projetos`,
      payload,
      this.httpOptions
    );
  }

  updateProject(payload: RequestUpdateProjectDTO): Observable<void> {
    return this.httpCliente.put<void>(
      `${this.baseUrl}/api/Projetos?id=${payload.id}`,
      payload,
      this.httpOptions
    );
  }

  deleteProject(id: string): Observable<void> {
    return this.httpCliente.delete<void>(
      `${this.baseUrl}/api/Projetos/${id}`,
      this.httpOptions
    );
  }

  getTasks(id: string): Observable<TasksDTO[]> {
    return this.httpCliente.get<TasksDTO[]>(
      `${this.baseUrl}/api/Tarefas?projetoId=${id}`,
      this.httpOptions
    );
  }

  createTask(payload: TaskRequestDTO): Observable<void> {
    return this.httpCliente.post<void>(
      `${this.baseUrl}/api/Tarefas`,
      payload,
      this.httpOptions
    );
  }

  getTaskById(id: string): Observable<TasksDTO> {
    return this.httpCliente.get<TasksDTO>(
      `${this.baseUrl}/api/Tarefas/${id}`,
      this.httpOptions
    );
  }

  updateTask(payload: RequestUpdateTaskDTO): Observable<void> {
    return this.httpCliente.put<void>(
      `${this.baseUrl}/api/Tarefas/${payload.id}`,
      payload,
      this.httpOptions
    );
  }

  updateTaskStatus(id: number, novoStatus: number): Observable<void> {
    return this.httpCliente.patch<void>(
      `${this.baseUrl}/api/Tarefas/${id}/status`,
      novoStatus,
      this.httpOptions
    );
  }

  deleteTask(id: number): Observable<void> {
    return this.httpCliente.delete<void>(
      `${this.baseUrl}/api/Tarefas/${id}`,
      this.httpOptions
    );
  }
}
