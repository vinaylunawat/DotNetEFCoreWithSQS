#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT="Production" 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrganizationSolution/Source/App/Geography.Service/Geography.Service.csproj", "OrganizationSolution/Source/App/Geography.Service/"]
COPY ["OrganizationSolution/Source/Framework/Framework.Configuration/Framework.Configuration.csproj", "OrganizationSolution/Source/Framework/Framework.Configuration/"]
COPY ["OrganizationSolution/Source/Framework/Framework.Constant/Framework.Constant.csproj", "OrganizationSolution/Source/Framework/Framework.Constant/"]
COPY ["OrganizationSolution/Source/Framework/Framework.Migrations/Framework.Migrations.csproj", "OrganizationSolution/Source/Framework/Framework.Migrations/"]
COPY ["OrganizationSolution/Source/Framework/Framework.Security/Framework.Security.csproj", "OrganizationSolution/Source/Framework/Framework.Security/"]
COPY ["OrganizationSolution/Source/App/Geography.Business/Geography.Business.csproj", "OrganizationSolution/Source/App/Geography.Business/"]
COPY ["OrganizationSolution/Source/Framework/Framework.Business/Framework.Business.csproj", "OrganizationSolution/Source/Framework/Framework.Business/"]
COPY ["OrganizationSolution/Source/Framework/Framework.DataAccess/Framework.DataAccess.csproj", "OrganizationSolution/Source/Framework/Framework.DataAccess/"]
COPY ["OrganizationSolution/Source/Framework/Framework.Entity/Framework.Entity.csproj", "OrganizationSolution/Source/Framework/Framework.Entity/"]
COPY ["OrganizationSolution/Source/Framework/Framework.Service/Framework.Service.csproj", "OrganizationSolution/Source/Framework/Framework.Service/"]
COPY ["OrganizationSolution/Source/App/Geography.DataAccess/Geography.DataAccess.csproj", "OrganizationSolution/Source/App/Geography.DataAccess/"]
COPY ["OrganizationSolution/Source/App/Geography.Entity/Geography.Entity.csproj", "OrganizationSolution/Source/App/Geography.Entity/"]
RUN dotnet restore "./OrganizationSolution/Source/App/Geography.Service/./Geography.Service.csproj"
COPY . .
WORKDIR "/src/OrganizationSolution/Source/App/Geography.Service"
RUN dotnet build "./Geography.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Geography.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Geography.Service.dll"]