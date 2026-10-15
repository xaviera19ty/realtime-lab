// 呼叫網頁後端的 /api/*。SSR 時在 Nuxt 伺服器上執行，要把使用者的 cookie 一起帶過去。
export function useApi(path, options = {}) {
  const headers = import.meta.server ? useRequestHeaders(['cookie']) : {}
  return useFetch(path, { ...options, headers: { ...headers, ...options.headers } })
}
