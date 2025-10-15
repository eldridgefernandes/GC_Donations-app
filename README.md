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

1. Create github account  
2. Create New Repository  
3. Click "import a repository"  <img width="3304" height="1686" alt="image" src="https://github.com/user-attachments/assets/16f90c81-f9f9-4986-b672-1b383b2046dd" />
4. Use the clone URL from technical-assessment repo  <img width="2972" height="1708" alt="image" src="https://github.com/user-attachments/assets/99a54c7c-b437-440c-8b20-9ab3e099da77" />
5. Create it as a "private" repository
6. After importing the technical-assessment repo to your own account, you can clone it to your local machine
7. Run `make all` and visit `localhost:3000` and `localhost:5050/ping` to confirm it is working
8. Complete the assessment given to you using the cloned repo with proper commit messages and branching
9. Go through the existing packages and file structure to figure out what you might want to use
10. You can use additional packages to your liking and create new files as necessary  
  
## Useful make commands for the repo

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
