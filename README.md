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

### **Main Architecture and Design Decisions**

#### **1. Architecture**
- **Type:** Client-Server Architecture
- **Backend:** A .NET 9 Web API responsible for file management, including uploading, downloading, and generating shareable links.
- **Frontend:** A React.js single-page application (SPA) that interacts with the backend via API endpoints to display and manage files.

#### **2. Backend Design**
- **Framework:** .NET 9 for building a robust and high-performance Web API.
- **Mediator Pattern:** Utilized `MediatR` to decouple business logic and improve maintainability by handling commands and queries in a clean, modular way.
- **Database:** MySQL with a simple schema:
  - `Users` table for managing user accounts.
  - `Documents` table for storing file metadata.
  - `SharedLinks` table for tracking shareable file links.
- **Security:** Shareable links provide controlled access to files, ensuring minimal exposure of sensitive data.

#### **3. Frontend Design**
- **Framework:** React.js for a dynamic and interactive user interface.
- **Components:** Modular and reusable components including:
  - `DocumentTable`: Displays the list of uploaded files.
  - `LoadingSpinner`: Provides visual feedback during loading processes.
  - `Login`: Provides simple user authentication. (Store userId in local storage)
  - `UploadFiles`: Allows users to upload files to the backend.

#### **4. Communication**
- **Client-Server Interaction:** RESTful API endpoints enable seamless communication between the React.js client and the .NET backend.
- **Data Exchange:** JSON format is used for consistent and lightweight data transfer.

#### **5. Design Principles**
- **Separation of Concerns:** Clear division between the backend (business logic and data management) and frontend (user interface).
- **Modularity:** Backend commands and queries are handled via `MediatR`, while frontend components are reusable and isolated.
- **Scalability:** The architecture supports future extensions, such as additional file types, permissions, or advanced link sharing features.

### **Technical Improvements**
1. **Scalability and Performance**
   - **CDN Integration:** Store and serve files through a Content Delivery Network (e.g., AWS S3, Azure Blob Storage) for faster downloads and reduced server load.
   - **Caching:** Use caching (e.g., Redis) for frequently accessed files or shared links to improve response times. 

2. **Logging and Monitoring**
   - Introduce centralized logging with tools like **Serilog** to monitor API usage and debug issues.  
   - Add health checks and monitoring for the backend to track performance and detect anomalies.

3. **Security Improvements**
   - Use **JWT with refresh tokens** for secure and scalable user authentication.  
   - Implement **rate limiting** to prevent abuse of the API endpoints.

4. **Automated Testing**
   - Add unit tests for backend services (e.g., MediatR handlers) to ensure the stability of business logic. 
