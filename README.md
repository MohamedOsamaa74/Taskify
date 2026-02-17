#  Taskify

[![CI](https://github.com/MohamedOsamaa74/Taskify/workflows/CI/badge.svg)](https://github.com/MohamedOsamaa74/Taskify/actions)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/MohamedOsamaa74/Taskify?style=social)](https://github.com/MohamedOsamaa74/Taskify/stargazers)

> A modern task management system built with .NET 10 and Clean Architecture principles.

##  About

Taskify is a robust task management API that helps teams organize their work efficiently. Built with industry best practices including:

-  Clean Architecture
-  Repository Pattern with Unit of Work
-  JWT Authentication & Authorization
-  Result Pattern for error handling
-  Entity Framework Core
-  ASP.NET Core Identity

##  Features

-  **Team Management** - Create and manage teams
-  **Task Organization** - Organize tasks with todo lists
-  **Secure Authentication** - JWT-based authentication with role-based authorization
-  **User Management** - Complete user registration and management system
-  **Role-Based Access** - Admin, Moderator, and User roles

##  Tech Stack

- **Framework:** .NET 10
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** ASP.NET Core Identity + JWT
- **Architecture:** Clean Architecture (Domain, Application, Infrastructure, API)

##  Project Structure
  ```plaintext
  Taskify/
  │
  ├── Taskify.Api/             # Presentation Layer
  │   ├── Controllers/         # API Controllers
  │   ├── Filters/             # Action Filters
  │   └── Program.cs           # Application Entry Poin
  │
  ├── Taskify.Application/     # Application Layer
  │   ├── DTOs/                # Data Transfer Objects
  │   ├── Services/            # Business Logic Services
  │   ├── ResultPattern/       # Result Pattern Implementation
  │   └── Helpers/             # Utility Classes
  │
  ├── Taskify.Domain/          # Domain Layer
  │   ├── Entities/            # Domain Entities
  │   ├── Repositories/        # Repository Interfaces
  │   └── Const/               # Domain Constants
  │
  ├── Taskify.InfraStructure/  # Infrastructure Layer
  │   ├── Contexts/            # Database Context
  │   ├── Repositories/        # Repository Implementations
  │   └── Migrations/          # EF Core Migrations
  ```

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Developer Edition)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**

   `git clone https://github.com/MohamedOsamaa74/Taskify`

2. **Update the database connection string**

   Open `Taskify.Api/appsettings.json` and update the connection string:

   `{ "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=TaskifyDb;Trusted_Connection=True;" } }`


3. **Apply database migrations**

   `dotnet ef database update --project Taskify.Infrastructure --startup-project Taskify.Api`


4. **Configure JWT settings** (Optional)

   Update JWT settings in `appsettings.json`:
   
   `{ "JWT": { "Key": "YourSuperSecretKeyHere_MinimumLength32Characters", "Issuer": "TaskifyAPI", "Audience": "TaskifyUsers", "DurationInMinutes": 60 } }`


5. **Run the application**
6. 
   `dotnet run --project Taskify.Api`


7. **Access the API**

   The API will be available at:
   - HTTPS:     `https://localhost:7011`
   - HTTP:      `http://localhost:5011`
   - Scalar UI: `https://localhost:7011/scalar/v1`
     
## Usage
Once the application is running, you can interact with the API using tools like Postman, scalar or through a frontend client.

##  CI/CD

This project uses **GitHub Actions** for continuous integration. Every push to `master` or `develop` branches automatically:

-  Restores NuGet packages
-  Builds the solution in Release mode
-  Runs all unit tests (when available)
-  Publishes deployment artifacts
-  Uploads build artifacts for download

### View Build Status

Check the current build status: [GitHub Actions](https://github.com/MohamedOsamaa74/Taskify/actions)

### Download Build Artifacts

After each successful build, you can download the deployment-ready artifacts from the Actions tab.

### Workflow Configuration

The CI workflow is defined in `.github/workflows/ci.yml` and runs on:
- Push to `master` or `develop` branches
- Pull requests targeting `master`
  
### Key Patterns Used

- **Repository Pattern** - Data access abstraction
- **Unit of Work** - Transaction management
- **Result Pattern** - Standardized error handling
- **Dependency Injection** - Loose coupling
- **JWT Authentication** - Stateless authentication
- **DTO Pattern** - Data transfer objects

##  Security

-  JWT Bearer token authentication
-  Role-based authorization (Admin, Moderator, User)
-  ASP.NET Core Identity for user management
-  Password hashing and validation
-  Refresh token mechanism


##  License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

##  Author

**Mohamed Osama**

- GitHub: [@MohamedOsamaa74](https://github.com/MohamedOsamaa74)
- LinkedIn: [MohamedOsama](https://linkedin.com/in/mohamed-osama-306573200/)

##  Contributing

Contributions, issues, and feature requests are welcome!

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

##  Support

If you have any questions or need help, please open an issue in the [Issues](https://github.com/MohamedOsamaa74/Taskify/issues) section.

---

<div align="center">
  
**⭐️ If you like this project, please give it a star! ⭐️**

Made with ❤️ by Mohamed Osama

</div>
