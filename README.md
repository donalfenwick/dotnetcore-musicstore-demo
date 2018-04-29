# Music store application #

This project is a demo implementation of a music store application using aspnet core 2.0, IdentityServer 4 and Angular 5 and was undertaken to learn and gain experience in each of those technologies.

## Applications ##
This repository contains the following dotnet core applications.

- ** Frontend **   
Frontend music store application implemented using Angular 5 and authenticates against [IdentityServer4](http://docs.identityserver.io/en/release/) using OpenID connect.  
This application also hosts the applications API implemented using aspnet webapi controllers.
  
- ** IdentityServer **    
Implementation of [IdentityServer4](http://docs.identityserver.io/en/release/) within an aspnet core application that is used for authenticating users via OpenIdConnect protocol and issuing JWT Bearer tokens for accessing api services in the frontend project.
  
- ** AdminServer **  
An MVC application written in aspnet core 2.0 that serves as an administration portal for managing content

- ** DatabaseSeeder **  
Application to create the required databases and populate them with initial data (such as the identity server config)

## How do I get set up? ###

** 1: Configure connectionstrings and database provider **  
To set the database connection string in each dotnet project's `appsettings.json` configuration file, edit and run either of the following scripts. 

> `python3 set-cn-strings.py`  
or  
> `powershell -command .\set-cn-strings.ps1`  

*NB: Open the .py/.ps1 file and modify connection string with your own database credentials before executing the script.*

** 2: Bootstrap the database **  
Execute the following commands to run the `DatabaseSeeder` console app to create the database tables and apply configuration/seed data to the DB.  

> `cd DatabaseSeeder; dotnet run`  


** 3: Start the web applications **  


Start the frontend application (`http://localhost:5600`)
> `cd frontend; dotnet run`  

Start the identity server web app (`http://localhost:5601`)
> `cd IdentityServer; dotnet run`  

Default login credentials are `username: testuser@mysite.com, password: Pa$$word1`.   
  
***  
  
To start the management application (http://localhost:5604)
> `cd adminsite; dotnet run`  
    

### Running tests ###

> `cd tests; dotnet test`  
> `cd frontened/clientapp; ng test -sm=false`  
