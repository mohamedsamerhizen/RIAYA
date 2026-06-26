# RIAYA — Medical Complex Management Backend

Portfolio-grade ASP.NET Core Web API for managing medical complex operations including doctors, patients, appointments, visits, prescriptions, departments, clinic rooms, billing, payments, and demo data.

RIAYA is built as a focused backend portfolio project with production-oriented patterns for authentication, role-based authorization, EF Core persistence, Swagger documentation, Dockerized local execution, and automated regression tests.

## Tech Stack

- ASP.NET Core Web API (.NET 9)
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT + refresh tokens
- Swagger / OpenAPI
- Docker and Docker Compose
- xUnit
- Serilog
- GitHub Actions CI

## Screenshots

The screenshots below were captured from the current local RIAYA API running at `http://localhost:5173/swagger`. They do not contain bearer tokens, refresh tokens, production secrets, or real patient data.

### Swagger Overview

![Swagger Overview](docs/screenshots/01-swagger-overview.png)

### Appointments Workflow

![Appointments Workflow](docs/screenshots/02-appointments-workflow.png)

### Medical Complex Structure

![Medical Complex Structure](docs/screenshots/03-medical-complex-structure.png)

### Billing & Payments

![Billing and Payments](docs/screenshots/04-billing-payments.png)

Test-result and Docker screenshots are intentionally not linked here because no real terminal or Docker-running screenshot was captured in this environment.

## Core Modules

- Authentication & roles
- Doctors
- Patients
- Appointments
- Visits
- Prescriptions
- Departments
- Clinic rooms
- Doctor clinic assignments
- Billing
- Invoices
- Payments
- Dashboard
- Demo seed data
- Swagger
- Docker
- Tests

## Roles

- `Admin`: manages the system and has full administrative and clinical access.
- `Doctor`: sees medical data only inside the linked doctor profile scope.
- `Receptionist`: manages administrative and operational data such as patients, appointments, check-in, invoices, and payments. Receptionists do not see clinical details such as diagnosis, notes, symptoms, prescription medication, dosage, or instructions.

Doctor profiles can only be linked to application users that already have the `Doctor` role.

## Business Rules

- Appointment duration is stored per appointment and defaults to 30 minutes.
- Doctor, patient, and clinic room appointment overlaps are rejected using appointment start time plus `DurationMinutes`.
- Cancelled appointments are ignored by overlap checks; other statuses still reserve the slot.
- Clinic room bookings require an active doctor-room assignment covering the full appointment duration.
- Check-in is allowed only for confirmed appointments and is blocked for pending, cancelled, completed, and no-show appointments.
- Future appointments cannot be marked as no-show.
- Creating a visit completes the linked appointment.
- Visits cannot be created for pending, cancelled, no-show, or future appointments.
- A doctor cannot create or modify visits or prescriptions for another doctor's appointment.
- Paid and partially paid invoices cannot have invoice items changed.
- Invoices with payments cannot be cancelled without a separate refund or reversal workflow.
- Cancelled invoices cannot receive payments.
- Overpayments are rejected.
- Invoice totals, paid amounts, remaining amounts, and status transitions are recalculated from invoice items and payments.
- Receptionist clinical search side channels are blocked for symptoms, diagnosis, notes, medication names, dosage, and prescription instructions.

## API Overview

Both unversioned and versioned route prefixes are available:

- Auth: `/api/auth`, `/api/v1/auth`
- Dashboard: `/api/dashboard/overview`, `/api/v1/dashboard/overview`
- Doctors: `/api/doctors`, `/api/v1/doctors`
- Specializations: `/api/specializations`, `/api/v1/specializations`
- Doctor schedules: `/api/doctorschedules`, `/api/v1/doctorschedules`
- Patients: `/api/patients`, `/api/v1/patients`
- Appointments: `/api/appointments`, `/api/v1/appointments`
- Visits: `/api/visits`, `/api/v1/visits`
- Prescriptions: `/api/prescriptions`, `/api/v1/prescriptions`
- Departments: `/api/departments`, `/api/v1/departments`
- Clinic rooms: `/api/clinicrooms`, `/api/v1/clinicrooms`
- Doctor clinic assignments: `/api/doctorclinicassignments`, `/api/v1/doctorclinicassignments`
- Medical services: `/api/medicalservices`, `/api/v1/medicalservices`
- Invoices: `/api/invoices`, `/api/v1/invoices`
- Payments: `/api/payments`, `/api/v1/payments`

