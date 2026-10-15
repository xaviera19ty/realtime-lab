// 技能 ↔ 頁面對照：首頁技能地圖與各功能頁底部共用同一份資料。
// 新增功能或完成一個功能時，只改這個檔案。

/** @typedef {'done' | 'wip'} PageStatus */

export const pages = {
  polls: { name: '即時投票', path: '/polls', status: 'wip' },
  queues: { name: '排隊叫號', path: '/queues', status: 'wip' },
  songs: { name: '互動點歌', path: '/songs', status: 'wip' },
  boards: { name: '協作白板', path: '/boards', status: 'wip' },
  dashboard: { name: '營運儀表板', path: '/dashboard', status: 'wip' },
}

export const groups = [
  { id: 'frontend', name: '前端' },
  { id: 'api', name: '網頁後端（C#）' },
  { id: 'core', name: '核心後端（Go）' },
  { id: 'data', name: '資料與部署' },
  { id: 'ai', name: 'AI 與整合' },
]

export const skills = [
  { id: 'nuxt', name: 'Nuxt 4', group: 'frontend', usages: [
    { page: 'polls', what: '檔案式路由、SSR 分享頁' },
    { page: 'songs', what: '觀眾頁 SSR、OBS 疊加層' },
  ] },
  { id: 'tailwind', name: 'Tailwind・SCSS', group: 'frontend', usages: [
    { page: 'songs', what: '透明背景疊加層與換歌動畫' },
    { page: 'polls', what: '嵌入式小工具樣式' },
  ] },
  { id: 'pwa', name: 'PWA・Web Push', group: 'frontend', usages: [
    { page: 'queues', what: '加到主畫面、快輪到時推播' },
  ] },
  { id: 'aspnet', name: 'ASP.NET Core', group: 'api', usages: [
    { page: 'polls', what: '投票與選項 CRUD' },
    { page: 'dashboard', what: '報表 API' },
  ] },
  { id: 'efcore', name: 'EF Core', group: 'api', usages: [
    { page: 'songs', what: '曲庫 CRUD、CSV 匯入' },
  ] },
  { id: 'go', name: 'Go・goroutine', group: 'core', usages: [
    { page: 'polls', what: '投票寫入、WebSocket 推播' },
    { page: 'queues', what: '原子取號、狀態機' },
    { page: 'boards', what: '便利貼同步、衝突處理' },
  ] },
  { id: 'mysql', name: 'MySQL 8', group: 'data', usages: [
    { page: 'polls', what: '唯一索引擋重複投票' },
    { page: 'dashboard', what: '視窗函數報表' },
  ] },
  { id: 'redis', name: 'Redis', group: 'data', usages: [
    { page: 'polls', what: 'HINCRBY 即時計票' },
    { page: 'queues', what: 'INCR 原子取號' },
    { page: 'songs', what: 'Sorted Set 熱度排序' },
  ] },
  { id: 'claude', name: 'Claude API', group: 'ai', usages: [
    { page: 'dashboard', what: 'AI 洞察摘要' },
    { page: 'boards', what: '便利貼分群' },
    { page: 'songs', what: '自然語言找歌' },
  ] },
]
