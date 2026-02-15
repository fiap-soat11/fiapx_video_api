# Test MySQL Connection

## Using MySQL Client

```bash
mysql -h localhost -P 3306 -u videouser -pvideopassword VideoUploadDb
```

## Using Docker Exec

```bash
docker exec -it video-upload-mysql mysql -u videouser -pvideopassword VideoUploadDb
```

## Connection String Format

```
Server=localhost;Database=VideoUploadDb;User=videouser;Password=videopassword;Port=3306;
```

## Troubleshooting

If connection fails:

1. Check if container is running:
   ```bash
   docker ps
   ```

2. Check container logs:
   ```bash
   docker logs video-upload-mysql
   ```

3. Restart container:
   ```bash
   docker-compose restart mysql
   ```

4. Recreate container (removes all data):
   ```bash
   docker-compose down -v
   docker-compose up -d
   ```

