# MySQL Connection Guide

## Connection Settings for MySQL Workbench

### Root User (Full Access)
- **Hostname:** 127.0.0.1
- **Port:** 3306
- **Username:** root
- **Password:** admin
- **Default Schema:** (deixe vazio ou use `VideoUploadDb` - exatamente com maiúsculas)

**IMPORTANTE:** Se você receber erro "Unknown database 'videouploaddb'", deixe o campo "Default Schema" vazio no MySQL Workbench. O banco será selecionado automaticamente após a conexão ou você pode selecioná-lo manualmente.

## Connection String for Application

```
Server=127.0.0.1;Database=VideoUploadDb;User=root;Password=admin;Port=3306;AllowUserVariables=True;DefaultCommandTimeout=30;
```

## Troubleshooting

If you get "Access denied" error:

1. **Restart the container:**
   ```bash
   docker-compose restart mysql
   ```

2. **Check if container is running:**
   ```bash
   docker ps
   ```

3. **Check container logs:**
   ```bash
   docker logs video-upload-mysql
   ```

4. **Test connection from command line:**
   ```bash
   docker exec -it video-upload-mysql mysql -u root -padmin VideoUploadDb
   ```

5. **Recreate container (removes all data):**
   ```bash
   docker-compose down -v
   docker-compose up -d
   ```

## Important Notes

- Always use `127.0.0.1` instead of `localhost` for TCP/IP connections
- The root password is: `admin`
- The database name is case-sensitive: `VideoUploadDb` (com maiúsculas)
- If MySQL Workbench shows error about database, leave "Default Schema" empty and select it after connecting
- The root user uses `mysql_native_password` authentication plugin

## Se o banco não aparecer no MySQL Workbench

### Solução 1: Execute o script SQL no MySQL Workbench

1. Abra o MySQL Workbench e conecte-se com as credenciais acima
2. Abra uma nova query (File > New Query Tab ou Ctrl+T)
3. Copie e cole o conteúdo do arquivo `docker/workbench-fix.sql`
4. Execute o script (Execute button ou Ctrl+Shift+Enter)
5. Atualize a lista de schemas: Clique com botão direito em "SCHEMAS" > "Refresh All"

### Solução 2: Via PowerShell

Execute o script:
```powershell
.\docker\create-database.ps1
```

Depois, no MySQL Workbench:
- Clique com botão direito em "SCHEMAS" no painel esquerdo
- Selecione "Refresh All"

### Solução 3: Verificar conexão

Certifique-se de que está conectado ao container Docker:
- **Hostname:** `127.0.0.1` (NÃO use `localhost`)
- **Port:** `3306`
- **Username:** `root`
- **Password:** `admin`

### Verificar via terminal:
```powershell
.\docker\verify-database.ps1
```

Ou manualmente:
```bash
docker exec video-upload-mysql mysql -u root -padmin -e "SHOW DATABASES;"
docker exec video-upload-mysql mysql -u root -padmin -e "USE VideoUploadDb; SHOW TABLES;"
```
