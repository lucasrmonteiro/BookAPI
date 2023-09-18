FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BookAPI/BookAPI.csproj", "BookAPI/"]
RUN dotnet restore "BookAPI/BookAPI.csproj"
COPY . .
WORKDIR "/src/BookAPI"
RUN dotnet build "BookAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookAPI.dll"]
