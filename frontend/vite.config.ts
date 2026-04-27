import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'
import { alphaTab } from "@coderline/alphatab-vite";


export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
    alphaTab(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },

  optimizeDeps: {
    exclude: ['@coderline/alphatab']
  }
})
