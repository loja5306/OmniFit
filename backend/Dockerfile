FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY ["src/OmniFit.Api/OmniFit.Api.csproj", "src/OmniFit.Api/"]
COPY ["src/OmniFit.Application/OmniFit.Application.csproj", "src/OmniFit.Application/"]
COPY ["src/OmniFit.Infrastructure/OmniFit.Infrastructure.csproj", "src/OmniFit.Infrastructure/"]
COPY ["src/OmniFit.Domain/OmniFit.Domain.csproj", "src/OmniFit.Domain/"]
RUN dotnet restore "src/OmniFit.Api/OmniFit.Api.csproj"
COPY . .
WORKDIR /app/src/OmniFit.Api
RUN dotnet build OmniFit.Api.csproj

FROM build AS publish
RUN dotnet publish OmniFit.Api.csproj -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OmniFit.Api.dll"]