# The Great Commission Foundation Technical Assessment
Take home technical assessment for developer interviews @thegc.org

## Pre-requisites

1. Install .Net 9.0  
https://dotnet.microsoft.com/en-us/download

2. Install latest Node.JS (used v23)  
https://nodejs.org/en/download

3. Install SQLite  
https://www.tutorialspoint.com/sqlite/sqlite_installation.htm

4. Recommended: Use Visual Studio Code  
https://code.visualstudio.com/Download  
_Install the C# Dev Kit extension on VSCode_  
https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit

## How to get started

1. Download the package or clone the repository 
2. Downloaded required prerequisites needed as shared above
3. In Backend folder, execute the Program.cs file through the terminal using dotnet run or using makefile or IDE functions
7. In the terminal if its displaying that is working, open the link where it is displaying "Now listening on: http://localhost:1234"
   {in my case it was http://localhost:5050; you can also use the SwaggerUI interface}
8. After the backend is working, execute the front end file - App.js through npm start in terminal or using makefile or using IDE functions
9. If it isn't allowing to execute the react file due to a permission issue use the command line below in the terminal "Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass"
10. After the execution, the react app should open and display the output on the default browser
11. The output will display fields to enter details for the donors, top donor, total amount collected as well as the table of donors existing donors
  
## Useful make commands for the repo

`dotnet restore` 
.Net command that downloads and install all project dependencies

`dotnet build` 
.Net command that builds the project and compiles the package into development workflow

`dotnet run` 
.Net command that executes the package and launches the application

`npm start` 
builds the react app and launches the app

`make all`  
_builds both frontend react app, and backend dotnet app, recommended to run before starting to code_  
  
`make frontend`  
_builds frontend react app (npm start)_  

`make backend`  
_builds backend dotnet app_  

`make run-test`  
_builds backend and runs the NUnit tests in the test/be-test directory, recommended to run before starting to code_  

`make init_db`  
_sets up the SQLite DB for the dotnet project and creates the following table_

```
PRAGMA table_info(donations);  
0|id|INTEGER|0||1  
1|donor_name|TEXT|1||0  
2|amount|REAL|1||0  
3|date|TEXT|1||0
```

`make terminate`  
_kills all processes for port 5050 (backend), and 3000 (frontend)_

`make list-pid-unix`   
_lists PIDs for port 5050 and 3000_  

`make list-pid-windows`  
_lists PIDs for port 5050 and 3000_
