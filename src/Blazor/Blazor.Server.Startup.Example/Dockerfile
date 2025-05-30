#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blazor/Startup.Blazor.Server/Startup.Blazor.Server.csproj", "Blazor/Startup.Blazor.Server/"]
COPY ["Startup.Business/Startup.Business.csproj", "Startup.Business/"]
COPY ["Startup.Common/Startup.Common.csproj", "Startup.Common/"]
COPY ["Startup.Data/Startup.Data.csproj", "Startup.Data/"]
RUN dotnet restore "./Blazor/Startup.Blazor.Server/./Startup.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/Blazor/Startup.Blazor.Server"
RUN dotnet build "./Startup.Blazor.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Startup.Blazor.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Startup.Blazor.Server.dll"]