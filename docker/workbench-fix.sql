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

SELECT 'Banco VideoUploadDb e tabela Videos criados com sucesso!' as Status;
SELECT COUNT(*) as TotalTabelas FROM information_schema.tables WHERE table_schema = 'VideoUploadDb';

