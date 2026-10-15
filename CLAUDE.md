# Realtime Lab：給 AI 工具的專案規則

## 三個專案的分工

- `web/`：Nuxt 4，JavaScript（不用 TypeScript，型別提示用 JSDoc）。只放頁面，不放商業邏輯。
  瀏覽器的 `/api/*` 由 `server/routes/api/[...path].js` 轉送給 api。
- `api/`：ASP.NET Core 10（C#）。會員、JWT、一般 CRUD、設定、報表、AI、對外整合、MVC 管理後台。
  Controller 只做輸入驗證與呼叫 service，商業邏輯放 `Services/`。
- `core/`：Go。即時 WebSocket、高併發寫入、原子操作、排程。不提供建立或設定類 API。

判斷放哪裡：會被大量同時觸發、需要即時推播、或要靠原子操作守住正確性的放 core；其餘放 api。

## 資料規則

- MySQL 是唯一事實來源；Redis 只放可重建的快取與即時狀態。
- 每張表只有一個寫入方（api 或 core），另一方只能讀。新增資料表時在 PR 說明寫入方。
- schema 只由 api 的 EF Core migration 修改；改完要重新匯出 `db/schema.sql`。
- 一律參數化查詢，不手動拼 SQL 字串。

## API 規則

- 瀏覽器只呼叫 api 的 `/api/*`；core 的 `/v1/*` 只在內網。
- 改 core 的 API 時，同一個 PR 一起改 `core/openapi.yaml`。
- 錯誤格式統一為 `{ "code": "ALREADY_VOTED", "message": "..." }`。

## 測試指令

```bash
cd api  && dotnet test
cd core && go test ./...
cd web  && pnpm build
```

## 寫程式碼時

- 併發、交易、衝突處理的程式碼必須附測試。
- 不確定時先問，不要猜資料表欄位或 API 路徑。
