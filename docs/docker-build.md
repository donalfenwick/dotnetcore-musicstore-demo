# Build with docker-compose
Follow the instructions below to build the application with [docker](https://www.docker.com).  
*Note: This requires that you first have docker installed on your OS.*

**Step 1:**  
If running docker on windows, first switch docker to use linux containers by choosing the "Switch to linux containers" command from the main docker menu.  

**Step 2:**  
Execute the `docker-compose build` command from a terminal in the repository root.   
This will build all of the docker images required for running the applications.  
  
*Note: This step may take some time as it will download the base images for building the application.*
  
**Step 3:**  
Execute the `docker-compose up` command from a terminal in the repository root. 
This will start up each of the docker containers built in the last step.  
  
*Note: It may take a few moments for the SQLserver container to start and the seeder to finish populating the database so the application is available.*
  
**Step 4:**  
Navigate to the running application in your browser.  
  
Applications will be avaiable under the following url's...  
frontend - `http://localhost:5600/`  
admin app - `http://localhost:5604/`  
identity server - `http://localhost:5601/`  
oauth test - `http://localhost:5607/` (test implementation of a server side implicit grant login with IdentityServer4)  
