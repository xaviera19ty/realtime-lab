<script setup>
import { pages } from '~/data/skills'

const menuOpen = ref(false)
const { songAppUrl } = useRuntimeConfig().public
</script>

<template>
  <div class="min-h-screen md:flex">
    <header class="flex items-center justify-between border-b border-line bg-card px-4 py-3 md:hidden">
      <NuxtLink to="/" class="flex items-center gap-2 font-semibold text-accent">
        <span class="size-3 rounded-sm bg-brand-500" aria-hidden="true" />
        Realtime Lab
      </NuxtLink>
      <div class="flex items-center gap-2">
        <ThemeToggle />
        <button
          class="rounded-md px-3 py-1.5 text-sm text-accent ring-1 ring-line hover:bg-hover"
          :aria-expanded="menuOpen"
          @click="menuOpen = !menuOpen"
        >
          選單
        </button>
      </div>
    </header>

    <nav
      :class="menuOpen ? 'flex' : 'hidden'"
      class="flex-col border-b border-line bg-card md:sticky md:top-0 md:flex md:h-screen md:w-56 md:shrink-0 md:border-r md:border-b-0"
    >
      <NuxtLink to="/" class="hidden items-center gap-2 px-5 py-6 font-semibold text-accent md:flex">
        <span class="size-3 rounded-sm bg-brand-500" aria-hidden="true" />
        Realtime Lab
      </NuxtLink>
      <ul class="space-y-1 p-3 md:pt-0">
        <li>
          <NuxtLink
            to="/"
            class="block rounded-md px-3 py-2 hover:bg-hover"
            exact-active-class="bg-brand-500 font-medium text-white hover:bg-brand-500"
          >
            首頁
          </NuxtLink>
        </li>
        <li v-for="(page, key) in pages" :key="key">
          <a
            v-if="page.status === 'external' && songAppUrl"
            :href="songAppUrl"
            target="_blank"
            rel="noopener"
            class="flex items-center justify-between rounded-md px-3 py-2 hover:bg-hover"
          >
            {{ page.name }}
            <span class="text-xs text-muted">獨立專案 ↗</span>
          </a>
          <NuxtLink
            v-else-if="page.status === 'done'"
            :to="page.path"
            class="block rounded-md px-3 py-2 hover:bg-hover"
            active-class="bg-brand-500 font-medium text-white hover:bg-brand-500"
          >
            {{ page.name }}
          </NuxtLink>
          <span v-else class="flex items-center justify-between px-3 py-2 text-brand-400">
            {{ page.name }}
            <span class="rounded bg-cream px-1.5 py-0.5 text-xs text-brand-600">
              {{ page.status === 'external' ? '獨立專案・開發中' : '開發中' }}
            </span>
          </span>
        </li>
      </ul>
      <div class="mt-auto hidden p-4 md:block">
        <ThemeToggle />
      </div>
    </nav>

    <main class="flex-1 p-4 md:p-8">
      <slot />
    </main>
  </div>
</template>
