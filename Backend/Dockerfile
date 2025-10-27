# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo e restaura dependências
COPY Backend/*.sln .
COPY Backend/OrdoTasksApplication/ OrdoTasksApplication/
COPY Backend/OrdoTasksDomain/ OrdoTasksDomain/
COPY Backend/OrdoTasksInfrastructure/ OrdoTasksInfrastructure/
COPY Backend/OrdoTasks/ OrdoTasks/

RUN dotnet restore "OrdoTasks/OrdoTasks.csproj"

# Build
WORKDIR /src/OrdoTasks
RUN dotnet publish -c Release -o /app/publish

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "OrdoTasks.dll"]
