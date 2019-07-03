# AdmInmuebles Netcore 2.2 API with Angular7 and Material2 Web App

TODO complete the description of AdmInmuebles Stack

### Introduction

Includes the following features:

* Social Login (Google and Facebook)
* Google Tag Manager
* NewRelic APM and Browsers
* Multi lenguages support
* Multi domain CORS

  
### How to start

```bash
#### Build docker image
> docker-compose build
#### Start Web App
> docker-compose up -d
```
  
### CI / CD
  
The process is fully automated based on the following branches mapping:
  
> Develop or feature branches -> Local environment  
> Staging -> Develop (https://admin-inmuebles-dev.herokuapp.com)  
> Released -> Production (https://www.adminmuebles.cl / https://api.adminmuebles.cl)  


### Notifications

All internal communications are in https://adminmuebles.slack.com at the moment we have the following groups:  
  
* Pipelines: Here are all the GitHub notifications
* NewRelic: WIP This group is for monitoring and alerts

### Load test

Includes the following k6 scripts:  
  
> loginTest.js  -> Load test /login endpoint  
