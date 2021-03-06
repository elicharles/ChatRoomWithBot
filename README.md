# ChatRoomWithBot
This is a Chat Room application with bot capabilities.

## Features
  - Chat Room for talking with other users
  - .NET Identity Core for users authentication
  - Consume Chat Bots

## Technical Details
The back end was built in [.NET Core 3.1](https://dotnet.microsoft.com/download), which implements a microservices architecture using [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs) with [MeadiatR](https://github.com/jbogard/MediatR).

For real time communications, [SignalR](https://github.com/dotnet/AspNetCore/tree/master/src/SignalR) was used between users, which is consumed over [MeadiatR](https://github.com/jbogard/MediatR) notifications and events, and [RabbitMQ](https://www.rabbitmq.com/install-windows.html) for handling bots requests and responses.

The front end was done in [Angular 8](https://angular.io/)

### Projects Structure

| Project | Description |
| ------ | ------ |
| ChatRoomWithBot | Web Application with Angular |
| ChatRoom.Appication | Application Core Class Library | 
| ChatRoom.ComService | Class Library for communication services: SignalR & RabbitMQ | 
| ChatRoom.Domain | Class Library for application related domain classes | 
| ChatRoom.Persistence | Data layer project | 
| ChatRoom.ChatBot.Domain | Class Library for Chatbots related domain classes | 
| ChatRoom.ChatBot | Console App for handling Bot events | 
| ChatRoom.XUnitTest | Test Project |  

### Requirements

  - [RabbitMQ](https://www.rabbitmq.com/install-windows.html) Server (Requires [ERLang/OTP](https://www.erlang.org/downloads))
  - [.NET Core 3.1](https://dotnet.microsoft.com/download)
  - [LocalDB Feature](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - [NodeJS 12](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - [Angular CLI](https://cli.angular.io/)
 
## Configuration

Database Server, Message Queue Server and Developer SDKs and Tools Requirements should be installed before the next setting up section.
Alternatively, You can create [Docker](https://www.docker.com/products/docker-desktop) images to deploy as following:
  - For SQL Server
  ```sh
  $ docker pull mcr.microsoft.com/mssql/server
  $ docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD={Your-Password}’ -p 1433:1433 -d --restart=always --name={Your-Server-Name} hostname={Your-Server-Name} mcr.microsoft.com/mssql/server:latest
  ```
  - For Rabbit MQ
  ```sh
  $ docker run -d --hostname {my-rabbit-name}  --name {my-rabbit-name} -p 15672:15672 -p 5672:5672 -e RABBITMQ_DEFAULT_USER=guess -e RABBITMQ_DEFAULT_PASS=guess -e RABBITMQ_ERLANG_COOKIE='rabbitcookie' rabbitmq:3-management
  ```
  - For the application
  ```sh
  $ docker build -t aspnetapp .
  $ docker run -d -p 8080:80 --name myapp aspnetapp
  ```
### Settings Files

  - [ChatRoomWithBot/appsettings.json](ChatRoomWithBot/appsettings.json) File with `ConnectionStrings` and `RabbitMQ Server credentials` for receiving queue messages. LocalDB is set by default, but You can define another location
  - [ChatBot/ChatRoom.ChatBot/appsettings.json](ChatBot/ChatRoom.ChatBot/appsettings.json) File with `Stocks bot` settings and `RabbitMQ Server credentials` for sending and receiving bot responses.

### Setup Application

  * For _Database deployment_, you need to run migrations from data layer project directory to create the database:
    - In Visual Studio, use the Package Manager Console to scaffold a new migration and apply it to the database:
    ```sh
    PM> Add-Migration [migration name] 
    PM> Update-Database
    ```
    - Alternatively, you can scaffold a new migration and apply it from a command prompt at your project directory:
      ```sh
      > cd {ChatRoom.Persistence folder}
      > dotnet ef migrations add [migration name] 
      > dotnet ef database update
      ```
      
  * For _dependencies downloads_, restore dotnet nuget pakages and install npm packages in command promt:
    ```sh
    > cd {Solution Source directory}
    > dotnet restore
    > cd {Angular ClientApp directory}
    > npm install
    ```
    
  * Multiple startup projects should be set:
    - ChatRoom.ChatBot
    - ChatRoomWithBot
  - Or You can run from terminal:
    ```sh
    > dotnet run
    ```
  
## Unit Tests

For testing, You can run from Visual Studio or by terminal:

```sh
> cd {test project folder}
> dotnet test
```
