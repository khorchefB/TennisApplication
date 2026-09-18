FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Tennis.Api/Tennis.Api.csproj", "Tennis.Api/"]
COPY ["Tennis.Application/Tennis.Application.csproj", "Tennis.Application/"]
COPY ["Tennis.Domain/Tennis.Domain.csproj", "Tennis.Domain/"]
COPY ["Tennis.Infrastructure/Tennis.Infrastructure.csproj", "Tennis.Infrastructure/"]

RUN dotnet restore "Tennis.Api/Tennis.Api.csproj"

COPY . .
WORKDIR /src/Tennis.Api
RUN dotnet publish "Tennis.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Tennis.Api.dll"]
