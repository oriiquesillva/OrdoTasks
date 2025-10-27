using OrdoTasksApplication.DTOs;
using OrdoTasksApplication.Exceptions.Projects;
using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.UseCases.Project_UseCases
{
    public class UpdateProjectUseCase
    {
        private readonly IOrdoTasksProjectRepository _projetoRepository;

        public UpdateProjectUseCase(IOrdoTasksProjectRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task Run(int id, UpdateProjectDTO projetoDTO)
        {
            var verificaProjeto = await _projetoRepository.GetByIdAsync(id);

            if (verificaProjeto == null)
                throw new ProjectNotFoundException();

            var projeto = new Projeto
            {
                Id = id,
                Nome = projetoDTO.Nome,
                Descricao = projetoDTO.Descricao,
                DataCriacao = verificaProjeto.DataCriacao
            };

            await _projetoRepository.UpdateAsync(projeto);
        }
    }
}
