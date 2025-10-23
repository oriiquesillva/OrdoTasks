using Microsoft.AspNetCore.Mvc;
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
    public class GetAllProjectsUseCase
    {
        private readonly IProjetoRepository _projetoRepository;

        public GetAllProjectsUseCase(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<List<Projeto>> Run()
        {
            var projetos = await _projetoRepository.GetAllAsync();

            return projetos.ToList();
        }
    }
}
