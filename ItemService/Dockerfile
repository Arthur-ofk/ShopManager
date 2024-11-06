
# Етап 1: Відновлення залежностей і збірка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Встановлення необхідної бібліотеки
RUN apt-get update && apt-get install -y libkrb5-dev

# Копіюємо файл проекту і відновлюємо залежності
COPY ["ItemService/ItemService/ItemService.csproj", "ItemService/ItemService/"]
RUN dotnet restore "ItemService/ItemService/ItemService.csproj"

# Копіюємо весь код і збираємо його
COPY . .
WORKDIR "/src/ItemService/ItemService"
RUN dotnet build -c Release -o /app/build

# Етап 2: Публікація
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Етап 3: Налаштування та запуск
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копіюємо опубліковані файли
COPY --from=publish /app/publish .

# Налаштовуємо ASPNETCORE_URLS для прослуховування порту 80
#ENV ASPNETCORE_URLS=http://+:80

# Відкриваємо порт 80
EXPOSE 80

# Вказуємо команду запуску
ENTRYPOINT ["dotnet", "ItemService.dll"]