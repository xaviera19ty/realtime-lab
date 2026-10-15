// 瀏覽器的 /api/* 原樣轉送給 api（ASP.NET Core）。
// 對瀏覽器來說還是同一個網域：不用處理 CORS，httpOnly cookie 照常運作。
export default defineEventHandler((event) => {
  const { apiBase } = useRuntimeConfig(event)
  return proxyRequest(event, `${apiBase}${event.path}`)
})
