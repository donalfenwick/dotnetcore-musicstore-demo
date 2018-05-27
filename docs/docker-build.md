
If running docker on windows, first switch docker to use linux containers.  
  
run `docker-compose build` to build the docker image for each app
run `docker-compose up` to start up each container in docker

applications will be avaiable under the following url's

frontend - http://localhost:5600/
admin app - http://localhost:5604/
identity server - http://localhost:5601/
oauth test - http://localhost:5607/ (test implementation of a server side implicit grant login with IdentityServer4)