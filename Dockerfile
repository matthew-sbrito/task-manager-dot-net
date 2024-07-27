#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080

ARG ENVIRONMENT_NAME
ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT_NAME}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ARG ENVIRONMENT_NAME
ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT_NAME}

COPY . .
RUN dotnet restore "Api/Api.csproj"

WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:EnvironmentName=${ENVIRONMENT_NAME}

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]