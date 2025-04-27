FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CybergymHRS.API/CybergymHRS.API.csproj", "CybergymHRS.API/"]
COPY ["CybergymHRS.Infrastructure/CybergymHRS.Infrastructure.csproj", "CybergymHRS.Infrastructure/"]
COPY ["CybergymHRS.Core/CybergymHRS.Core.csproj", "CybergymHRS.Core/"]
RUN dotnet restore "CybergymHRS.API/CybergymHRS.API.csproj"
COPY . .
WORKDIR "/src/CybergymHRS.API"
RUN dotnet build "CybergymHRS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CybergymHRS.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "CybergymHRS.API.dll"]
