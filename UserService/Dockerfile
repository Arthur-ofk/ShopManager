# Етап 1: Збірка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
RUN apt-get update && apt-get install -y libkrb5-dev
# Копіюємо файл проекту і відновлюємо залежності
COPY ["UserService/UserService/UserService.csproj", "UserService/UserService/"]
RUN dotnet restore "UserService/UserService/UserService.csproj"

# Копіюємо весь код і збираємо його
COPY . .
WORKDIR "/app/UserService/UserService"
RUN dotnet build -c Release -o /app/build

# Етап 2: Публікація
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Етап 3: Налаштування та запуск
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Налаштовуємо ASPNETCORE_URLS для прослуховування порту 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "UserService.dll"]