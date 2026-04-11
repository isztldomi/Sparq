# Sparq WebAPI

Ez a projekt egy .NET alapú webalkalmazás, amely PostgreSQL adatbázist használ.

---

## Elindítás

### 1. Előfeltételek

A projekt futtatásához az alábbiak szükségesek:

- .NET SDK (.NET 10)
- PostgreSQL adatbázis szerver
- Opcionálisan: pgAdmin vagy más adatbázis kezelő

---

### 2. PostgreSQL provider

A projekt PostgreSQL-t használ Entity Framework Core-on keresztül:

```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.1" />
```

A projekt érzékeny beállításai User Secrets segítségével vannak kezelve.

```bash
dotnet user-secrets init --project Sparq.WebApi
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=SparqDB;Username=postgresUser;Password=0000" --project Sparq.WebApi
dotnet user-secrets set "JwtSettings:SecretKey" "DEV_ONLY_SUPER_SECRET_KEY" --project Sparq.WebApi
dotnet user-secrets set "JwtSettings:Audience" "SparqClient" --project Sparq.WebApi
dotnet user-secrets set "JwtSettings:Issuer" "SparqApi" --project Sparq.WebApi
dotnet user-secrets set "JwtSettings:AccessTokenExpirationMinutes" "15" --project Sparq.WebApi
```

Végső file struktúra:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=SparqDB;Username=postgresUser;Password=0000"
  },
  "JwtSettings": {
    "SecretKey": "DEV_ONLY_SUPER_SECRET_KEY",
    "Audience": "SparqClient",
    "Issuer": "SparqApi",
    "AccessTokenExpirationMinutes": 15
  }
}
```

---

## 🖥️ 3. Frontend (sparq.react.next)

A projekt frontend része a `sparq.react.next` mappában található.

### 1. Telepítés

Első lépésként telepítsd a függőségeket:

```bash
npm install
```

Hozz létre egy .env file-t a projekt gyökerében és tedd bele ezt: VITE_APP_API_BASEURL=/api

Linux

```bash
echo "VITE_APP_API_BASEURL=/api" > .env
```

Windows

```bash
echo VITE_APP_API_BASEURL=/api > .env
```

Majd indítsuk el

```bash
npm run dev
```
