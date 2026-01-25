# Многоступенчатая сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build  
WORKDIR /src

# Копируем исходники
COPY Swiga.API/ .
# Восстанавливаем зависимости
RUN dotnet restore "Swiga.API/Swiga.API.csproj"

WORKDIR "/src/Swiga.API"
# Собираем проект
RUN dotnet build "Swiga.API.csproj" -c Release -o /app/build

FROM build AS publish 
# Публикуем (упаковываем для запуска)
RUN dotnet publish "Swiga.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
WORKDIR /app
# Копируем только собранное приложение
COPY --from=publish /app/publish .
# Запускаем
ENTRYPOINT ["dotnet", "Swiga.API.dll"]