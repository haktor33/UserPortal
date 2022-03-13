# User Portal and Management Applicatians (Used Docker and Apache Kafka)
## Install Docker Desktop on Windows
https://docs.docker.com/desktop/windows/install

### Compose Configure

docker-compose build

docker-compose up

### Run Migaration File
#### In the package manager console window, we will execute the commands below:

dotnet ef database update --project UserPortal

#### Use Replica

docker-compose up --scale userportal=2 --scale management=3 -d


## If you want to test project on debug mode without docker then  change docker-compose.yml commands below:

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

## After run code below

docker-compose up