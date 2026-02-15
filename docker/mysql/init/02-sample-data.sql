USE VideoUploadDb;

INSERT INTO Videos (Id, FileName, OriginalFileName, S3Key, S3ZipKey, Status, FileSize, ContentType, CreatedAt, UpdatedAt)
VALUES 
    ('01234567-89ab-cdef-0123-456789abcdef', '01234567-89ab-cdef-0123-456789abcdef.mp4', 'sample-video.mp4', 'videos/01234567-89ab-cdef-0123-456789abcdef/original/01234567-89ab-cdef-0123-456789abcdef.mp4', 'videos/01234567-89ab-cdef-0123-456789abcdef/processed/frames.zip', 3, 10485760, 'video/mp4', NOW(), NOW()),
    ('01234567-89ab-cdef-0123-456789abcde0', '01234567-89ab-cdef-0123-456789abcde0.mov', 'example-video.mov', 'videos/01234567-89ab-cdef-0123-456789abcde0/original/01234567-89ab-cdef-0123-456789abcde0.mov', NULL, 1, 5242880, 'video/quicktime', NOW(), NOW())
ON DUPLICATE KEY UPDATE UpdatedAt = NOW();

