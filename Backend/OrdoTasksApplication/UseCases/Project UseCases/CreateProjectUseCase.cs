using OrdoTasksApplication.DTOs;
using OrdoTasksApplication.Exceptions.Projects;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Entities;
using OrdoTasksDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.UseCases.Project_UseCases
{
    public class CreateProjectUseCase
    {
        private readonly IOrdoTasksProjectRepository _projetoRepository;

        public CreateProjectUseCase(IOrdoTasksProjectRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<CreateProjectResult> Run(CreateProjectDTO projetoDto)
        {
            if (string.IsNullOrWhiteSpace(projetoDto.Nome))
                throw new InvalidProjectDataException();

            var projeto = new Projeto
            {
                Nome = projetoDto.Nome,
                Descricao = projetoDto.Descricao,
                DataCriacao = DateTime.UtcNow 
            };

            var id = await _projetoRepository.CreateAsync(projeto);
            projeto.Id = id;

            return new CreateProjectResult
            {
                Id = id,
                Projeto = projeto
            };
        }

    }
}
