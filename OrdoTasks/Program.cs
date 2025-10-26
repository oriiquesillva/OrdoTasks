using OrdoTasks.Hubs;
using OrdoTasksApplication.Interfaces;
using OrdoTasksApplication.UseCases.DashboardUseCases;
using OrdoTasksApplication.UseCases.Project_UseCases;
using OrdoTasksApplication.UseCases.TasksUseCases;
using OrdoTasksInfrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// SERILOG
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

// CONTROLLERS / SWAGGER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SIGNALR
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200") // sua aplicação Angular
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// INJEÇÃO DE DEPENDÊNCIA DO PROJETO
builder.Services.AddScoped<IOrdoTasksProjectRepository, OrdoTasksProjectRepository>();
builder.Services.AddScoped<GetAllProjectsUseCase>();
builder.Services.AddScoped<GetProjetByIdUseCase>();
builder.Services.AddScoped<CreateProjectUseCase>();
builder.Services.AddScoped<UpdateProjectUseCase>();
builder.Services.AddScoped<DeleteProjectUseCase>();

// INJEÇÃO DE DEPENDÊNCIA DAS TAREFAS
builder.Services.AddScoped<IOrdoTasksTaskRepository, OrdoTasksTaskRepository>();
builder.Services.AddScoped<GetAllTasksUseCase>();
builder.Services.AddScoped<GetTaskByIdUseCase>();
builder.Services.AddScoped<CreateTaskUseCase>();
builder.Services.AddScoped<UpdateTaskUseCase>();
builder.Services.AddScoped<UpdateTaskStatusUseCase>();
builder.Services.AddScoped<DeleteTaskUseCase>();
builder.Services.AddScoped<GetDelayedTaskUseCase>();

// INJEÇÃO DE DEPENDÊNCIA DO DASHBOARD
builder.Services.AddScoped<DashboardUseCase>();


var app = builder.Build();

app.UseCors("AllowAngularApp");

// SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
