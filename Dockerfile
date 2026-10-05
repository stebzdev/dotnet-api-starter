FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
	 
COPY ["src/Starter.Api/Starter.Api.csproj", "src/Starter.Api/"]
COPY ["src/Starter.Application/Starter.Application.csproj", "src/Starter.Application/"]
COPY ["src/Starter.Domain/Starter.Domain.csproj", "src/Starter.Domain/"]
COPY ["src/Starter.Infrastructure/Starter.Infrastructure.csproj", "src/Starter.Infrastructure/"]
COPY ["src/Starter.ServiceDefaults/Starter.ServiceDefaults.csproj", "src/Starter.ServiceDefaults/"]

RUN	dotnet restore "src/Starter.Api/Starter.Api.csproj"

COPY . .

RUN dotnet publish "src/Starter.Api/Starter.Api.csproj" \ 
	--configuration Release \
	--no-restore \
	--output /app/publish \
	/p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

USER $APP_UID

EXPOSE 8080

ENTRYPOINT ["dotnet", "Starter.Api.dll"] 
