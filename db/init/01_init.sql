CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL DEFAULT 'Member',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
-- 管理者ユーザのサンプルデータ（パスワード: admin123）
INSERT INTO users (email, password_hash, role)
VALUES ('admin@example.com', 'c2FsdA==.hash', 'Admin') ON CONFLICT (email) DO NOTHING;