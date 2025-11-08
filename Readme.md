# API Gateway with Ocelot - Beginner-Friendly Microservices Orchestration

Welcome to the **API Gateway**! This is a beginner-friendly, lightweight API Gateway built using **Ocelot** in ASP.NET Core, designed to orchestrate microservices for authentication and CRUD operations. It serves as a single entry point for your applications, securely routing requests to downstream services while enforcing JWT-based authentication. Perfect for developers new to microservices or those looking to learn, this project offers an easy-to-understand setup with scalability and security in mind.

## ✨ Why This works
- **Beginner-Friendly**: Simple setup and clear documentation for learning microservices.
- **Microservices Ready**: Seamlessly routes to Auth and CRUD  services.
- **Secure by Design**: Integrates JWT authentication with customizable validation.
- **Scalable Architecture**: Supports load balancing and easy extension for more services.
- **Open Source**: Contribute, fork, and enhance this gateway for your needs!

## 🚀 Features
- Routes client requests to microservices (e.g., /auth/* to Auth, /api/* to CRUD).
- Validates JWT tokens for protected endpoints (e.g., /api/books).
- Configurable via ocelot.json for dynamic routing and HTTPS support.
- Built with ASP.NET Core for high performance and cross-platform compatibility.
- Includes logging for debugging and monitoring.

## 🛠️ Prerequisites
- **.NET 9 SDK** (or your version)
- **Visual Studio 2022** (or code editor with .NET support)
- **Auth Service** and **CRUD Service**  running
- **Postman** or similar for API testing

## 📋 Installation
1. **Clone the Repository**
   ```bash
   git clone https://github.com/sivavithu/Api-GateWay-MicroServices.git
   cd ApiGateway
   ```

2. **Configure Secrets**
   
   The application requires JWT configuration. **Never commit secrets to git!**
   
   For local development, use .NET User Secrets:
   ```bash
   cd ApiGateway
   dotnet user-secrets set "AppSettings:Key" "your-secure-jwt-secret-key-minimum-32-characters"
   dotnet user-secrets set "AppSettings:issuer" "loginapp"
   dotnet user-secrets set "AppSettings:Audience" "myAwesomeAudience"
   ```
   
   📖 **See [SECRETS.md](SECRETS.md) for complete configuration guide including production deployment.**

3. **Build and Run**
   ```bash
   dotnet build
   dotnet run
   ```
