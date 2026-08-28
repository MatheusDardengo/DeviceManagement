# 1. Usa a imagem oficial do .NET 9 para rodar a aplicação (leve)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Usa a imagem oficial do SDK do .NET 9 para compilar o código (mais pesada)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 3. Copia o arquivo de projeto e restaura as dependências
COPY ["DeviceManagement.Api/DeviceManagement.Api.csproj", "DeviceManagement.Api/"]
RUN dotnet restore "DeviceManagement.Api/DeviceManagement.Api.csproj"

# 4. Copia o resto do código e compila
COPY . .
WORKDIR "/src/DeviceManagement.Api"
RUN dotnet build "DeviceManagement.Api.csproj" -c Release -o /app/build

# 5. Publica a aplicação otimizada
FROM build AS publish
RUN dotnet publish "DeviceManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 6. Pega a imagem base lá do passo 1 e joga a aplicação publicada dentro dela
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceManagement.Api.dll"]