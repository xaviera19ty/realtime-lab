package platform

import "os"

// Config 從環境變數讀取設定；預設值給本機開發用。
type Config struct {
	Port      string
	MySQLDSN  string
	RedisAddr string
	JWTSecret string
}

func LoadConfig() Config {
	return Config{
		Port:      getenv("PORT", "8080"),
		MySQLDSN:  getenv("MYSQL_DSN", "realtime:change-me@tcp(127.0.0.1:3306)/realtime_lab?parseTime=true"),
		RedisAddr: getenv("REDIS_ADDR", "127.0.0.1:6379"),
		JWTSecret: getenv("JWT_SECRET", ""),
	}
}

func getenv(key, fallback string) string {
	if v := os.Getenv(key); v != "" {
		return v
	}
	return fallback
}
