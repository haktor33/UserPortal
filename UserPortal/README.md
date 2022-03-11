# Database Configuration
## Make sure you have already installed the dotnet-ef tool, which can be done something like this
dotnet tool install dotnet-ef -g

## Powershell/Commandline as to start adding the database and create our first initial tables
### To Add Database
dotnet ef database update
### To add the migration
dotnet ef migrations add InitialMigration

# Apache Kafka Using Docker Configuration
## Install Docker Desktop on Windows
https://docs.docker.com/desktop/windows/install/

##Creating a docker-compose.yml file
version: '2'
services:
  zookeeper:
    image: wurstmeister/zookeeper
    ports:
      - "2181:2181"
  kafka:
    image: wurstmeister/kafka
    ports:
      - "9092:9092"
    environment:
      KAFKA_ADVERTISED_HOST_NAME: 127.0.0.1
      KAFKA_CREATE_TOPICS: "UserPortalTopic:1:2,ManagementTopic:1:3"
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock


## Run Docker File
Navigate via the command line to the folder where you saved the docker-compose.yml file.
Then, run the following command to start the images:

docker-compose up

## To check if the Docker images are up, run the following command in another cmd window:

docker ps





