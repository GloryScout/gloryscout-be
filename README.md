# GloryScout

A robust backend API for a social platform connecting scouts and players in the sports domain.

## 🚀 Overview

GloryScout is a comprehensive backend solution designed to facilitate the connection between sports talents and scouts. The platform enables user profile management, content posting, interaction through comments and likes, and includes an integrated payment system.

## 📋 Table of Contents

- [Project Structure](#project-structure)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Docker Deployment](#docker-deployment)
- [API Endpoints](#api-endpoints)
- [Authentication and Authorization](#authentication-and-authorization)
- [Database](#database)
- [Configuration](#configuration)

## 🏗️ Project Structure

The solution follows a clean architecture approach with the following projects:

- **GloryScout.API**: Entry point for the application containing controllers and service configurations
- **GloryScout.Domain**: Contains DTOs, validators and business logic
- **GloryScout.Data**: Data access layer with models, repositories, and database context
- **GloryScout.Infrastructure**: Cross-cutting concerns, helpers, and configurations

## ✨ Features

- **User Authentication and Profile Management**: Register, login, and manage user profiles
- **Content Posting**: Create, retrieve, update, and delete posts
- **Social Interactions**: Like, comment, and follow functionality
- **Search Capabilities**: Search for players, scouts, and content
- **Payment Integration**: Secure payment processing via Paymob
- **Verification System**: Email verification for secure account management

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8.0
- **API**: RESTful with Swagger documentation
- **Data Access**: Entity Framework Core
- **Validation**: FluentValidation
- **Authentication**: JWT Bearer tokens
- **Payment Gateway**: Paymob
- **Deployment**: Docker containerization

## 🏁 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (or compatible database)
- Docker (optional, for containerized deployment)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/GloryScout.git
   cd GloryScout
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Update database connection string in `appsettings.json` in the GloryScout.API project.

4. Apply migrations:
   ```bash
   dotnet ef database update --project GloryScout.Data --startup-project GloryScout.API
   ```

5. Run the application:
   ```bash
   dotnet run --project GloryScout.API
   ```

### Docker Deployment

1. Build the Docker image:
   ```bash
   docker build -t gloryscout -f GloryScout.API/Dockerfile .
   ```

2. Run the container:
   ```bash
   docker run -p 8080:8080 -p 8081:8081 gloryscout
   ```

## 🔌 API Endpoints

The API exposes the following controller endpoints:

- **Users**: Authentication, registration, and user management
- **UserProfile**: Profile creation, updating, and retrieval
- **Post**: Content creation and management
- **Search**: Search functionality across the platform
- **Payment**: Payment processing and tracking
- **HomePage**: Featured and relevant content for users

Access the Swagger UI when running in development mode: `https://localhost:8081/swagger`

## 🔐 Authentication and Authorization

The API uses JWT Bearer tokens for authentication and authorization. Users must register and login to obtain a token that must be included in the Authorization header for secured endpoints.

## 💾 Database

The application uses Entity Framework Core with a Code-First approach. The database models are organized in domains like:

- User management
- Player and scout profiles
- Posts, comments, and likes
- Followers and connections
- Payment transactions
- Verification processes

## ⚙️ Configuration

Key configurations in `appsettings.json` include:

- Database connection strings
- JWT authentication settings
- Paymob integration keys
- CORS policies
- Logging settings

---

## 📝 License

[Specify your license here]

## 📞 Contact

[Your contact information]