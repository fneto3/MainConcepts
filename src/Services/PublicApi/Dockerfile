#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/PublicApi/PublicApi.csproj", "Services/PublicApi/"]
COPY ["Core/ApplicationCore/ApplicationCore.csproj", "Core/ApplicationCore/"]
COPY ["Core/Infrastructure/Infrastructure.csproj", "Core/Infrastructure/"]
COPY ["docker-compose.dcproj" "docker-compose.dcproj"]

RUN dotnet restore "Services/PublicApi/PublicApi.csproj"
COPY . .
WORKDIR "/src/Services/PublicApi"
RUN dotnet build "PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicApi.dll"]