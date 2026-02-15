# fiapx_video_api
Microsserviço responsável pela interação com usuário para upload e download de videos

## Arquitetura

- **Clean Architecture** com separação de responsabilidades
- Padrão **CQRS** usando MediatR
- Princípios de **Domain-Driven Design**
- **API RESTful** com versionamento
- **AWS S3** para armazenamento de arquivos
- **AWS SQS** para filas de mensagens
- **MySQL** para persistência de dados

## Pré-requisitos

- .NET 8 SDK
- MySQL 8.0+
- Docker e Docker Compose (opcional, para MySQL)
- Conta AWS com S3 e SQS configurados

## Configuração do Banco de Dados

### Opção 1: Usando Docker é o certo...

1. Inicie o container MySQL:
   ```bash
   docker-compose up -d
   ```

2. O banco de dados `VideoUploadDb` e a tabela `Videos` serão criados automaticamente.

3. Detalhes de conexão:
   - **Hostname:** 127.0.0.1
   - **Porta:** 3306
   - **Usuário:** root
   - **Senha:** admin
   - **Banco de Dados:** VideoUploadDb

### Opção 2: Configuração Manual do MySQL

Se você preferir usar uma instância MySQL existente ao invés do Docker:

1. **Conecte-se ao seu servidor MySQL** usando MySQL Workbench, MySQL CLI ou qualquer cliente MySQL.

2. **Execute os scripts SQL** na seguinte ordem:

   **Passo 1: Criar Banco de Dados**
   ```bash
   mysql -u root -p < docker/mysql/init/00-create-database.sql
   ```
   Ou execute o conteúdo diretamente:
   ```sql
   CREATE DATABASE IF NOT EXISTS `VideoUploadDb` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```

   **Passo 2: Criar Tabela**
   ```bash
   mysql -u root -p < docker/mysql/init/01-init-database.sql
   ```
   Ou execute o conteúdo diretamente:
   ```sql
   USE `VideoUploadDb`;
   
   na raiz tem um arquivo com o nome "CriarBanco.SQL" rode na mão.

 
3. **Alternativa: Use o script completo**
   
   Para facilitar, você pode usar o script completo que cria tanto o banco quanto a tabela:
   ```bash
   mysql -u root -p < docker/workbench-fix.sql
   ```

4. **Atualize a String de Conexão** em `src/WebApi/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=127.0.0.1;Database=VideoUploadDb;User=root;Password=sua_senha;Port=3306;AllowUserVariables=True;DefaultCommandTimeout=30;"
     }
   }
   ```

   Substitua `sua_senha` pela senha root do seu MySQL.

## Configuração

### Configuração AWS

Atualize `src/WebApi/appsettings.json` com suas credenciais AWS:

```json
{
  "AWS": {
    "S3": {
      "BucketName": "seu-bucket-name",
      "Region": "us-east-2",
      "AccessKey": "sua-access-key",
      "SecretKey": "sua-secret-key"
    },
    "SQS": {
      "QueueUrl": "https://sqs.region.amazonaws.com/account-id/queue-name.fifo",
      "Region": "us-east-2",
      "AccessKey": "sua-access-key",
      "SecretKey": "sua-secret-key"
    }
  }
}

hoje está default, configurada, mas pode ser alterada.
```

## Endpoints da API

### Upload de Vídeo
- **POST** `/v1/videos`
- **Content-Type:** `multipart/form-data`
- **Body:** `videoFile` (arquivo)

### Obter Status do Vídeo
- **GET** `/v1/videos/{id}`

### Download do Vídeo Processado
- **GET** `/v1/videos/{id}/download`

### Health Check
- **GET** `/v1/health`

## Estrutura do Projeto

```
src/
├── Domain/              # Entidades de domínio e lógica de negócio
├── Application/         # Casos de uso, interfaces e lógica da aplicação
├── Infrastructure/      # Serviços externos (S3, SQS, Banco de Dados)
└── WebApi/             # Controladores da API e configuração

tests/
├── Application.UnitTests/
└── WebApi.IntegrationTests/

docker/
└── mysql/
    └── init/           # Scripts SQL de inicialização
```

## Localização dos Scripts SQL

Todos os scripts SQL para configuração manual do banco de dados estão localizados em:
- `docker/mysql/init/00-create-database.sql` - Cria o banco de dados
- `docker/mysql/init/01-init-database.sql` - Cria a tabela Videos
- `docker/workbench-fix.sql` - Script completo (banco + tabela)

## Solução de Problemas

### Problemas de Conexão com o Banco de Dados

1. **Verifique se o MySQL está rodando:**
   ```bash
   # Para Docker
   docker ps
   
   # Para MySQL local
   mysqladmin -u root -p ping
   ```

2. **Teste a conexão:**
   ```bash
   mysql -u root -p -h 127.0.0.1 -P 3306
   ```

3. **Verifique se o banco existe:**
   ```sql
   SHOW DATABASES LIKE 'VideoUploadDb';
   ```

4. **Verifique se a tabela existe:**
   ```sql
   USE VideoUploadDb;
   SHOW TABLES;
   ```

### Requisitos da Fila SQS FIFO

A fila SQS deve ser uma fila FIFO e requer:
- `MessageGroupId` (definido automaticamente usando VideoId)
- `MessageDeduplicationId` (gerado automaticamente)
