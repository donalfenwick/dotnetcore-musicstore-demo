# Build with dotnet cli  
Follow the instructions below to build and run each of the applications individually from the dotnet cli.

**1: Configure connectionstrings and database provider**  
To set the database connection string in each dotnet project's `appsettings.json` configuration file, edit and run either of the following scripts. 

> `python3 set-cn-strings.py`  
or  
> `powershell -command .\set-cn-strings.ps1`  

*NB: Open the .py/.ps1 file and modify connection string with your own database credentials before executing the script.*

**2: Bootstrap the database**  
Execute the following commands to run the `DatabaseSeeder` console app to create the database tables and apply configuration/seed data to the DB.  

> `cd DatabaseSeeder; dotnet run`  


**3: Start the web applications**  


Start the frontend application (`http://localhost:5600`)
> `cd src/frontend; dotnet run`  

Start the identity server web app (`http://localhost:5601`)
> `cd src/IdentityServer; dotnet run`  

Default login credentials are `username: testuser@mysite.com, password: Pa$$word1`.   
  
***  
  
To start and run the management application (`http://localhost:5604`)
`cd src/adminsite`
`npm install` (requires node)
`gulp` (requires gulp cli)
`dotnet run`  