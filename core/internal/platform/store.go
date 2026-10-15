package platform

import (
	"context"
	"database/sql"
	"time"

	_ "github.com/go-sql-driver/mysql"
	"github.com/redis/go-redis/v9"
)

// OpenMySQL 建立連線池；不在這裡 Ping，連不上時由 /readyz 回報。
func OpenMySQL(dsn string) (*sql.DB, error) {
	db, err := sql.Open("mysql", dsn)
	if err != nil {
		return nil, err
	}
	db.SetMaxOpenConns(25)
	db.SetMaxIdleConns(25)
	db.SetConnMaxLifetime(5 * time.Minute)
	return db, nil
}

func OpenRedis(addr string) *redis.Client {
	return redis.NewClient(&redis.Options{Addr: addr})
}

// RedisPinger 讓 health 套件不用直接依賴 go-redis。
type RedisPinger struct {
	Client *redis.Client
}

func (p RedisPinger) PingContext(ctx context.Context) error {
	return p.Client.Ping(ctx).Err()
}
