import tailwindcss from '@tailwindcss/vite'

export default defineNuxtConfig({
  compatibilityDate: '2026-10-01',
  devtools: { enabled: true },
  modules: ['@pinia/nuxt'],
  css: ['~/assets/css/main.css'],
  vite: {
    plugins: [tailwindcss()],
  },
  runtimeConfig: {
    // 伺服器端才看得到；docker compose 用 NUXT_API_BASE=http://api:8080 覆蓋
    apiBase: 'http://localhost:5080',
    public: {
      // 互動點歌平台（獨立專案）的網址；留空時首頁與選單顯示「開發中」
      songAppUrl: '',
    },
  },
  app: {
    head: {
      htmlAttrs: { lang: 'zh-Hant-TW' },
      title: 'Realtime Lab',
      meta: [{ name: 'description', content: '即時投票、排隊叫號、協作白板與營運儀表板' }],
      // 在畫面渲染前套上深色 class，避免先閃一下淺色；沒選過就跟隨系統設定（見 composables/useTheme.js）
      script: [{
        innerHTML: "try{var t=localStorage.getItem('theme');if(t==='dark'||(!t&&matchMedia('(prefers-color-scheme: dark)').matches))document.documentElement.classList.add('dark')}catch(e){}",
      }],
    },
  },
})
