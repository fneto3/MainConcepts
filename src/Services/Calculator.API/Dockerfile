#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Calculator.API/Calculator.API.csproj", "Services/Calculator.API/"]
COPY ["Core/ApplicationCore/ApplicationCore.csproj", "Core/ApplicationCore/"]
COPY ["Core/Infrastructure/Infrastructure.csproj", "Core/Infrastructure/"]
COPY ["docker-compose.dcproj" "docker-compose.dcproj"]

RUN dotnet restore "Services/Calculator.API/Calculator.API.csproj"
COPY . .
WORKDIR "/src/Services/Calculator.API"
RUN dotnet build "Calculator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calculator.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calculator.API.dll"]