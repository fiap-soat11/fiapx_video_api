Write-Host "Criando banco de dados e tabela no MySQL..." -ForegroundColor Cyan

$sqlScript = @"
CREATE DATABASE IF NOT EXISTS `VideoUploadDb` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE `VideoUploadDb`;

CREATE TABLE IF NOT EXISTS Videos (
    Id CHAR(36) NOT NULL PRIMARY KEY,
    FileName VARCHAR(500) NOT NULL,
    OriginalFileName VARCHAR(500) NOT NULL,
    S3Key VARCHAR(1000) NOT NULL,
    S3ZipKey VARCHAR(1000) NULL,
    Status INT NOT NULL,
    FileSize BIGINT NOT NULL,
    ContentType VARCHAR(100) NOT NULL,
    CreatedAt DATETIME(6) NOT NULL,
    UpdatedAt DATETIME(6) NOT NULL,
    INDEX IX_Videos_Status (Status),
    INDEX IX_Videos_CreatedAt (CreatedAt)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
"@

$sqlScript | docker exec -i video-upload-mysql mysql -u root -padmin

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Banco e tabela criados com sucesso!" -ForegroundColor Green
    
    Write-Host "`nVerificando..." -ForegroundColor Yellow
    docker exec video-upload-mysql mysql -u root -padmin -e "SHOW DATABASES LIKE 'VideoUploadDb'; USE VideoUploadDb; SHOW TABLES; SELECT COUNT(*) as TableCount FROM information_schema.tables WHERE table_schema = 'VideoUploadDb';" 2>$null
    
    Write-Host "`n⚠️  IMPORTANTE: No MySQL Workbench, verifique:" -ForegroundColor Yellow
    Write-Host "  1. Hostname: 127.0.0.1 (NÃO localhost)" -ForegroundColor White
    Write-Host "  2. Port: 3306" -ForegroundColor White
    Write-Host "  3. Username: root" -ForegroundColor White
    Write-Host "  4. Password: admin" -ForegroundColor White
    Write-Host "  5. Após conectar, clique com botão direito em 'SCHEMAS' e selecione 'Refresh All'" -ForegroundColor White
} else {
    Write-Host "`n❌ Erro ao criar banco de dados" -ForegroundColor Red
}

