using OrdoTasksApplication.Exceptions.Projects;
using OrdoTasksApplication.Exceptions.Tasks;
using OrdoTasksApplication.Interfaces;
using OrdoTasksDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdoTasksApplication.UseCases.Project_UseCases
{
    public class DeleteProjectUseCase
    {
        private readonly IOrdoTasksProjectRepository _projetoRepository;

        public DeleteProjectUseCase(IOrdoTasksProjectRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task Run(int id)
        {
            var verificaProjeto = await _projetoRepository.GetByIdAsync(id);

            if (verificaProjeto == null)
            {
                throw new ProjectNotFoundException();
            }

            bool verificaTarefas = await _projetoRepository.HasTarefasAsync(id);

            if (verificaTarefas)
            {
                throw new UnauthorizedDeleteProjectException();
            }

            await _projetoRepository.DeleteAsync(id);
        }
    }
}
