# BloodSword Online 🏰

BloodSword Online is a web-based RPG system inspired by the classic "Blood Sword" game books.
This project serves as a practical example of building a complex, stateful application using modern .NET technologies and architectural patterns.

## 🎯 Goals

* Implement a rich domain model based on standard RPG rules (classes, stats, combat, inventory).
* Demonstrate **Clean Architecture** (Onion Architecture) in a real-world scenario.
* Showcase best practices: Unit Testing, Git Flow, CI/CD, Containerization (Docker).

## 🏗️ Architecture

The solution follows Clean Architecture principles to ensure separation of concerns and testability:

* **BloodSword.Domain**: Contains the core business entities and rules (Heroes, Items, Monsters). No external dependencies.
* **BloodSword.Application**: Orchestrates use cases (e.g., `CreateHero`, `AttackMonster`) and defines interfaces for infrastructure.
* **BloodSword.Infrastructure**: Implements external concerns (EF Core Database context, external APIs, Identity providers).
* **BloodSword.WebAPI**: The entry point of the application. A thin layer that exposes RESTful endpoints.

## 🛠️ Tech Stack

* .NET 8 / 10
* Entity Framework Core
* ASP.NET Core Web API
* SQLite / SQL Server (TBD)
* xUnit (for testing)
