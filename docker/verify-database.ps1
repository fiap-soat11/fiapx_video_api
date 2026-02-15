Write-Host "Verificando banco de dados MySQL..." -ForegroundColor Cyan

Write-Host "`n1. Verificando se o container está rodando..." -ForegroundColor Yellow
docker ps --filter "name=video-upload-mysql" --format "table {{.Names}}\t{{.Status}}"

Write-Host "`n2. Listando bancos de dados..." -ForegroundColor Yellow
docker exec video-upload-mysql mysql -u root -padmin -e "SHOW DATABASES;" 2>$null

Write-Host "`n3. Verificando tabelas no VideoUploadDb..." -ForegroundColor Yellow
docker exec video-upload-mysql mysql -u root -padmin -e "USE VideoUploadDb; SHOW TABLES;" 2>$null

Write-Host "`n4. Estrutura da tabela Videos..." -ForegroundColor Yellow
docker exec video-upload-mysql mysql -u root -padmin -e "USE VideoUploadDb; DESCRIBE Videos;" 2>$null

Write-Host "`n✅ Verificação concluída!" -ForegroundColor Green
Write-Host "`nSe o banco não aparecer no MySQL Workbench:" -ForegroundColor Yellow
Write-Host "  1. Clique com botão direito em 'SCHEMAS' e selecione 'Refresh All'" -ForegroundColor White
Write-Host "  2. Ou desconecte e reconecte a conexão" -ForegroundColor White
Write-Host "  3. Verifique se está usando: Host=127.0.0.1, Port=3306, User=root, Password=admin" -ForegroundColor White

