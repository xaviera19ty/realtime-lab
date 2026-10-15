FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/RealtimeLab.Api/RealtimeLab.Api.csproj src/RealtimeLab.Api/
RUN dotnet restore src/RealtimeLab.Api/RealtimeLab.Api.csproj
COPY src/ src/
RUN dotnet publish src/RealtimeLab.Api/RealtimeLab.Api.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
# curl 給 docker compose 的 healthcheck 用
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "RealtimeLab.Api.dll"]
