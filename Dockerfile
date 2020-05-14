# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore -r linux-musl-x64

# copy everything else and build app
COPY . .
RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true
#RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained false

# final stage/image
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
LABEL author="Lefebsy" \
      info="DotnetCore3.1 - Simple REST Json webservice feeding a kafka producer"
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80/tcp
#name of the csproj
ENTRYPOINT ["./Json2Kafka"] 