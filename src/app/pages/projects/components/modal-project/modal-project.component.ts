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
import {
  ProjectDTO,
  RequestProjectDTO,
  RequestUpdateProjectDTO,
} from '@shared/service/api/DTOs/ProjectDTO';

@Component({
  selector: 'app-modal-criar-projeto',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  providers: [ApiService],
  templateUrl: './modal-project.component.html',
  styleUrls: ['./modal-project.component.scss'],
})
export class ModalCriarProjetoComponent implements OnInit {
  private overlayRef = inject(OverlayRef);
  private fb = inject(FormBuilder);
  private apiService = inject(ApiService);

  @Input() dadoProjeto!: ProjectDTO;
  @Output() atualizaProjetos = new EventEmitter<boolean>();

  form: FormGroup = this.fb.group({
    id: [],
    nome: ['', [Validators.required, Validators.minLength(3)]],
    descricao: ['', [Validators.required, Validators.minLength(5)]],
  });

  ngOnInit(): void {
    if (this.dadoProjeto?.id) {
      this.form.patchValue({
        id: this.dadoProjeto.id,
        nome: this.dadoProjeto.nome,
        descricao: this.dadoProjeto.descricao,
      });
    }
  }

  fechar(): void {
    this.overlayRef.dispose();
  }

  async criarProjeto(project: RequestProjectDTO) {
    await firstValueFrom(this.apiService.createProject(project));
  }

  async AtualizaProjeto(project: RequestUpdateProjectDTO) {
    await firstValueFrom(this.apiService.updateProject(project));
  }

  async salvar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const projeto: ProjectDTO = this.form.value;
    if (!projeto.id) {
      const requestDTO: RequestProjectDTO = {
        nome: projeto.nome,
        descricao: projeto.descricao,
      };
      await this.criarProjeto(requestDTO);
    } else {
      await this.AtualizaProjeto(projeto);
    }

    this.atualizaProjetos.emit(true);

    this.fechar();
  }
}