Postman collection:

- [RIAYA.postman_collection.json](postman/RIAYA.postman_collection.json)

## How To Run

```powershell
dotnet restore ".\Riaya.slnx"
dotnet build ".\Riaya.slnx" --no-restore
dotnet test ".\Riaya.slnx"
dotnet ef database update --project ".\Riaya.Api\Riaya.Api.csproj" --startup-project ".\Riaya.Api\Riaya.Api.csproj"
dotnet run --project ".\Riaya.Api\Riaya.Api.csproj" --launch-profile http
```

Local URLs:

- API: `http://localhost:5173`
- Swagger: `http://localhost:5173/swagger`
- Health: `http://localhost:5173/health`

Docker:

```powershell
copy .env.example .env
docker compose up --build
```

Docker uses database `RiayaDb`, API container `riaya-api`, and SQL Server container `riaya-sqlserver`.

## Demo Accounts

`.env.example` contains local demo values only. Do not use those values for production or shared deployments.

Default local demo account values:

- `ADMIN_EMAIL=admin@riaya.local`
- `ADMIN_PASSWORD=Admin@12345`
- `ADMIN_FULL_NAME=RIAYA Admin`

When `DemoSeed__Enabled=true`, the demo seed also creates local-only demo users:

- `admin@riaya.local`
- `reception1@riaya.local`
- `reception2@riaya.local`
- `doctor1@riaya.local` through `doctor8@riaya.local`

The shared local demo password is `Admin@12345`. These accounts are for local portfolio demonstration only.

## Demo Seed Data

Demo seed data is enabled with:

- `DemoSeed__Enabled=true`

The dataset is fictional, idempotent, and intended for local demo use only. It is not production data and does not contain real patients, secrets, or payment integrations.

The current seed creates:

- 3 roles: Admin, Doctor, Receptionist
- 11 demo users: 1 admin, 2 receptionists, 8 doctors
- 8 specializations
- 8 departments
- 8 clinic rooms
- 8 doctor clinic assignments
- 35 patients
- 70 appointments distributed across past dates, today, and upcoming dates
- 24 visits
- 20 prescriptions
- 12 medical services
- 36 invoices
- 24 payments

The seed respects appointment overlap rules, doctor-room assignments, completed-appointment visit links, invoice totals, partial/full payment status, no overpayments, and no payments on cancelled invoices.

## Database

Migrations are stored under `Riaya.Api/Migrations`.

Common EF commands:

```powershell
dotnet ef database update --project ".\Riaya.Api\Riaya.Api.csproj" --startup-project ".\Riaya.Api\Riaya.Api.csproj"
dotnet ef migrations has-pending-model-changes --project ".\Riaya.Api\Riaya.Api.csproj" --startup-project ".\Riaya.Api\Riaya.Api.csproj"
```

Local demo data is controlled by:

- `AdminSeed__Enabled=true`
- `DemoSeed__Enabled=true`

Demo seed data is for local development and portfolio demonstration only.

## Tests

```powershell
dotnet test ".\Riaya.slnx"
```

Current suite: 81 passing tests covering auth, authorization metadata, appointment workflow, duration-based overlap checks, check-in/no-show behavior, clinical privacy, medical complex structure, doctor-room assignment consistency, billing/payment integrity, demo seed counts, demo seed idempotency, demo seed appointment integrity, demo seed billing integrity, and API smoke coverage.

## Security Notes

- Refresh tokens are generated server-side, hashed before storage, and rotated.
- JWT settings must come from environment-specific configuration outside source control.
- Receptionist-facing patient, visit, and prescription responses intentionally omit clinical details.
- Clinical search is role-aware to avoid leaking diagnosis, notes, medication names, dosage, or instructions through result counts.
- Soft-deleted records are excluded from normal business validations.
- `.env.example` is local/demo-only documentation and not a production secrets file.

## Roadmap

- AuthController refactor.
- TimeProvider / UTC policy.
- Reports Pro.
- Daily Cash Closing.
- Audit Log Pro.
- Lab/Radiology requests.
- Notification Center.
