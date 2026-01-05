# .NET Aspire + Nuxt UI Starter Template

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Aspire](https://img.shields.io/badge/Aspire-13.1-512BD4)](https://aspire.dev)
[![Nuxt](https://img.shields.io/badge/Nuxt-4.x-00DC82?logo=nuxt)](https://nuxt.com/)
[![Nuxt UI](https://img.shields.io/badge/Nuxt%20UI-4.x-00DC82)](https://ui.nuxt.com/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.x-3178C6?logo=typescript)](https://www.typescriptlang.org/)

A production-ready starter template combining **.NET Aspire** for cloud-native orchestration with **Nuxt UI** for a beautiful, modern frontend. Perfect for building full-stack applications with enterprise-grade observability, resilience, and developer experience.

## ✨ Features

- **🚀 .NET Aspire 13.1** - Cloud-native orchestration with built-in service discovery, health checks, and telemetry
- **⚡ .NET 10** - Latest .NET runtime with minimal APIs and OpenAPI support
- **🎨 Nuxt 4 + Nuxt UI 4** - Modern Vue 3 frontend with beautiful, accessible components, pre-configured with the [Nuxt UI SaaS template](https://github.com/nuxt-ui-templates/saas)
- **📊 OpenTelemetry** - Distributed tracing, metrics, and logging out of the box
- **🔄 Service Discovery** - Automatic service-to-service communication
- **💪 Resilience** - Built-in retry policies and circuit breakers via `Microsoft.Extensions.Http.Resilience`
- **📝 Content Management** - Nuxt Content for documentation, blog, and changelog
- **🔍 Full-Text Search** - Built-in content search powered by Nuxt Content
- **🎯 Type Safety** - End-to-end TypeScript with Zod validation
- **🤖 AI-Assisted Development** - GitHub Copilot [custom instructions](.github/copilot-instructions.md) for consistent code generation

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                     MyApp.AppHost (Aspire)                      │
│         Orchestration, Service Discovery, Telemetry             │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────┐         ┌─────────────────────────┐   │
│  │  MyApp.ApiService   │◄────────│      MyApp.Web          │   │
│  │   (.NET 10 API)     │         │   (Nuxt 4 Frontend)     │   │
│  │                     │         │                         │   │
│  │  • Minimal APIs     │         │  • Nuxt UI Components   │   │
│  │  • OpenAPI/Swagger  │         │  • Nuxt Content         │   │
│  │  • Health Checks    │         │  • VueUse Composables   │   │
│  └─────────────────────┘         └─────────────────────────┘   │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 📁 Project Structure

```
├── MyApp.AppHost/           # Aspire orchestration host
│   └── AppHost.cs           # Service configuration & orchestration
├── MyApp.ApiService/        # .NET backend API
│   └── Program.cs           # Minimal API endpoints
├── MyApp.ServiceDefaults/   # Shared Aspire configurations
│   └── Extensions.cs        # OpenTelemetry, health checks, resilience
├── MyApp.Web/               # Nuxt frontend application
│   ├── app/                 # Nuxt application code
│   │   ├── components/      # Vue components
│   │   ├── composables/     # Vue composables (useApi, etc.)
│   │   ├── layouts/         # Page layouts
│   │   └── pages/           # File-based routing
│   ├── content/             # Markdown content (docs, blog, changelog)
│   └── nuxt.config.ts       # Nuxt configuration
└── MyApp.slnx               # Solution file
```

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 20+](https://nodejs.org/) (LTS recommended)
- [pnpm](https://pnpm.io/) (package manager)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (optional, for containerized resources)

### Installation

1. **Clone or use as template:**
   ```bash
   git clone https://github.com/YOUR_USERNAME/aspire-nuxtui-starter.git
   cd aspire-nuxtui-starter
   ```

2. **Install frontend dependencies:**
   ```bash
   cd MyApp.Web
   pnpm install
   cd ..
   ```

3. **Run with Aspire:**
   ```bash
   dotnet run --project MyApp.AppHost
   ```

4. **Open the Aspire Dashboard:**
   
   Navigate to `https://localhost:17225` (or the URL shown in terminal) to view the Aspire dashboard with:
   - Service health status
   - Distributed traces
   - Metrics and logs
   - Service endpoints

### Running Frontend Independently

If you want to run only the Nuxt frontend:

```bash
cd MyApp.Web
pnpm dev
```

> **Note:** API calls will use the fallback URL configured in `nuxt.config.ts`. Update `NUXT_PUBLIC_API_BASE` environment variable to point to your API.

## 🔧 Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `NUXT_PUBLIC_API_BASE` | Backend API base URL | Auto-discovered via Aspire |
| `PORT` | Nuxt dev server port | Auto-assigned by Aspire |

### Aspire Service Configuration

The AppHost configures services with:
- **Health checks** - Automatic `/health` endpoint monitoring
- **Service discovery** - Services reference each other by name
- **External endpoints** - Frontend exposed externally

```csharp
// MyApp.AppHost/AppHost.cs
var apiService = builder.AddProject<Projects.MyApp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var frontend = builder.AddNpmApp("frontend", "../MyApp.Web", "dev")
    .WithReference(apiService)
    .WithEnvironment("NUXT_PUBLIC_API_BASE", apiService.GetEndpoint("http"));
```

## 🛠️ Development

### Adding API Endpoints

Add new endpoints in `MyApp.ApiService/Program.cs`:

```csharp
app.MapGet("/api/items", () => new[] { "Item1", "Item2" })
    .WithName("GetItems");
```

### Calling the API from Nuxt

Use the `useApi` composable:

```typescript
const { get, post } = useApi()

// GET request
const data = await get<WeatherForecast[]>('/weatherforecast')

// POST request
const result = await post<CreateResponse>('/items', { name: 'New Item' })
```

### Adding Content

Add Markdown files to `MyApp.Web/content/`:
- `1.docs/` - Documentation pages
- `3.blog/` - Blog posts
- `4.changelog/` - Release notes

## 🧪 Available Commands

### Backend (.NET)

```bash
# Run with Aspire (recommended)
dotnet run --project MyApp.AppHost

# Run API service only
dotnet run --project MyApp.ApiService

# Build solution
dotnet build

# Run tests
dotnet test
```

### Frontend (Nuxt)

```bash
cd MyApp.Web

# Development server
pnpm dev

# Build for production
pnpm build

# Preview production build
pnpm preview

# Lint code
pnpm lint

# Type check
pnpm typecheck
```

## 🌐 Deployment

### Azure Container Apps (Recommended)

.NET Aspire integrates seamlessly with Azure Container Apps:

```bash
# Install Azure Developer CLI
winget install Microsoft.Azd

# Initialize and deploy
azd init
azd up
```

### Other Deployment Options

- **Kubernetes** - Use Aspire's manifest generation: `dotnet run --project MyApp.AppHost -- --publisher manifest`
- **Docker Compose** - Generate with Aspire tooling
- **Vercel/Netlify** - Deploy Nuxt frontend separately (update API base URL)

## 📚 Key Technologies Documentation

### Backend
- [.NET Aspire](https://aspire.dev) - Cloud-native orchestration framework
- [.NET 10](https://learn.microsoft.com/dotnet/) - .NET runtime and SDK
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis) - Lightweight API development
- [OpenTelemetry .NET](https://opentelemetry.io/docs/languages/net/) - Observability instrumentation
- [Microsoft.Extensions.Http.Resilience](https://learn.microsoft.com/dotnet/core/resilience/) - Resilience patterns

### Frontend
- [Nuxt](https://nuxt.com/docs) - Vue.js meta-framework
- [Nuxt UI](https://ui.nuxt.com/docs) - UI component library
- [Nuxt Content](https://content.nuxt.com/) - File-based CMS
- [VueUse](https://vueuse.org/) - Vue composition utilities
- [Zod](https://zod.dev/) - TypeScript-first schema validation

### Development Tools
- [pnpm](https://pnpm.io/) - Fast, disk space efficient package manager
- [TypeScript](https://www.typescriptlang.org/docs/) - Typed JavaScript
- [ESLint](https://eslint.org/) - JavaScript/TypeScript linter

## 🤝 Using This Template

1. Click "Use this template" on GitHub or clone the repository
2. Rename the solution and projects as needed:
   - Update `.slnx` file
   - Rename project folders and `.csproj` files
   - Update namespaces in C# files
   - Update `package.json` name in `MyApp.Web`
3. Update this README with your project details
4. Start building your application!

## 📝 License

This template is open source and available under the [MIT License](LICENSE).

---

**Happy coding!** 🎉

If you find this template helpful, please ⭐ star the repository!
