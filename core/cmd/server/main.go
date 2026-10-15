// core：Realtime Lab 的核心後端（即時、高併發、原子操作）。
package main

import (
	"context"
	"errors"
	"log/slog"
	"net/http"
	"os"
	"os/signal"
	"syscall"
	"time"

	"realtime-lab/core/internal/health"
	"realtime-lab/core/internal/httpx"
	"realtime-lab/core/internal/platform"
)

func main() {
	logger := slog.New(slog.NewJSONHandler(os.Stdout, nil))
	slog.SetDefault(logger)

	cfg := platform.LoadConfig()

	db, err := platform.OpenMySQL(cfg.MySQLDSN)
	if err != nil {
		logger.Error("open mysql", "err", err)
		os.Exit(1)
	}
	defer db.Close()

	rdb := platform.OpenRedis(cfg.RedisAddr)
	defer rdb.Close()

	mux := http.NewServeMux()
	h := health.NewHandler(db, platform.RedisPinger{Client: rdb})
	mux.HandleFunc("GET /healthz", h.Healthz)
	mux.HandleFunc("GET /readyz", h.Readyz)
	mux.HandleFunc("GET /v1/ping", func(w http.ResponseWriter, r *http.Request) {
		httpx.WriteJSON(w, http.StatusOK, map[string]string{
			"service":   "core",
			"requestId": httpx.RequestIDFrom(r.Context()),
		})
	})

	srv := &http.Server{
		Addr:              ":" + cfg.Port,
		Handler:           httpx.RequestID(httpx.Logging(logger, mux)),
		ReadHeaderTimeout: 5 * time.Second,
	}

	go func() {
		logger.Info("core listening", "addr", srv.Addr)
		if err := srv.ListenAndServe(); err != nil && !errors.Is(err, http.ErrServerClosed) {
			logger.Error("listen", "err", err)
			os.Exit(1)
		}
	}()

	// 優雅關機：收到 SIGTERM 先停止接新連線，等進行中的請求結束（之後 WebSocket hub 也在這裡廣播 server.restarting）
	ctx, stop := signal.NotifyContext(context.Background(), syscall.SIGINT, syscall.SIGTERM)
	defer stop()
	<-ctx.Done()

	shutdownCtx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer cancel()
	if err := srv.Shutdown(shutdownCtx); err != nil {
		logger.Error("shutdown", "err", err)
	}
	logger.Info("core stopped")
}
