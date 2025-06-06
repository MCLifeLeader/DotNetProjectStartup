#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api.Startup.Example/Api.Startup.Example.csproj", "Api/Api.Startup.Example/"]
COPY ["Startup.Business/Startup.Business.csproj", "Startup.Business/"]
COPY ["Startup.Common/Startup.Common.csproj", "Startup.Common/"]
COPY ["Startup.Data/Startup.Data.csproj", "Startup.Data/"]
RUN dotnet restore "./Api/Api.Startup.Example/./Api.Startup.Example.csproj"
COPY . .
WORKDIR "/src/Api/Api.Startup.Example"
RUN dotnet build "./Api.Startup.Example.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.Startup.Example.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Startup.Example.dll"]