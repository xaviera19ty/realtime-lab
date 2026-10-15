-- core 只讀 users（寫入方是 api）
-- name: GetUserByID :one
SELECT id, email, name, role
FROM users
WHERE id = ?;
