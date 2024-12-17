## Project Setup Guide

This repository contains two parts:

- Backend: A .NET API that handles document management, including file upload, shareable links, and user management.
- Frontend: A React.js application for interacting with the backend and displaying document details.

## Table of Contents
- Prerequisites
- Backend Setup
- Frontend Setup
- Configuration
- Running the Application
- Folder Structure

## Prerequisites
Before setting up the project, make sure you have the following installed on your machine:

- Node.js (v14.x or above) for running the React client.
- .NET SDK (v9.0) for running the backend.
- MySQL for database storage.

## Backend Setup (.NET)
Follow these steps to set up and run the backend (.NET API).
1. Clone the Repository:
```bash
git clone https://github.com/SlavaPeshko/FileManager.git
cd FileManager
```
2. Install Dependencies: Ensure that your .NET SDK is installed. Then restore NuGet packages for the backend project:
```bash
dotnet restore
```
3. Configure the Database:
  - In the backend project, find the appsettings.json file.
  - Update the database connection string to match your MySQL setup:
```bash
{
  "FileManager": {
    "Path": "documents"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FileManagerDB;User Id=root;Password=1234;"
  }
}
```
4. Create and Migrate Database: Run the following commands to create the MySQL database and apply migrations:
```bash
dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project WebUI/WebUI.csproj --context Infrastructure.Context.FileManagerDbContext --configuration Debug  
```
5. Run the Backend: Once everything is set up, you can run the backend using the following command:
```bash
dotnet run --project .\WebUI\
```
The API will start on https://localhost:7257

## Admin User Details

When the database is updated, the script automatically adds an admin user to the database with the following credentials:

- **Username**: `admin`
- **Password**: `admin`

## Frontend Setup (React.js)
Follow these steps to set up and run the frontend (React.js application).
1. Open the Repository:
```bash
cd .\WebUI\client-app\
```
2. Install Node.js Dependencies: Ensure that Node.js is installed. Then, install the frontend dependencies:
```bash
npm install
```
3. Run the Frontend: After installing dependencies, you can start the React frontend:
```bash
npm run dev
```
