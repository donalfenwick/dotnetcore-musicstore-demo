# Build with docker-compose
Follow the instructions below to build the application with [docker](https://www.docker.com).  
*Note: This requires that you first have docker installed on your OS.*

**Step 1:**  
If running docker on windows, first switch docker to use linux containers by choosing the "Switch to linux containers" command from the main docker menu.  

**Step 2:**  
Install the ssl cert on the host OS.

As the applications run in isolated containers within docker we cant simply use the default trusted ssl cert.  
Instead an included shared ssl cert is explicitly set in the kestrel configuration for each app. As a result we also need to install and trust this cert on the host OS for https to work in the browser.

The cert to install is located at `/src/Shared/DevCerts/aspnetapp-docker.pfx` and the password for the pfx file is **dockerDevAppPassword**  
  
- On windows [follow this tutoorial](https://blogs.technet.microsoft.com/sbs/2008/05/08/installing-a-self-signed-certificate-as-a-trusted-root-ca-in-windows-vista/) to install and trust the cert.
  
- On osx follow the instructions in [this video](https://tosbourn.com/getting-os-x-to-trust-self-signed-ssl-certificates/) to install and trust the cert.  
  

**Step 3:**  
Execute the `docker-compose build` command from a terminal in the repository root.   
This will build all of the docker images required for running the applications.  
  
*Note: This step may take some time as it will download the base images for building the application from docker hub.*
  
**Step 4:**  
Execute the `docker-compose up` command from a terminal in the repository root. 
This will start up each of the docker containers built in the last step.  
  
*Note: It may take a few moments for the SQLserver container to start and the seeder to finish populating the database so that the application is available.*
  
**Step 5:**  
Navigate to the running application in your browser.  
  
Applications will be avaiable under the following url's...  
frontend - `http://localhost:44350/`  
admin app - `http://localhost:44354/`  
identity server - `http://localhost:44353/`  
oauth test - `http://localhost:44357/` (test implementation of a server side implicit grant login with IdentityServer4)  
  
Default login credentials are username: **testuser@mysite.com**, password: **Pa$$word1**  
