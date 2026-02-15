# Docker MySQL Setup

## Running MySQL Container

To start the MySQL container with the VideoUploadDb database:

```bash
docker-compose up -d
```

## Connection String

Update your `appsettings.json` with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=VideoUploadDb;User=videouser;Password=videopassword;Port=3306;"
  }
}
```

## Database Credentials

- **Root Password**: rootpassword
- **Database**: VideoUploadDb
- **User**: videouser
- **Password**: videopassword
- **Port**: 3306

## Stopping the Container

```bash
docker-compose down
```

To remove volumes (deletes all data):

```bash
docker-compose down -v
```

