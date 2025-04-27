# CybergymHRS

## Setup Instructions

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js 14 or 16](https://nodejs.org/en/) (for Angular 11)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Angular CLI 11](https://angular.io/cli):

```bash
npm install -g @angular/cli@11

Running Locally (Without Docker)
Backend (API):
cd CybergymHRS
dotnet restore
dotnet ef database update
dotnet run

Running Locally (With Docker):

Create the DB inside the container:
docker exec -u 0 -it cybergym_sql /bin/bash
apt-get update
apt-get install -y curl gnupg
curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
curl https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/mssql-release.list
apt-get update
ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '0neStrongP4ssword!'