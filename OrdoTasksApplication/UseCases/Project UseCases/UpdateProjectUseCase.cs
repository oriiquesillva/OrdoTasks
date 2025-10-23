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
        private readonly IProjetoRepository _projetoRepository;

        public UpdateProjectUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task Run(int id, Projeto projeto)
        {
            var verifaProjeto = await _projetoRepository.GetByIdAsync(id);

            if (verifaProjeto == null)
            {
                throw new ProjectNotFoundException();
            }

            projeto.Id = id;

            await _projetoRepository.UpdateAsync(projeto);
        }
    }
}
