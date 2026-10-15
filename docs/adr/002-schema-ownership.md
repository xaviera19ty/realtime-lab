# ADR 002：schema 由 EF Core migration 管理，匯出 schema.sql 給 sqlc

## 背景

api（C#）與 core（Go）共用同一個 MySQL，兩邊都要知道資料表結構。

## 決定

- 唯一來源是 api 的 EF Core migration；api 啟動時自動套用
- 每次 migration 後用 `dotnet ef migrations script`（純 DDL，sqlc 才讀得懂） 匯出 `db/schema.sql`
- core 的 sqlc 讀 `db/schema.sql` 產生型別安全的 Go 程式碼
- 每張表只有一個寫入方，另一方只讀

## 取捨

- 優點：schema 只有一個地方能改，不會兩邊各寫 migration 打架
- 缺點：忘記重新匯出 schema.sql 時 core 會用到舊結構 → CI 加檢查
