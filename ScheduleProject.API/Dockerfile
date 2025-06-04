# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ScheduleProject.API/ScheduleProject.API.csproj", "ScheduleProject.API/"]
COPY ["ScheduleProject.Application/ScheduleProject.Application.csproj", "ScheduleProject.Application/"]
COPY ["ScheduleProject.Core/ScheduleProject.Core.csproj", "ScheduleProject.Core/"]
COPY ["ScheduleProject.Infrastructure/ScheduleProject.Infrastructure.csproj", "ScheduleProject.Infrastructure/"]
RUN dotnet restore "./ScheduleProject.API/ScheduleProject.API.csproj"
COPY . .
WORKDIR "/src/ScheduleProject.API"
RUN dotnet build "./ScheduleProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ScheduleProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScheduleProject.API.dll"]