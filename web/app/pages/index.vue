<script setup>
import { groups, pages, skills } from '~/data/skills'

// 技能分組的識別色，取自配色表的搭配色
const groupDot = {
  frontend: 'bg-berry',
  api: 'bg-sky',
  core: 'bg-brand-500',
  data: 'bg-gold',
  ai: 'bg-rose',
}

const activeSkill = ref(null)

const skillsByGroup = computed(() =>
  groups.map((g) => ({ ...g, skills: skills.filter((s) => s.group === g.id) })),
)

function isUsedBy(pageKey) {
  return !activeSkill.value || activeSkill.value.usages.some((u) => u.page === pageKey)
}

function toggle(skill) {
  activeSkill.value = activeSkill.value?.id === skill.id ? null : skill
}

// 骨架階段用來確認 web → api → core 整條路都通
const { data: status, error: statusError } = await useApi('/api/system/status')
</script>

<template>
  <div class="mx-auto max-w-5xl space-y-10">
    <section class="rounded-2xl bg-gradient-to-br from-brand-200 via-blush to-brand-100 p-6 dark:from-brand-800 dark:via-brand-900 dark:to-card md:p-10">
      <h1 class="text-2xl font-semibold text-heading md:text-3xl">嗨，我是＿＿＿</h1>
      <p class="mt-2 text-muted">Vue 前端工程師，跨足 C# 與 Go 後端。喜歡唱歌與直播。</p>
    </section>

    <section>
      <h2 class="text-lg font-semibold text-heading">技能地圖</h2>
      <p class="mt-1 text-sm text-muted">點一個技能，看它用在哪些頁面。</p>
      <div class="mt-4 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        <div v-for="group in skillsByGroup" :key="group.id">
          <h3 class="flex items-center gap-2 text-sm font-medium text-muted">
            <span class="size-2 rounded-full" :class="groupDot[group.id]" aria-hidden="true" />
            {{ group.name }}
          </h3>
          <div class="mt-2 flex flex-wrap gap-2">
            <button
              v-for="skill in group.skills"
              :key="skill.id"
              :aria-pressed="activeSkill?.id === skill.id"
              class="rounded-full px-3 py-1 text-sm ring-1 transition"
              :class="activeSkill?.id === skill.id
                ? 'bg-brand-500 text-white ring-brand-500 shadow-sm shadow-brand-300 dark:shadow-none'
                : 'bg-card text-ink ring-line hover:bg-hover hover:ring-line-strong'"
              @click="toggle(skill)"
            >
              {{ skill.name }}
            </button>
          </div>
        </div>
      </div>
      <ul
        v-if="activeSkill"
        class="mt-5 space-y-1 rounded-xl border-l-4 border-brand-500 bg-card p-4 text-sm ring-1 ring-line"
      >
        <li v-for="usage in activeSkill.usages" :key="usage.page">
          <span class="font-medium text-accent">{{ pages[usage.page].name }}</span>：{{ usage.what }}
        </li>
      </ul>
    </section>

    <section>
      <h2 class="text-lg font-semibold text-heading">功能</h2>
      <div class="mt-4 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        <div
          v-for="(page, key) in pages"
          :key="key"
          class="rounded-xl bg-card p-4 ring-1 transition"
          :class="isUsedBy(key)
            ? (activeSkill ? 'ring-2 ring-brand-400' : 'ring-line hover:ring-line-strong')
            : 'opacity-40 ring-line'"
        >
          <div class="flex items-center justify-between">
            <span class="font-medium text-heading">{{ page.name }}</span>
            <span
              class="rounded-full px-2 py-0.5 text-xs"
              :class="page.status === 'done' ? 'bg-mint text-brand-800' : 'bg-cream text-brand-600'"
            >
              {{ page.status === 'done' ? '已完成' : '開發中' }}
            </span>
          </div>
        </div>
      </div>
    </section>

    <section class="rounded-xl bg-card p-4 text-sm ring-1 ring-line">
      <h2 class="font-semibold text-heading">系統狀態</h2>
      <p v-if="statusError" class="mt-2 font-medium text-danger">連不到網頁後端（api）</p>
      <dl v-else-if="status" class="mt-2 grid grid-cols-[auto_1fr] gap-x-4 gap-y-1">
        <dt class="text-muted">api</dt>
        <dd>{{ status.api }}</dd>
        <dt class="text-muted">core</dt>
        <dd>{{ status.core.ok ? 'ok' : status.core.error }}</dd>
        <dt class="text-muted">request id</dt>
        <dd class="font-mono">{{ status.requestId }}</dd>
      </dl>
    </section>
  </div>
</template>
