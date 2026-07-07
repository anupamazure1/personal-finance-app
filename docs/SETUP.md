# Setup Instructions

## Prerequisites

- macOS 11 or later
- .NET 8.0 SDK
- MySQL 8.0
- Node.js 18+
- Git

## Installation Steps

### 1. Install .NET 8.0

```bash
# Using Homebrew
brew install dotnet-sdk@8

# Verify installation
dotnet --version
```

### 2. Install MySQL

```bash
# Using Homebrew
brew install mysql

# Start MySQL service
brew services start mysql

# Verify installation
mysql --version
```

### 3. Install Node.js

```bash
# Using Homebrew
brew install node

# Verify installation
node --version
npm --version
```

### 4. Clone Repository

```bash
git clone https://github.com/anupamazure1/personal-finance-app.git
cd personal-finance-app
```

## Backend Setup

### 1. Restore Dependencies

```bash
cd backend/PersonalFinance.Api
dotnet restore
```

### 2. Configure Database Connection

Edit `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PersonalFinance;User=root;Password=;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### 3. Create Database

```bash
# Using MySQL CLI
mysql -u root -e "CREATE DATABASE PersonalFinance;"

# Or use EntityFramework migrations
cd backend/PersonalFinance.Data
dotnet ef database update
```

### 4. Run Backend

```bash
cd backend/PersonalFinance.Api
dotnet run
```

API will be available at `http://localhost:5000`

## Frontend Setup

### 1. Install Dependencies

```bash
cd frontend
npm install
```

### 2. Configure API URL

Create `.env` file in `frontend` directory:

```bash
VITE_API_URL=http://localhost:5000
```

### 3. Run Frontend

```bash
npm run dev
```

UI will be available at `http://localhost:5173`

## Access from LAN

### Find Your Mac's Local IP

```bash
# Get WiFi IP
ipconfig getifaddr en0

# Get Ethernet IP
ipconfig getifaddr en1
```

### Configure for LAN Access

#### 1. Update Backend URL in Frontend

```bash
# Replace localhost with your Mac's IP in .env
VITE_API_URL=http://192.168.x.x:5000
```

#### 2. Update Backend CORS Policy

Edit `backend/PersonalFinance.Api/Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLAN", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowLAN");
```

#### 3. Update Backend Kestrel Settings

Edit `appsettings.json`:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:5000"
      }
    }
  }
}
```

#### 4. Configure Firewall

Allow inbound connections on ports 5000 and 5173:

```bash
# Option 1: Disable firewall temporarily
sudo /usr/libexec/ApplicationFirewall/socketfilterfw --setglobalstate off

# Option 2: Add specific rules (recommended)
sudo /usr/libexec/ApplicationFirewall/socketfilterfw --add /usr/local/bin/dotnet --unblockapp /usr/local/bin/dotnet
```

#### 5. Access from Other Devices

- Frontend: `http://192.168.x.x:5173`
- API: `http://192.168.x.x:5000`

## Using Docker Compose

### 1. Install Docker

```bash
brew install docker docker-compose
```

### 2. Build and Run

```bash
# Build all services
docker-compose build

# Start all services
docker-compose up -d

# Check status
docker-compose ps

# View logs
docker-compose logs -f
```

### 3. Access Services

- Frontend: `http://localhost:5173`
- API: `http://localhost:5000`
- MySQL: `localhost:3306`

## Troubleshooting

### MySQL Connection Issues

```bash
# Check if MySQL is running
brew services list | grep mysql

# Restart MySQL
brew services restart mysql

# Check MySQL logs
cd /usr/local/var/mysql && tail -f *.err
```

### Port Already in Use

```bash
# Find process using port 5000
lsof -i :5000

# Kill process
kill -9 <PID>
```

### Entity Framework Issues

```bash
# Remove old migrations
rm -rf Migrations/

# Create new migrations
cd backend/PersonalFinance.Data
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Frontend Build Issues

```bash
# Clear node_modules and reinstall
cd frontend
rm -rf node_modules package-lock.json
npm install
```

## Development Workflow

### Running in Development Mode

```bash
# Terminal 1: MySQL
brew services start mysql

# Terminal 2: Backend
cd backend/PersonalFinance.Api
dotnet watch run

# Terminal 3: Frontend
cd frontend
npm run dev
```

### Monitoring

```bash
# Backend logs
cd backend/PersonalFinance.Api && dotnet run > app.log 2>&1 &
tail -f app.log

# MySQL logs
tail -f /usr/local/var/mysql/$(hostname).err
```

## Next Steps

1. Create a family profile through the UI
2. Add family members
3. Add bank accounts
4. Upload test bank statements
5. View the dashboard

## Common Commands

```bash
# Backend - Create migration
cd backend/PersonalFinance.Data
dotnet ef migrations add MigrationName

# Backend - Update database
dotnet ef database update

# Frontend - Build production
cd frontend && npm run build

# Docker - View logs
docker-compose logs -f api

# Docker - Remove all
docker-compose down -v
```
