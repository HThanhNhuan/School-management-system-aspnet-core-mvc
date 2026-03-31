# QLSV - Student Management System

A web-based **Student Management System** built with **ASP.NET Core MVC** for managing users, students, teachers, employees, classes, courses, subjects, grades, attendance, payments, and internal alerts.

This project follows a layered structure with **MVC + Repository Pattern + Entity Framework Core + ASP.NET Core Identity**, and includes role-based dashboards, email-based account activation, profile image upload support, and seeded demo data.

---

## Table of Contents

- [Overview](#overview)
- [Core Features](#core-features)
- [Business Modules](#business-modules)
- [Architecture](#architecture)
- [Authentication and User Lifecycle](#authentication-and-user-lifecycle)
- [Roles](#roles)
- [Role and Permission Matrix](#role-and-permission-matrix)
- [Controller Authorization Summary](#controller-authorization-summary)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Database Overview](#database-overview)
- [API Endpoint](#api-endpoint)
- [Seed Data](#seed-data)
- [Prerequisites](#prerequisites)
- [Configuration](#configuration)
- [How to Run](#how-to-run)
- [Suggested Demo Flow](#suggested-demo-flow)
- [Known Notes and Limitations](#known-notes-and-limitations)
- [Future Improvements](#future-improvements)

---

## Overview

This repository contains a school/student management application designed for academic administration scenarios such as:

- managing student records
- managing teachers and employees
- organizing courses, classes, and subjects
- recording grades and attendance
- handling payment records
- creating and resolving internal alerts
- authenticating users with role-based access control

The solution uses **ASP.NET Core MVC** for the web layer, **Entity Framework Core** for data access, **SQL Server** for persistence, and **ASP.NET Core Identity** for authentication and roles.

> **Important naming note:** the repository/folder is presented as **QLSV**, while the project namespace and `.csproj` name are **SchoolManagementSystem**.

---

## Core Features

### 1. Account and Identity Management

- User registration
- Login and logout
- Email confirmation
- First-password change flow after activation
- Forgot password / reset password
- Update personal profile information
- Role-based redirection after login

### 2. User Types and Role-Based Workflows

- Admin
- Employee
- Teacher
- Student
- Pending
- Anonymous/Public visitor

### 3. Student Management

- Create, edit, view, and delete students
- Assign a student to a school class
- Link a student profile to an Identity user account

### 4. Teacher Management

- Create, edit, view, and delete teachers
- Assign teachers to subjects
- Assign teachers to school classes

### 5. Employee Management

- Create, edit, view, and delete employees
- Assign department, academic degree, hire date, and status

### 6. Academic Structure Management

- Course management
- Subject management
- School class management
- Course-to-subject mapping
- Class-to-course mapping
- Teacher-to-subject mapping
- Teacher-to-class mapping

### 7. Grades Management

- Record grades per student and subject
- Edit and delete grades
- View detailed grade history
- Student self-service page for `MyGrades`
- Automatic pass/fail interpretation based on score

### 8. Attendance Management

- Record attendance per student and subject
- Edit and delete attendance records
- Calculate attendance details by subject/class context
- Student self-service page for `MyAttendances`

### 9. Payment Management

- Create, edit, view, and delete payment records
- View pending payments

### 10. Alert and Notification Management

- Employees can create internal alerts
- Employees can view their own alerts
- Admin can review unresolved alerts
- Alerts are also used for internal notification workflows

### 11. API Support

- API endpoint to retrieve students by school class

---

## Business Modules

| Module         | Main Purpose                                                            |
| -------------- | ----------------------------------------------------------------------- |
| Account        | Registration, login, email confirmation, password flows, profile update |
| Dashboard      | Separate landing pages for Admin, Employee, Teacher, and Student        |
| Students       | Manage student records and class assignments                            |
| Teachers       | Manage teacher records, subjects, and class assignments                 |
| Employees      | Manage staff/administrative employee records                            |
| Courses        | Manage academic courses                                                 |
| Subjects       | Manage subjects and credit/class metadata                               |
| School Classes | Manage school classes and their course association                      |
| Grades         | Record and review student grades                                        |
| Attendance     | Record and review student attendance                                    |
| Payments       | Track tuition/payment records                                           |
| Alerts         | Handle internal operational alerts                                      |
| API            | Expose students by class for dynamic UI or integrations                 |

---

## Architecture

The project is organized using a classic layered web application style:

- **Controllers**: request handling and navigation
- **Views**: Razor UI pages
- **Models / ViewModels**: data exchange between controllers and views
- **Data / Entities**: domain entities and `DbContext`
- **Repositories**: data access abstraction and business-oriented queries
- **Helpers**: user, email, conversion, and blob-storage utilities
- **Identity**: authentication, roles, password, email confirmation

### Architectural Patterns Used

- **ASP.NET Core MVC**
- **Repository Pattern**
- **Dependency Injection**
- **Role-Based Authorization**
- **Entity Framework Core Code-First style setup**

---

## Authentication and User Lifecycle

A user lifecycle is implemented around Identity roles and profile creation.

### Registration Flow

1. A user registers through the account module.
2. The system creates an Identity user.
3. The user is assigned to the **Pending** role.
4. A confirmation email is sent.
5. After confirmation, the user can activate the account and change the password.
6. An Admin or Employee can later create the matching domain profile:
   - Student profile
   - Teacher profile
   - Employee profile
7. The role is updated from **Pending** to the correct operational role.

### Login Flow

After successful login, the system redirects users by role:

- `Admin` -> `Dashboard/AdminDashboard`
- `Employee` -> `Dashboard/EmployeeDashboard`
- `Teacher` -> `Dashboard/TeacherDashboard`
- `Student` -> `Dashboard/StudentDashboard`

### Profile Synchronization

When a user updates personal information, helper logic synchronizes the corresponding domain record for:

- Employee
- Student
- Teacher

---

## Roles

| Role          | Description                                                                                                                                              |
| ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Admin**     | Full administrative access. Manages employees and has access to most academic modules.                                                                   |
| **Employee**  | Operational staff role. Manages students, teachers, classes, courses, subjects, grades, attendance, and alerts.                                          |
| **Teacher**   | Currently has a dedicated dashboard, but no major CRUD academic management actions are explicitly granted in the current controller authorization setup. |
| **Student**   | Has a student dashboard and self-service access to personal grades and attendance.                                                                       |
| **Pending**   | Newly registered user waiting for account activation and/or profile assignment.                                                                          |
| **Anonymous** | Public visitor who can access public pages and authentication pages.                                                                                     |

---

## Role and Permission Matrix

The table below describes the **current functional access model** based on controller authorization, dashboard/sidebar visibility, and account flow.

| Permission / Feature           |                                                             Admin |                                                          Employee |                                Teacher |                                Student |                       Pending / Public |
| ------------------------------ | ----------------------------------------------------------------: | ----------------------------------------------------------------: | -------------------------------------: | -------------------------------------: | -------------------------------------: |
| View public home pages         |                                                                ✅ |                                                                ✅ |                                     ✅ |                                     ✅ |                                     ✅ |
| Register a new account         |                                                                ✅ |                                                                ✅ |                                     ✅ |                                     ✅ |                                     ✅ |
| Login / logout                 |                                                                ✅ |                                                                ✅ |                                     ✅ |                                     ✅ |                                     ✅ |
| Confirm email / reset password |                                                                ✅ |                                                                ✅ |                                     ✅ |                                     ✅ |                                     ✅ |
| Update own profile             |                                                                ✅ |                                                                ✅ |                                     ✅ |                                     ✅ |                ⚠️ Activation flow only |
| Access Admin dashboard         |                                                                ✅ |                                                                 — |                                      — |                                      — |                                      — |
| Access Employee dashboard      |                                                                 — |                                                                ✅ |                                      — |                                      — |                                      — |
| Access Teacher dashboard       |                                                                 — |                                                                 — |                                     ✅ |                                      — |                                      — |
| Access Student dashboard       |                                                                 — |                                                                 — |                                      — |                                     ✅ |                                      — |
| Manage employees (CRUD)        |                                                                ✅ |                                                                 — |                                      — |                                      — |                                      — |
| Manage students (CRUD)         |                                                                ✅ |                                                                ✅ |                                      — |                                      — |                                      — |
| Manage teachers (CRUD)         |                                                                ✅ |                                                                ✅ |                                      — |                                      — |                                      — |
| Manage courses (CRUD)          |                                                                ✅ |                                                                ✅ |                                      — |                                      — |                                      — |
| Manage subjects (CRUD)         |                                                                ✅ |                                                                ✅ |                                      — |                                      — |                                      — |
| Manage school classes (CRUD)   |                                                                ✅ |                                                                ✅ |                                      — |                                      — |                                      — |
| Manage grades                  |                                                                ✅ |                                                                ✅ |                                      — |                                      — |                                      — |
| View own grades                |                                                                 — |                                                                 — |                                      — |                                     ✅ |                                      — |
| Manage attendance              |                                                                ✅ |                                                                ✅ |                                      — |                                      — |                                      — |
| View own attendance            |                                                                 — |                                                                 — |                                      — |                                     ✅ |                                      — |
| Create alert                   |                ⚠️ Intended admin visibility is review, not create |                                                                ✅ |                                      — |                                      — |                                      — |
| View own created alerts        |                                                                 — |                                                                ✅ |                                      — |                                      — |                                      — |
| Review unresolved alerts       |                                                                ✅ |                                                                 — |                                      — |                                      — |                                      — |
| Manage payments                | ⚠️ Routes exist, but controller authorization should be tightened | ⚠️ Routes exist, but controller authorization should be tightened |                                      — |                                      — |                                      — |
| Access student API by class    |                            ⚠️ Currently no explicit authorization |                            ⚠️ Currently no explicit authorization | ⚠️ Currently no explicit authorization | ⚠️ Currently no explicit authorization | ⚠️ Currently no explicit authorization |

### Legend

- `✅` = allowed by current implementation
- `—` = not available in the current implementation
- `⚠️` = available or reachable, but should be reviewed or explicitly secured

---

## Controller Authorization Summary

This table reflects the **current code-level authorization state**.

| Controller / Area         | Current Authorization                                                                                       |
| ------------------------- | ----------------------------------------------------------------------------------------------------------- |
| `DashboardController`     | `[Authorize]` at controller level, then role-specific dashboard actions                                     |
| `EmployeesController`     | `[Authorize(Roles = "Admin")]`                                                                              |
| `StudentsController`      | `[Authorize(Roles = "Employee,Admin")]`                                                                     |
| `TeachersController`      | `[Authorize(Roles = "Employee,Admin")]`                                                                     |
| `CoursesController`       | `[Authorize(Roles = "Employee,Admin")]`                                                                     |
| `SubjectsController`      | `[Authorize(Roles = "Employee,Admin")]`                                                                     |
| `SchoolClassesController` | `[Authorize(Roles = "Employee,Admin")]`                                                                     |
| `GradesController`        | Grade management actions: `Employee,Admin`; `MyGrades`: `Student`                                           |
| `AttendanceController`    | Management actions: `Employee,Admin`; `MyAttendances`: `Student`                                            |
| `AccountController`       | Mostly public/authentication flow actions; some actions rely on current signed-in user context              |
| `AlertController`         | **No explicit `[Authorize]` attributes**; access currently depends more on UI flow and current user context |
| `PaymentsController`      | **No explicit `[Authorize]` attributes**                                                                    |
| `StudentsApiController`   | **No explicit `[Authorize]` attributes**                                                                    |
| `HomeController`          | Public                                                                                                      |
| `ErrorController`         | Public                                                                                                      |

### Security Review Recommendation

The following controllers should be reviewed and hardened with explicit authorization attributes:

- `AlertController`
- `PaymentsController`
- `StudentsApiController`
- selected signed-in-only account actions such as profile update flows

---

## Technology Stack

### Backend

- **ASP.NET Core MVC (.NET 8)**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **JWT Bearer Authentication**

### Database

- **SQL Server**

### Utilities and Integrations

- **MailKit** for email sending
- **Serilog file logging**
- **Syncfusion** UI library
- **Bootstrap** and Razor views for UI

### Main NuGet Packages

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `MailKit`
- `Serilog.Extensions.Logging.File`
- `Syncfusion.EJ2.AspNet.Core`

---

## Project Structure

```text
QLSV/
├── Controllers/
│   ├── API/
│   │   └── StudentsApiController.cs
│   ├── AccountController.cs
│   ├── AlertController.cs
│   ├── AttendanceController.cs
│   ├── CoursesController.cs
│   ├── DashboardController.cs
│   ├── EmployeesController.cs
│   ├── ErrorController.cs
│   ├── GradesController.cs
│   ├── HomeController.cs
│   ├── PaymentsController.cs
│   ├── SchoolClassesController.cs
│   ├── StudentsController.cs
│   ├── SubjectsController.cs
│   ├── TeachersController.cs
│   └── UserManagementController.cs
├── Data/
│   ├── Entities/
│   ├── SchoolDbContext.cs
│   └── SeedDb.cs
├── Helpers/
├── Logs/
├── Migrations/
├── Models/
├── Repositories/
├── ViewComponents/
├── Views/
├── wwwroot/
├── Program.cs
├── appsettings.Development.json
└── SchoolManagementSystem.csproj
```

---

## Database Overview

The domain model includes the following main entities:

| Entity               | Purpose                                                              |
| -------------------- | -------------------------------------------------------------------- |
| `User`               | Identity user with first name, last name, address, phone, timestamps |
| `Student`            | Student profile linked to an Identity user                           |
| `Teacher`            | Teacher profile linked to an Identity user                           |
| `Employee`           | Administrative/staff profile linked to an Identity user              |
| `Course`             | Academic course definition                                           |
| `Subject`            | Subject definition with credits and total classes                    |
| `SchoolClass`        | Class instance linked to a course                                    |
| `Grade`              | Grade by student and subject                                         |
| `Attendance`         | Attendance by student and subject                                    |
| `Payment`            | Payment record tied to a student                                     |
| `Alert`              | Internal alert linked to an employee                                 |
| `CourseSubject`      | Many-to-many relation between courses and subjects                   |
| `TeacherSubject`     | Many-to-many relation between teachers and subjects                  |
| `TeacherSchoolClass` | Many-to-many relation between teachers and classes                   |

### Relationship Highlights

- One `User` can correspond to one `Student`, `Teacher`, or `Employee` profile.
- One `Course` can contain many `Subjects` via `CourseSubject`.
- One `Course` can have many `SchoolClasses`.
- One `Teacher` can teach many `Subjects` and many `SchoolClasses`.
- One `Student` belongs to one optional `SchoolClass`.
- One `Student` can have many `Grades`, `Attendances`, and `Payments`.

### Database Initialization

The application currently seeds the database through `SeedDb` using:

- `EnsureCreatedAsync()`

This means the app can create the database automatically at startup if the connection string is valid.

> Migration files are present, but the current startup flow relies on `EnsureCreatedAsync()` rather than `Database.Migrate()`.

---

## API Endpoint

### Get Students by School Class

```http
GET /api/StudentsApi/BySchoolClass/{schoolClassId}
```

### Example

```http
GET /api/StudentsApi/BySchoolClass/1
```

### Response Shape

```json
[
  {
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "status": 1,
    "enrollmentDate": "2026-03-31T00:00:00",
    "schoolClassId": 1
  }
]
```

---

## Seed Data

On first run, the application creates roles and inserts sample data.

### Seeded Roles

- `Admin`
- `Student`
- `Teacher`
- `Employee`
- `Anonymous`
- `Pending`

### Seeded Users

| Role     | Email                  | Password       |
| -------- | ---------------------- | -------------- |
| Admin    | `admin@school.com`     | `Admin123!`    |
| Student  | `student1@school.com`  | `Student123!`  |
| Student  | `student2@school.com`  | `Student123!`  |
| Teacher  | `teacher1@school.com`  | `Teacher123!`  |
| Employee | `employee1@school.com` | `Employee123!` |

### Seeded Domain Data

- 2 school classes
- 2 subjects
- 2 courses
- 1 teacher profile
- 1 employee profile

### Important Seed Note

The current seed creates **student user accounts**, but it does **not create matching `Student` domain records** in `SeedDb`.
So if you want full student flows such as `MyGrades` or `MyAttendances`, you should create student profiles after startup.

---

## Prerequisites

Before running the project, make sure you have:

- **.NET 8 SDK**
- **SQL Server / SQL Server Express / LocalDB**
- **Visual Studio 2022** or **VS Code with C# support**
- **Azurite** if you want to use local blob storage with `UseDevelopmentStorage=true`

---

## Configuration

The main development configuration file is:

```text
appsettings.Development.json
```

You should update the configuration to match your local environment.

### Example Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SchoolManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Mail": {
    "NameFrom": "School Management System",
    "From": "your_email@gmail.com",
    "Smtp": "smtp.gmail.com",
    "Port": "587",
    "Password": "your_app_password"
  },
  "Blob": {
    "ConnectionStrings": "UseDevelopmentStorage=true"
  },
  "Tokens": {
    "Issuer": "SchoolManagementSystem",
    "Audience": "SchoolManagementSystemUsers",
    "Key": "YOUR_SECRET_KEY_AT_LEAST_32_CHARACTERS"
  }
}
```

### Configuration Notes

- `DefaultConnection`: SQL Server connection string
- `Mail`: used for email confirmation and password recovery
- `Blob:ConnectionStrings`: image upload storage configuration
- `Tokens`: JWT bearer configuration

### Security Note

The uploaded project contains development configuration values. Before publishing the repository publicly, you should:

- remove real secrets from `appsettings.*`
- use **User Secrets**, **Environment Variables**, or a secure secret manager
- rotate any exposed credentials

---

## How to Run

### 1. Restore dependencies

```bash
dotnet restore
```

### 2. Run the application

```bash
dotnet run
```

### 3. Open the application

Based on `launchSettings.json`, the application runs on:

- `http://localhost:5013`
- `https://localhost:7017`

You may also run through **IIS Express** from Visual Studio.

---

## Suggested Demo Flow

If you want to present the system in a demo or defense:

1. Start the application.
2. Login as **Admin**.
3. Show the Admin dashboard.
4. Register a new account.
5. Create an employee/teacher/student profile from pending users.
6. Show employee management.
7. Show student management.
8. Show teacher management.
9. Show course, subject, and class management.
10. Add grade and attendance records.
11. Login as a student and show `MyGrades` and `MyAttendances`.
12. Login as an employee and create an alert.
13. Login as an admin and review unresolved alerts.

---

## Known Notes and Limitations

- `AlertController` does not currently have explicit `[Authorize]` attributes.
- `PaymentsController` does not currently have explicit `[Authorize]` attributes.
- `StudentsApiController` does not currently have explicit `[Authorize]` attributes.
- Teacher role currently has a dashboard, but very limited explicitly authorized academic actions.
- Seed data creates student users but not full student entity records.
- Startup uses `EnsureCreatedAsync()` instead of migration-based schema updates.
- Some image paths depend on Azure Blob storage conventions and may need local adaptation.

---

## Future Improvements

- Add explicit authorization to all sensitive controllers and API routes
- Add full teacher workflows such as subject-based grading and attendance entry
- Add pagination, filtering, search, and better dashboard analytics
- Replace `EnsureCreatedAsync()` with migration-based startup for production
- Add audit logging for CRUD operations
- Add stronger validation and domain constraints
- Improve payment authorization and reporting
- Add Swagger/OpenAPI for APIs
- Add unit tests and integration tests

---

## Summary

This project is a solid academic **Student Management System** foundation that already includes:

- multi-role authentication
- CRUD management for core academic entities
- self-service student features
- alert handling
- payment records
- repository-based data access
- email and image upload infrastructure

It is suitable as a course project, capstone foundation, or an academic demonstration system, and it can be extended further into a more production-ready school ERP style application.
