# MovieDatabase

Client-server application for movies.
Using Microsoft SQL Server together with .NET Razor pages.

## About The Project
![Video](/media/video.gif)

The Movie Database is a simple project I did for school. The point of this project is to utilize different technologies I have learnt and use them together.

### The Database
![image](/media/diagram.png)

The database managment system I chose is Microsoft SQL Server. I created just tables and indexes because I like to keep the logic in application. The data is imported from [TMDb](https://themoviedb.org).

### The Server Application

The server application runs on Microsoft's .NET Framework using the C# language.

### The Client Application

The client application is a web application using a few frameworks:

* [JQuery](https://jquery.com/)
* [Bootstrap](https://getbootstrap.com/)
* [Slick Carousel](https://kenwheeler.github.io/slick)

## Prerequisites
* Microsoft SQL Server - tested with version 18.3.1
* Visual Studio - tested with version 16.4.0
  * ASP.NET and web development workload
  * .NET desktop development workload

## Installation
* Clone the repository
```bash
git clone https://github.com/DanielSima/MovieDatabase.git
```
* database
  * Run [generatedDB.sql](/database/generatedDB.sql)
* client
  * Change the server address in [those classes](/client/MovieDatabase/TableClasses).  
  *I tried to put this into Web.config file but it just didn't work.* 😕
  * Run [MovieDatabase.sln](/client/MovieDatabase.sln). Packages should install automatically. If they do not, try `Update-Package -reinstall` in the Package Manager console (Tools > NuGet Package Manager > Package Manager Console).
