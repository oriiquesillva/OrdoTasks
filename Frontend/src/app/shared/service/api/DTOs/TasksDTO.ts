export interface TasksDTO {
  id: number;
  titulo: string;
  descricao: string;
  status: number;
  prioridade: number;
  projetoId: number;
  projetoNome?: string;
  responsavelId: string;
  dataPrazo: string;
}

export enum TasksStatus {
  Pendente = 0,
  EmAndamento = 1,
  Concluida = 2,
  Cancelada = 3,
}

export enum TasksPriority {
  Baixa = 0,
  Media = 1,
  Alta = 2,
}

export type TaskRequestDTO = Pick<
  TasksDTO,
  | 'titulo'
  | 'descricao'
  | 'status'
  | 'prioridade'
  | 'projetoId'
  | 'responsavelId'
  | 'dataPrazo'
>;

export type RequestUpdateTaskDTO = Pick<
  TasksDTO,
  | 'id'
  | 'titulo'
  | 'descricao'
  | 'status'
  | 'prioridade'
  | 'projetoId'
  | 'responsavelId'
  | 'dataPrazo'
>;
