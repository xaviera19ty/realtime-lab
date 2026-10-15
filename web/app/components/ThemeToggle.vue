<script setup>
const { isDark, toggle } = useTheme()
</script>

<template>
  <button
    type="button"
    class="inline-flex size-9 items-center justify-center rounded-md text-accent ring-1 ring-line transition hover:bg-hover"
    :aria-label="isDark ? '切換為淺色' : '切換為深色'"
    :title="isDark ? '切換為淺色' : '切換為深色'"
    @click="toggle"
  >
    <!-- 深色時顯示太陽（按下回淺色），淺色時顯示月亮 -->
    <svg v-if="isDark" class="size-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" aria-hidden="true">
      <circle cx="12" cy="12" r="4" />
      <path d="M12 2v2M12 20v2M4.93 4.93l1.41 1.41M17.66 17.66l1.41 1.41M2 12h2M20 12h2M4.93 19.07l1.41-1.41M17.66 6.34l1.41-1.41" />
    </svg>
    <svg v-else class="size-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">
      <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z" />
    </svg>
  </button>
</template>
