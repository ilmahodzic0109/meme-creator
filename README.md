# Meme Creator 
## 1) Features
- Upload PNG/JPG
- Edit meme config - top/bottom text, font, size, colors, stroke, align, padding, ALL CAPS
- Optional watermark + position
---
## 2) Tech Stack
- **Backend:** .NET (ASP.NET Core), SkiaSharp for image rendering
- **Frontend:** React + Vite + Bootstrap
- **Database:** MS SQL Server
- **Containerization:** Docker, Docker Compose
---
## 3) Prerequisites 
- Node.js 18+
- .NET SDK 8 (or your project’s target SDK)
- Docker Desktop (recommended)
- SQL Server (only needed if running without Docker)

---
## 4) Run Locally (No Docker)

### Backend
- Navigate to the backend project directory:

```bash
cd backend/MemeCreator.Api/MemeCreator.Api
dotnet restore
dotnet run
```
- Backend runs on: https://localhost:7012 (Swagger enabled in Development)

### Database (Local)
Provide a SQL Server connection string via:
- appsettings.Development.json (recommended for local dev), or
- environment variable: ConnectionStrings__DefaultConnection="Server=localhost;Database=MemeCreatorDb;Trusted_Connection=True;TrustServerCertificate=True;"
- Ensure SQL Server is running. The database schema is created by applying EF Core migrations (see commands below).

### Frontend
- Navigate to the frontend project directory:
```bash
cd frontend
npm install
npm run dev
```
Create a .env file in the frontend root directory:
- VITE_API_BASE_URL=https://localhost:7012
- After changing environment variables, restart the frontend dev server.
---
## 5) Run with Docker (Recommended)

### Prerequisites
- Docker + Docker Compose

### Environment file
1) Create `.env` in the project root (do not commit this file).
2) Use `.env.example` as a template

### Database / Migrations
The backend uses Entity Framework Core (code-first) and migrations are included in the repository.

- Migrations are **not applied automatically** on application startup.
- The database schema must be created by applying migrations once.

#### Apply migrations (Local)
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update --project backend/MemeCreator.Infrastructure --startup-project backend/MemeCreator.Api
```

#### Apply migrations (Docker)
Start containers:
```bash
docker compose up -d db
```
Apply migrations from your host machine:
```bash
dotnet ef database update --project backend/MemeCreator.Infrastructure --startup-project backend/MemeCreator.Api
```
After the first migration, you can start the full stack:
```bash
docker compose up -d --build
```
### Access the Application
- Frontend docker: http://localhost:3000
- Backend API (Swagger) docker: http://localhost:5000/swagger
### Logs / Stop
- docker compose logs -f
- docker compose down
- Optional remove volumes (deletes DB data): docker compose down -v
### Testing
- Open the frontend and use the meme editor UI
- Verify API endpoints via Swagger UI
- Confirm database persistence by restarting containers
- upload image
- preview returns image
- generate triggers download
- POST /api/config returns id, PUT works
### Stopping the Application
 ```bash 
docker compose down
```
---
## 6) Project Structure (High-Level)
### Backend (Layered Architecture)
- Api, Controllers, DTOs, Application, Interfaces, Services (business logic), Domain, Entities, Infrastructure,Repositories, Database access

### Frontend 
- pages/ – route-level pages (HomePage, MemeEditorPage)
- components/ – reusable UI components (ControlsPanel, PreviewPane)
- hooks/ – state management and orchestration logic (useMemeEditor)
- api/ – HTTP API communication (configApi, memeApi)
- models/ – TypeScript models and types (MemeConfig)
---
## 7) Deployment (Optional)
- This solution was deployed on an Azure Linux VM using Docker Compose.
- Steps: 
- Create an Ubuntu VM in Azure
- Install Docker + Docker Compose plugin on the VM
- Open inbound ports on the VM Network Security Group (NSG):
- TCP 3000 (frontend)
- TCP 5000 (backend)
- SSH into the VM
- Clone the repository to the VM
- Configure production .env on the VM (use VM public IP for frontend API base)
- Run docker compose up -d --build
- SSH only from my IP (recommended)
- Do NOT expose 1433 publicly 
### Access (Deployed)
- Frontend: http://52.232.25.174:3000/
- Backend Swagger: http://52.232.25.174:5000/swagger/index.html