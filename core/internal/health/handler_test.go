package health

import (
	"context"
	"errors"
	"net/http"
	"net/http/httptest"
	"testing"
)

type fakePinger struct{ err error }

func (f fakePinger) PingContext(context.Context) error { return f.err }

func TestReadyz(t *testing.T) {
	down := errors.New("connection refused")
	tests := []struct {
		name  string
		mysql error
		redis error
		want  int
	}{
		{"all up", nil, nil, http.StatusOK},
		{"mysql down", down, nil, http.StatusServiceUnavailable},
		{"redis down", nil, down, http.StatusServiceUnavailable},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			h := NewHandler(fakePinger{tt.mysql}, fakePinger{tt.redis})
			rec := httptest.NewRecorder()
			h.Readyz(rec, httptest.NewRequest(http.MethodGet, "/readyz", nil))
			if rec.Code != tt.want {
				t.Fatalf("status = %d, want %d", rec.Code, tt.want)
			}
		})
	}
}

func TestHealthz(t *testing.T) {
	h := NewHandler(fakePinger{}, fakePinger{})
	rec := httptest.NewRecorder()
	h.Healthz(rec, httptest.NewRequest(http.MethodGet, "/healthz", nil))
	if rec.Code != http.StatusOK {
		t.Fatalf("status = %d, want 200", rec.Code)
	}
}
