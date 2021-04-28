[![Build Status](https://travis-ci.com/lefebsy/json2kafka.svg?branch=master)](https://travis-ci.com/lefebsy/json2kafka)
![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/lefebsy/json2kafka)
![Docker Image Version (latest semver)](https://img.shields.io/docker/v/lefebsy/json2kafka?label=dockerhub)
[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/lefebsy/json2kafka)

# REST microService : Json to Kafka

- Code : `dotNetcore v3.1 - linux alpine self-contained compiled` 
- Docker images : <https://hub.docker.com/r/lefebsy/json2kafka/tags>
- Memory needed : ~ 30Mo 

# Main features

- Designed for kubernetes, all settings can be setted by environnements variables or a configmap `appsettings.json` file 
- Liveness and readiness probes ready `/health`
- Can publish on a Kafka with TLS and Sasl username/password
- The REST API can be exposed with a simple basicAuth login/password

# How to use

- Method : `POST`
- URI : `/api/msg`
- Content-Type : `application/json`
- Format : `UTF-8`
- Body : `Json document`
- [BasicAuth] : `username and password header`

# Configuration
- kafka : [read the service settings](https://github.com/lefebsy/json2kafka/blob/master/appsettings.json)

# It is not !
- There is NO authentication impersonation, REST credentials are not played as Kafka credentials
- This code is not fully tested, it is a very simple project to test Kafka in dotnetCore microservice
