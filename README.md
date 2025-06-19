# Personal Expense Tracker

> A secure ASP .NET Core MVC web app with user authentication and a Tailwind-styled dashboard for tracking personal expenses.

## 🚀 Features

- **User Registration & Login**  
  - Secure password hashing with ASP .NET Identity  
  - Session-based authentication

- **Expense Management**  
  - Create, view and list expenses (date, category, amount)  
  - Scoped to the logged-in user

- **Modern UI**  
  - Responsive design with Tailwind CSS  
  - Modal form for adding new expenses  
  - Clean, mobile-first dashboard

## 🛠 Tech Stack

- **Framework:** ASP .NET Core 8.0 MVC  
- **ORM:** Entity Framework Core  
- **Database:** SQL Server (LocalDB or full SQL Server)  
- **Styling:** Tailwind CSS  
- **Auth:** Session state + `IPasswordHasher<T>`  
- **Tools:** `dotnet CLI`, EF Core Migrations

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)  
- SQL Server (LocalDB or full)  
- `dotnet-ef` global tool (for migrations):
  ```bash
  dotnet tool install --global dotnet-ef
