# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS publish
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore --runtime alpine-x64

# copy everything else and build app
COPY . .
RUN dotnet publish -c release -o /app --runtime alpine-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishReadyToRunShowWarnings=true
# squash 25Mo of useless kafka lib dedicated to distrib other than alpine
RUN echo "" > /app/centos7-librdkafka.so
RUN echo "" > /app/debian9-librdkafka.so


FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine as final-release
LABEL author="Lefebsy" \
      info="DotnetCore3.1 - Simple REST Json webservice feeding a kafka producer"
WORKDIR /app
COPY --from=publish /app ./
ENV ASPNETCORE_URLS "http://*:8080"
EXPOSE 8080/tcp
USER 1000
ENTRYPOINT ["/app/Json2Kafka"]