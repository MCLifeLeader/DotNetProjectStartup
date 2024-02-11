#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Startup.Web/Startup.Web.csproj", "Web/Startup.Web/"]
COPY ["Startup.Data/Startup.Data.csproj", "Startup.Data/"]
COPY ["Startup.Common/Startup.Common.csproj", "Startup.Common/"]
RUN dotnet restore "./Web/Startup.Web/./Startup.Web.csproj"
COPY . .
WORKDIR "/src/Web/Startup.Web"
RUN dotnet build "./Startup.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Startup.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Startup.Web.dll"]