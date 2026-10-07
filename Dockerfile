# Etapa 1: Base com SDK para compilar o código
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas os arquivos de projeto primeiro (melhora o tempo de build graças ao cache do Docker)
COPY ["SmartOrderManagement.sln", "./"]
COPY ["SmartOrderManagement.API/SmartOrderManagement.API.csproj", "SmartOrderManagement.API/"]
COPY ["SmartOrderManagement.Application/SmartOrderManagement.Application.csproj", "SmartOrderManagement.Application/"]
COPY ["SmartOrderManagement.Domain/SmartOrderManagement.Domain.csproj", "SmartOrderManagement.Domain/"]
COPY ["SmartOrderManagement.Infrastructure/SmartOrderManagement.Infrastructure.csproj", "SmartOrderManagement.Infrastructure/"]
RUN dotnet restore

# Copia o restante do código da API e compila
COPY . .
WORKDIR "/src/SmartOrderManagement.API"
RUN dotnet build "SmartOrderManagement.API.csproj" -c Release -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish "SmartOrderManagement.API.csproj" -c Release -o /app/publish

# Etapa 2: Base mais leve apenas com o Runtime (para rodar a aplicação)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define a porta que o container vai expor (No .NET 8 o padrão é 8080)
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "SmartOrderManagement.API.dll"]
