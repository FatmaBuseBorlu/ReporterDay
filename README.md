# ReporterDay

ReporterDay is a layered ASP.NET Core MVC blog/news platform. It includes article publishing, category and slider management, author flows, user registration/login with ASP.NET Core Identity, comments, slug-based article URLs, protected article identifiers, and AI-assisted comment moderation with Hugging Face.

This project is a stronger portfolio piece because it demonstrates multi-layered .NET architecture, EF Core, Identity, validation, data access abstractions, ViewComponents, and external AI service integration.

## Features

- Public blog/news homepage
- Article listing and article detail pages
- Slug-based article URLs
- Category management
- Slider management
- Author article creation flow
- User registration and login with ASP.NET Core Identity
- Comment system
- Comment moderation with blocked words
- Optional Hugging Face toxicity model integration
- Memory cache for moderation responses
- Protected article IDs with ASP.NET Core Data Protection
- ViewComponent-based page composition
- FluentValidation-based validation rules
- Layered architecture with Entity, Data Access, Business, and Presentation layers

## Tech Stack

- ASP.NET Core MVC 8.0
- C#
- Entity Framework Core 8
- SQL Server
- ASP.NET Core Identity
- Razor Views
- ViewComponents
- FluentValidation
- MemoryCache
- HttpClient
- Hugging Face inference API integration
- Data Protection API
- Bootstrap / ZenBlog template

## Solution Structure

```text
ReporterDay/
├── ReporterDay.sln
├── ReporterDay.EntityLayer/
│   └── Entities/
├── ReporterDay.DataAccessLayer/
│   ├── Context/
│   ├── EntityFramework/
│   ├── Migrations/
│   └── Repositories/
├── BusinessLayer/
│   ├── Abstract/
│   ├── Concrete/
│   ├── Models/
│   └── ValidationRules/
├── ReporterDay.PresentationLayer/
│   ├── Controllers/
│   ├── Models/
│   ├── Security/
│   ├── ViewComponents/
│   ├── Views/
│   ├── Program.cs
│   └── appsettings.json
└── README.md
```

## Requirements

Before running the project, make sure the following tools are installed:

- .NET 8 SDK
- SQL Server, SQL Server Express, or LocalDB
- Visual Studio 2022 or Visual Studio Code
- EF Core CLI tools

Install EF Core CLI tools if they are not already installed:

```bash
dotnet tool install --global dotnet-ef
```

If the tool is already installed, update it:

```bash
dotnet tool update --global dotnet-ef
```

## Database Setup

The project uses SQL Server with EF Core migrations.

The connection string is currently configured in:

```text
ReporterDay.DataAccessLayer/Context/ArticleContext.cs
```

Current connection string:

```csharp
Server=DESKTOP-NBRMDOS;initial catalog=ReporterBlogDayDb;integrated security=true;trust server certificate=true
```

Before running the project locally, update the `Server` value according to your SQL Server setup.

For SQL Server Express:

```csharp
Server=.\\SQLEXPRESS;initial catalog=ReporterBlogDayDb;integrated security=true;trust server certificate=true
```

For LocalDB:

```csharp
Server=(localdb)\\MSSQLLocalDB;initial catalog=ReporterBlogDayDb;integrated security=true;trust server certificate=true
```

Apply migrations:

```bash
dotnet ef database update --project ReporterDay.DataAccessLayer/ReporterDay.DataAccessLayer.csproj --startup-project ReporterDay.PresentationLayer/ReporterDay.PresentationLayer.csproj
```

This creates the `ReporterBlogDayDb` database and required tables for articles, categories, comments, sliders, tags, and Identity users.

## Optional AI Moderation Setup

The project supports optional Hugging Face-based toxicity detection for comments.

Default model settings are located in:

```text
ReporterDay.PresentationLayer/appsettings.json
```

The API token should not be committed to the repository. Use user secrets:

```bash
dotnet user-secrets set "HuggingFace:ApiToken" "your-huggingface-token" --project ReporterDay.PresentationLayer/ReporterDay.PresentationLayer.csproj
```

If no token is configured, the moderation service reports the model as unavailable. The project also supports blocked-word moderation through `CommentModeration:BlockedWords`.

## How to Run

Clone the repository:

```bash
git clone https://github.com/FatmaBuseBorlu/ReporterDay.git
```

Navigate into the project folder:

```bash
cd ReporterDay
```

Restore dependencies:

```bash
dotnet restore
```

Apply database migrations:

```bash
dotnet ef database update --project ReporterDay.DataAccessLayer/ReporterDay.DataAccessLayer.csproj --startup-project ReporterDay.PresentationLayer/ReporterDay.PresentationLayer.csproj
```

Run the application:

```bash
dotnet run --project ReporterDay.PresentationLayer/ReporterDay.PresentationLayer.csproj
```

Open the application in your browser using the localhost URL shown in the terminal.

## Main Routes

```text
/Default/Index              Public homepage
/Article/ArticleDetail      Article detail page
/Register/CreateUser        User registration
/Login/UserLogin            User login
/Author/CreateArticle       Author article creation
/Author/MyArticleList       Author article list
/Category/CategoryList      Category management
/Slider/SliderList          Slider management
```

In development mode, there are also diagnostic endpoints:

```text
/test-config    Shows whether Hugging Face token/config exists
/test-tox       Tests toxicity model integration
/_endpoints     Lists mapped endpoints
```

## What I Practiced

- Layered architecture in ASP.NET Core
- Entity Framework Core with SQL Server
- ASP.NET Core Identity integration
- Repository and service abstractions
- FluentValidation validation rules
- Article/category/comment domain modeling
- Slug generation for readable URLs
- ASP.NET Core Data Protection for ID handling
- ViewComponent-based UI composition
- AI-assisted comment moderation
- External API integration with HttpClient
- MemoryCache usage for repeated moderation checks

## Future Improvements

- Move the SQL Server connection string to `appsettings.json`
- Add role-based admin authorization
- Add seed data for categories, sliders, and sample articles
- Add pagination and search for article lists
- Add image upload support for articles
- Add unit/integration tests
- Add Docker Compose for SQL Server and the web app
- Add GitHub Actions CI workflow
- Add screenshots to the README

## Repository

GitHub: https://github.com/FatmaBuseBorlu/ReporterDay