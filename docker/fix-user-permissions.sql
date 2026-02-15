USE mysql;

DROP USER IF EXISTS 'videouser'@'%';
DROP USER IF EXISTS 'videouser'@'localhost';

CREATE USER 'videouser'@'%' IDENTIFIED WITH mysql_native_password BY 'videopassword';
CREATE USER 'videouser'@'localhost' IDENTIFIED WITH mysql_native_password BY 'videopassword';

GRANT ALL PRIVILEGES ON VideoUploadDb.* TO 'videouser'@'%';
GRANT ALL PRIVILEGES ON VideoUploadDb.* TO 'videouser'@'localhost';

FLUSH PRIVILEGES;

