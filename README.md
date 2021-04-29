[![Build Status](https://travis-ci.com/lefebsy/json2kafka.svg?branch=master)](https://travis-ci.com/lefebsy/json2kafka)
![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/lefebsy/json2kafka)
![Docker Image Version (latest semver)](https://img.shields.io/docker/v/lefebsy/json2kafka?label=dockerhub)
[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/lefebsy/json2kafka)

# REST microService : Json to Kafka

- Code : `dotNetcore v5.0 - linux alpine self-contained compiled` 
- Docker images : <https://hub.docker.com/r/lefebsy/json2kafka/tags>
- Memory needed : ~ 30Mo 

# Main features

- Designed for kubernetes, all settings can be setted by environnements variables or a configmap `appsettings.json` file 
- Liveness and readiness probes ready `/health`
- Can publish on a Kafka with TLS and Sasl username/password
- The REST API can be exposed with a simple basicAuth login/password
- Logs `JSON formated` compliant with `kubernetes annotations elastic.co` to be parsed and displayed in Kibana
https://5001-violet-aphid-fbqy3u2p.ws-eu04.gitpod.io
# How to use

- Method : `POST`
- URI : `/api/msg`
- Content-Type : `application/json`
- Format : `UTF-8`
- Body : `Json document`
- [BasicAuth] : `username and password header`

Example : `curl -d '{"key1":"value1", "key2":"value2"}' -u admin:admin -H "Content-Type: application/json" -X POST https://host-xyz.gitpod.io/api/msg`

# Configuration
- kafka : [read the service settings](https://github.com/lefebsy/json2kafka/blob/master/appsettings.json)

# It is not !
- There is NO authentication impersonation, REST credentials are not played as Kafka credentials
- This code is not fully tested, it is a very simple project to test Kafka in dotnetCore microservice
