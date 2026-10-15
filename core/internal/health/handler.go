// Package health 提供 /healthz（程式活著）與 /readyz（依賴服務連得上）。
package health

import (
	"context"
	"net/http"
	"time"

	"realtime-lab/core/internal/httpx"
)

type Pinger interface {
	PingContext(ctx context.Context) error
}

type Handler struct {
	mysql Pinger
	redis Pinger
}

func NewHandler(mysql, redis Pinger) *Handler {
	return &Handler{mysql: mysql, redis: redis}
}

func (h *Handler) Healthz(w http.ResponseWriter, _ *http.Request) {
	httpx.WriteJSON(w, http.StatusOK, map[string]string{"status": "ok"})
}

func (h *Handler) Readyz(w http.ResponseWriter, r *http.Request) {
	ctx, cancel := context.WithTimeout(r.Context(), 2*time.Second)
	defer cancel()

	checks := map[string]string{"mysql": "ok", "redis": "ok"}
	status := http.StatusOK
	if err := h.mysql.PingContext(ctx); err != nil {
		checks["mysql"] = err.Error()
		status = http.StatusServiceUnavailable
	}
	if err := h.redis.PingContext(ctx); err != nil {
		checks["redis"] = err.Error()
		status = http.StatusServiceUnavailable
	}
	httpx.WriteJSON(w, status, checks)
}
