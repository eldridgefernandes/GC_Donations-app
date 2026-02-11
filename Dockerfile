# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy all files and publish
COPY . ./
RUN dotnet publish -c Release -o out 

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
# COPY --from=publish /app/publish ./

# Expose port (matches your launch settings)
# ENV ASPNETCORE_URLS=http://+:5050
ENV ASPNETCORE_URLS=http://0.0.0.0:5050
EXPOSE 5050
ENTRYPOINT ["dotnet", "backend.dll"]
