// 深淺色切換。實際狀態是 <html class="dark">，選擇存在 localStorage。
// 首屏由 nuxt.config.js 的 head script 先套上 class，避免先閃一下淺色。

const STORAGE_KEY = 'theme'

export function useTheme() {
  // SSR 不知道使用者的選擇，一律先當淺色，掛載後再同步實際狀態
  const isDark = useState('theme-dark', () => false)

  onMounted(() => {
    isDark.value = document.documentElement.classList.contains('dark')
  })

  function toggle() {
    isDark.value = !isDark.value
    document.documentElement.classList.toggle('dark', isDark.value)
    try {
      localStorage.setItem(STORAGE_KEY, isDark.value ? 'dark' : 'light')
    } catch {
      // 無痕模式等情況存不了，只影響下次開啟
    }
  }

  return { isDark, toggle }
}
