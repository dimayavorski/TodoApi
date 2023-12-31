#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT="Azure"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TodoApi/TodoApi.csproj", "TodoApi/"]
COPY ["TodoApi.Infrastructure/TodoApi.Infrastructure.csproj", "TodoApi.Infrastructure/"]
COPY ["TodoApi.Application/TodoApi.Application.csproj", "TodoApi.Application/"]
COPY ["TodoApi.Core/TodoApi.Core.csproj", "TodoApi.Core/"]
RUN dotnet restore "TodoApi/TodoApi.csproj"
COPY . .
WORKDIR "/src/TodoApi"
RUN dotnet build "TodoApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]