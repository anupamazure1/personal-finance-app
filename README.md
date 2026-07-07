# Personal Finance App

A comprehensive personal finance management application built with .NET and MySQL for managing multiple family members, bank accounts, and investment portfolios across various Indian banks.

## Features

- **Multi-Family & Multi-Account Management**: Track finances for multiple family members and their various bank accounts
- **Bank Statement Parsing**: Automatic parsing of statements from:
  - HDFC Bank
  - ICICI Bank
  - Kotak Bank
  - Yes Bank
- **Account Types**: Support for:
  - Savings Accounts
  - Current Accounts
  - Fixed Deposits (FD)
  - Mutual Fund Accounts
  - Trading Accounts
  - Demat Accounts
- **Unified Dashboard**: View consolidated net worth for each family member
- **Local LAN Access**: Host on your MacBook and access from any device on your local network
- **Transaction Management**: Categorized transaction tracking and analytics

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: MySQL 8.0
- **Frontend**: React with Vite
- **ORM**: Entity Framework Core
- **File Processing**: NPOI (for Excel), CsvHelper

## Project Structure

```
personal-finance-app/
├── backend/
│   ├── PersonalFinance.Api/          # Main API project
│   ├── PersonalFinance.Core/         # Business logic & services
│   ├── PersonalFinance.Data/         # Data access layer (EF Core)
│   ├── PersonalFinance.Domain/       # Domain entities
│   └── PersonalFinance.Tests/        # Unit tests
├── frontend/                          # React UI
├── docker-compose.yml                 # MySQL & API setup
├── docs/                              # Documentation
└── README.md
```

## Prerequisites

- .NET 8.0 SDK
- MySQL 8.0
- Node.js 18+ (for frontend)
- MacBook with macOS 11+

## Quick Start

### 1. Clone Repository
```bash
git clone https://github.com/anupamazure1/personal-finance-app.git
cd personal-finance-app
```

### 2. Start MySQL (Using Docker)
```bash
docker-compose up -d mysql
```

Or install locally:
```bash
brew install mysql
brew services start mysql
```

### 3. Setup Backend
```bash
cd backend/PersonalFinance.Api

# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update --project ../PersonalFinance.Data

# Run API
dotnet run
```

API will be available at `http://localhost:5000`

### 4. Setup Frontend
```bash
cd frontend
npm install
npm run dev
```

UI will be available at `http://localhost:5173`

## Access from LAN

### Find Your Mac's Local IP
```bash
ipconfig getifaddr en0
```

### Update Frontend Configuration
Edit `frontend/.env` and set:
```
VITE_API_URL=http://<YOUR_MAC_IP>:5000
```

### Access from Other Devices
- Frontend: `http://<YOUR_MAC_IP>:5173`
- API: `http://<YOUR_MAC_IP>:5000`

**Note**: Ensure your MacBook firewall allows inbound connections on ports 5000 and 5173, or disable firewall for LAN.

## Database Schema

### Core Tables
- **Families**: Family profiles
- **FamilyMembers**: Individual members in a family
- **Accounts**: Bank accounts (links member to account type)
- **TransactionCategories**: Predefined categories

### Bank-Specific Tables
- **HdfcBankTransactions**: HDFC transactions
- **IciciBankTransactions**: ICICI transactions
- **KotakBankTransactions**: Kotak transactions
- **YesBankTransactions**: Yes Bank transactions

### Account Type Support
Each account can be one of:
- Savings Account
- Current Account
- Fixed Deposit (FD)
- Mutual Fund Account
- Trading Account
- Demat Account

## API Endpoints

### Family Management
```
GET    /api/families              - List all families
POST   /api/families              - Create new family
GET    /api/families/{id}         - Get family details
PUT    /api/families/{id}         - Update family
DELETE /api/families/{id}         - Delete family
```

### Members
```
GET    /api/members               - List family members
POST   /api/members               - Add new member
GET    /api/members/{id}          - Get member details
GET    /api/members/{id}/net-worth - Get member's net worth
```

### Accounts
```
GET    /api/accounts              - List all accounts
POST   /api/accounts              - Add new account
GET    /api/accounts/{id}         - Get account details
PUT    /api/accounts/{id}         - Update account
DELETE /api/accounts/{id}         - Delete account
```

### Transactions
```
GET    /api/transactions          - List transactions (with filters)
POST   /api/transactions/hdfc     - Upload HDFC statement
POST   /api/transactions/icici    - Upload ICICI statement
POST   /api/transactions/kotak    - Upload Kotak statement
POST   /api/transactions/yesbank  - Upload Yes Bank statement
GET    /api/transactions/{id}     - Get transaction details
```

### Dashboard
```
GET    /api/dashboard/summary     - Overall summary
GET    /api/dashboard/net-worth   - Net worth by member
GET    /api/dashboard/accounts    - Account summary
```

## Bank Statement Formats

### HDFC Bank
- **Formats**: CSV, PDF
- **Parser**: Handles standard HDFC CSV export format
- **Expected Columns**: Transaction Date, Value Date, Cheque #, Reference #, Description, Amount, Balance

### ICICI Bank
- **Formats**: Excel, CSV
- **Parser**: Handles ICICI iconnect format
- **Expected Columns**: Transaction Date, Amount, Description, Cheque Number, Balance

### Kotak Bank
- **Formats**: CSV, PDF
- **Parser**: Handles Kotak statement format
- **Expected Columns**: Txn Date, Particulars, Cheque #, Amount, Balance

### Yes Bank
- **Formats**: Excel, PDF
- **Parser**: Handles Yes Bank statement format
- **Expected Columns**: Transaction Date, Description, Amount, Cheque Number, Balance

## Configuration

### Database Connection
Update `backend/PersonalFinance.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PersonalFinance;User=root;Password=your_password;"
  },
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:5000"
      }
    }
  }
}
```

### LAN Access
Update `backend/PersonalFinance.Api/Program.cs` to allow cross-origin requests:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("LanAccess", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

## Usage Example

### 1. Create a Family
```bash
curl -X POST http://localhost:5000/api/families \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Smith Family",
    "description": "Family Finance"
  }'
```

### 2. Add Family Member
```bash
curl -X POST http://localhost:5000/api/members \
  -H "Content-Type: application/json" \
  -d '{
    "familyId": 1,
    "name": "John Smith",
    "email": "john@example.com"
  }'
```

### 3. Add Bank Account
```bash
curl -X POST http://localhost:5000/api/accounts \
  -H "Content-Type: application/json" \
  -d '{
    "memberId": 1,
    "bank": "HDFC",
    "accountType": "Savings",
    "accountNumber": "00001234567890",
    "accountHolder": "John Smith",
    "currentBalance": 50000
  }'
```

### 4. Upload Bank Statement
```bash
curl -X POST http://localhost:5000/api/transactions/hdfc \
  -F "file=@statement.csv" \
  -F "accountId=1"
```

### 5. View Net Worth
```bash
curl http://localhost:5000/api/dashboard/net-worth
```

## Features Coming Soon

- [ ] Investment tracking with portfolio analysis
- [ ] Budget planning and spending analytics
- [ ] Bill payment reminders
- [ ] Multi-currency support
- [ ] Mobile app (React Native)
- [ ] Tax optimization recommendations
- [ ] Wealth growth projections

## Contributing

Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

MIT License - see LICENSE file for details

## Support

For issues and questions, please open an issue on GitHub.

## Author

Created for personal finance management across Indian banks.
