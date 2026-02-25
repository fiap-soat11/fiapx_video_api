USE `VideoUploadDb`;

-- Tabela users (colunas em snake_case)
CREATE TABLE IF NOT EXISTS users (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT UQ_users_email UNIQUE (email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabela video_processings (colunas em snake_case)
CREATE TABLE IF NOT EXISTS video_processings (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    original_file_name VARCHAR(255) NOT NULL,
    status ENUM('Pending', 'Processing', 'Completed', 'Failed') NOT NULL DEFAULT 'Pending',
    s3_input_path VARCHAR(2048) NOT NULL,
    s3_output_path VARCHAR(2048) NULL,
    failure_reason TEXT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP NULL,
    CONSTRAINT FK_video_processings_users FOREIGN KEY (user_id) REFERENCES users (id),
    INDEX IX_video_processings_user_id (user_id),
    INDEX IX_video_processings_status (status),
    INDEX IX_video_processings_created_at (created_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Seed: usuário padrão (só insere se a tabela estiver vazia)
INSERT INTO users (name, email, password_hash, created_at, updated_at)
SELECT 'Default User', 'default@example.com', 'changeme', NOW(), NOW()
FROM DUAL
WHERE NOT EXISTS (SELECT 1 FROM users LIMIT 1);
