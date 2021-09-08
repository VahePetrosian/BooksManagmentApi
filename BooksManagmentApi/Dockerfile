FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BooksManagmentApi/BooksManagmentApi/BooksManagmentApi.csproj", "BooksManagmentApi/"]
RUN dotnet restore "BooksManagmentApi/BooksManagmentApi.csproj"
COPY . .
WORKDIR "/src/BooksManagmentApi"
RUN dotnet build "BooksManagmentApi.csproj" -c Release -o /app/build
