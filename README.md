# Realtime Lab 即時互動平台

即時投票、排隊叫號、互動點歌、協作白板與營運儀表板。一個 monorepo 放三個專案，兩個後端依特性分工。

| 專案 | 技術 | 負責 |
| --- | --- | --- |
| `web/` | Nuxt 4（Vue 3、Tailwind CSS） | 頁面與 SSR；`/api/*` 轉送給 api |
| `api/` | ASP.NET Core 10（C#、EF Core） | 會員、CRUD、設定、報表、AI、對外整合、MVC 管理後台 |
| `core/` | Go | 即時 WebSocket、高併發寫入、原子操作、排程 |

資料層：MySQL 8（唯一事實來源）、Redis（快取與即時狀態）、MongoDB（稽核與紀錄）。

## 快速開始

需要 Docker Desktop。

```bash
cp .env.example .env          # 改掉裡面的密碼與 JWT_SECRET
docker compose up -d --build  # 啟動六個服務
```

| 服務 | 網址 |
| --- | --- |
| 前端 | http://localhost:3000 |
| 系統狀態（經 Nuxt → api → core） | http://localhost:3000/api/system/status |
| Go 健康檢查 | http://localhost:8081/healthz |

## 本機開發

資料庫類服務用 Compose 起，三個專案各自用熱重載跑：

```bash
docker compose up -d mysql redis mongo

# api（.NET 10 SDK）
cd api && dotnet watch --project src/RealtimeLab.Api

# core（Go 1.26+）
cd core && go run ./cmd/server

# web（Node 24）
pnpm install && pnpm dev        # 在根目錄執行，會轉給 web/
# 或：cd web && pnpm install && pnpm dev
```

本機跑時各服務的位址：web 3000、api 5080、core 8080。設定見各專案的設定檔與 `.env.example`。

## 資料表與 migration

- schema 唯一來源是 `api/` 的 EF Core migration
- 改了 Entity 之後：
  ```bash
  cd api
  dotnet ef migrations add <名稱> --project src/RealtimeLab.Api
  dotnet ef migrations script --project src/RealtimeLab.Api -o ../db/schema.sql
  ```
- `core/` 的 sqlc 讀 `db/schema.sql` 產生 Go 程式碼：`cd core && sqlc generate`

## 文件

- `docs/adr/`：架構決策紀錄
- `docs/ai-workflow.md`：AI 輔助開發流程與紀錄
- `core/openapi.yaml`：Go 內部 API 規格
- `CLAUDE.md`：給 AI 工具的專案規則
