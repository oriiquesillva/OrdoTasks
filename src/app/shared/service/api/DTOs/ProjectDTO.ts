export interface ProjectDTO {
  id: number;
  nome: string;
  descricao: string;
  dataCriacao: string;
}

export type RequestProjectDTO = Pick<ProjectDTO, 'nome' | 'descricao'>;
export type RequestUpdateProjectDTO = Omit<ProjectDTO, 'dataCriacao'>;
