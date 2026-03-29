# 🚀 SynthPay - High-Availability Payment Core

A highly available, event-driven microservices architecture for processing payments, built with **.NET 8** and **Clean Architecture** principles.

## 🧠 Architecture Overview

SynthPay demonstrates Senior-level software engineering patterns to ensure data consistency, decoupling, and resilience in a distributed environment.

* **Clean Architecture & CQRS:** Separation of concerns using `MediatR` to isolate business rules from infrastructure and presentation.
* **Event-Driven Architecture:** Microservices communicate asynchronously via **RabbitMQ**, decoupled by `MassTransit`.
* **Transactional Outbox Pattern:** Guarantees that no domain events are lost if the message broker goes down. Events are saved in the same SQL transaction as the business entity and published by a background worker.
* **Database-per-Service:** Each microservice (`Transactions.Api` and `Ledger.Worker`) owns its isolated SQLite database.
* **Idempotent Consumer:** The Ledger service is protected against duplicate messages from the broker, ensuring a user's balance is never credited twice for the same transaction.

## 🛠️ Technologies & Tools

* **C# / .NET 8** (Web API & Background Workers)
* **Entity Framework Core** (Code-First, Migrations)
* **SQLite** (Relational isolated databases)
* **RabbitMQ & MassTransit** (Message Broker & Bus)
* **FluentValidation** (Fail-fast request validation)
* **Docker & Docker Compose** (Containerization & Orchestration)

## 🐳 How to Run (Docker)

You don't need Visual Studio or .NET installed on your host machine to run this project. Everything is containerized.

1. Clone this repository.
2. Open a terminal in the root folder (where the `docker-compose.yml` is located).
3. Run the following command:

```bash
docker-compose up --build -d
