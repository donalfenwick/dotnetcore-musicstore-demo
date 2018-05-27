
[![Build Status](https://travis-ci.org/donalfenwick/dotnetcore-musicstore-demo.svg?branch=master)](https://travis-ci.org/donalfenwick/dotnetcore-musicstore-demo)

# Music store application

This project is a demo implementation of a music store application using aspnet core 2.0, IdentityServer 4 and Angular 6 and was undertaken to learn and gain experience in each of those technologies.

## Applications
This repository contains the following dotnet core applications.

- **Frontend**   
Frontend music store application implemented using [Angular 6](https://angular.io) and authenticates against [IdentityServer4](http://docs.identityserver.io/en/release/) using [OpenID connect](http://openid.net/connect/).  
This application also hosts the applications API implemented using aspnet webapi controllers.
  
- **IdentityServer**   
Implementation of [IdentityServer4](http://docs.identityserver.io/en/release/) within an aspnet core application that is used for authenticating users via OpenIdConnect protocol and issuing JWT Bearer tokens for accessing api services in the frontend project.
  
- **AdminSite**  
An MVC application written in aspnet core 2.0 that serves as an administration portal for managing content

- **DatabaseSeeder**  
Application to create the required databases and populate them with initial data (such as the identity server config)

## How do I get set up?

- **Build with docker**  
[Click here](docs/docker-build.md) for instructions to build with docker.  
  
- **Build apps individually with dotnet cli**  
[Click here](docs/local-build.md) for instructions to build each app via the dotnet cli commands.  
  
    
### Running tests

> `cd tests; dotnet test`  
> `cd frontend/clientapp; ng test -sm=false`  
