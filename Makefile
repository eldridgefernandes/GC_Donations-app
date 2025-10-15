DB_FILE = db/mydatabase.db
TABLE_NAME = donations
SQL_CREATE_TABLE = CREATE TABLE $(TABLE_NAME) ( \
    id INTEGER PRIMARY KEY, \
    donor_name TEXT NOT NULL, \
    amount REAL NOT NULL, \
	date TEXT NOT NULL \
);

.PHONY : all frontend backend run-test

frontend:
	@echo "-----Running Frontend React App-----"
	(cd frontend; npm i; npm start &)

backend:
	@echo "-----Running Backend .NET App-----"
	(cd backend; dotnet run &)

all: frontend backend
	@echo "-----Completed running both Frontend and Backend-----"

run-test: backend
	@echo "-----Completed running Backend and starting its tests-----"
	dotnet test be-test.sln
	
init-db:
	@echo "-----Installing SQLite for .NET-----"
	(cd backend; dotnet add package Microsoft.Data.Sqlite)
	(cd test/be-test; dotnet add package Microsoft.Data.Sqlite)
	@echo "-----Creating database and table-----"
	mkdir db
	sqlite3 $(DB_FILE) "$(SQL_CREATE_TABLE)"
	@echo "-----Table '$(TABLE_NAME)' created in '$(DB_FILE)'-----"

list-pid-unix:
	lsof -i:5050
	lsof -i:3000
	@echo "kill tasks with: kill -9 [PID]"

list-pid-windows:
	netstat -ano | findstr :5050
	netstat -ano | findstr :3000
	@echo "kill tasks with: taskkill /PID [PID] /F"

terminate:
ifeq ($(OS),Windows_NT)
	@echo "Killing processes on ports 5050 & 3000 for Windows..."
	@FOR /F "tokens=5" %%P IN ('netstat -aon ^| findstr :5050') DO taskkill /F /PID %%P > nul 2>&1 || (echo No process found on port 5050)
	@FOR /F "tokens=5" %%P IN ('netstat -aon ^| findstr :3000') DO taskkill /F /PID %%P > nul 2>&1 || (echo No process found on port 3000)
else
	@echo "Killing processes on ports 5050 & 3000 for Mac/Linux..."
	@lsof -t -i:5050 | xargs kill -9 > /dev/null 2>&1 || (echo No process found on port 5050)
	@lsof -t -i:3000 | xargs kill -9 > /dev/null 2>&1 || (echo No process found on port 3000)
endif
	@echo "Done."
