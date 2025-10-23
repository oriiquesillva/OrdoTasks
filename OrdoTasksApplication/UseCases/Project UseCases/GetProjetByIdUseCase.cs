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
    public class GetProjetByIdUseCase
    {
        private readonly IProjetoRepository _projetoRepository;

        public GetProjetByIdUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<Projeto> Run(int id)
        {
            var projeto = await _projetoRepository.GetByIdAsync(id);

            if (projeto == null)
            {
                throw new ProjectNotFoundException();
            }

            return projeto;
        }
    }
}
