# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore
# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
ENV SERVER=host.docker.internal
ENV PORT=1433
ENV DATABASE=ElProyecteGrande
ENV USERID=sa
ENV PASSWORD=hgUrjkl8in12
ENV ENCRYPT=false
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ElProyecteGrandeBackend.dll"]
ENV ASPNETCORE_URLS=http://+:5073
EXPOSE 5073