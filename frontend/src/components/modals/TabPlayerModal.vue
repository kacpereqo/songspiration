<template>
  <div class="modal-overlay api-modal" @click.self="$emit('close')">
    <div class="modal-card api-card">
      <div class="modal-header">
        <button @click="$emit('close')" class="close-api-btn">&times;</button>
      </div>
      <div class="modal-body api-viewport-custom">
        <div class="at-header-custom">
          <div class="at-action-wrapper">
            <button @click="handleAction" :class="['btn-action-custom', { 'is-playing': isPlaying }]">
              {{ isPlaying ? 'Stop' : 'Graj' }}
            </button>
          </div>
          <div class="at-track-info-custom">
            <h2 class="at-title-custom">{{ pin?.title }}</h2>
            <div class="at-tags-custom">
              <span class="at-tag-custom">{{ getInstrumentName(pin?.instrument) }}</span>
            </div>
          </div>
        </div>
        <div class="tab-viewport-centered">
          <div class="at-content-wrapper">
            <div ref="alphaTabContainer"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, nextTick } from 'vue';

const props = defineProps(['pin', 'apiUrl']);
const emit = defineEmits(['close']);

const alphaTabContainer = ref(null);
const api = ref(null);
const isPlaying = ref(false);
const isManualStop = ref(false);
let wasPlaying = false;

const getInstrumentName = (id) => ({ 0: 'Gitara', 1: 'Bas', 2: 'Perkusja' }[id] || 'Instrument');

const loadAlphaTabScript = () => {
  return new Promise((resolve) => {
    if (window.alphaTab) return resolve();
    const script = document.createElement('script');
    script.src = 'https://cdn.jsdelivr.net/npm/@coderline/alphatab@latest/dist/alphaTab.min.js';
    script.async = true;
    script.onload = resolve;
    document.head.appendChild(script);
  });
};

const initAlphaTab = async () => {
  await loadAlphaTabScript();
  await nextTick();
  if (!props.pin.filePath) return;

  api.value = new window.alphaTab.AlphaTabApi(alphaTabContainer.value, {
    file: `${props.apiUrl}${props.pin.filePath}`,
    player: {
      enablePlayer: true,
      soundFont: 'https://cdn.jsdelivr.net/npm/@coderline/alphatab@latest/dist/soundfont/sonivox.sf2',
      enableCursor: true,
    }
  });

  api.value.playerStateChanged.on((args) => {
    const currentIsPlaying = args.state === 1;
    if (args.state === 0 && wasPlaying && !isManualStop.value) {
      setTimeout(() => api.value?.play(), 50);
    }
    isPlaying.value = currentIsPlaying;
    wasPlaying = currentIsPlaying;
  });
};

const handleAction = () => {
  if (isPlaying.value) {
    isManualStop.value = true;
    api.value?.stop();
  } else {
    isManualStop.value = false;
    api.value?.play();
  }
};

onMounted(initAlphaTab);
onUnmounted(() => api.value?.destroy());
</script>

<style scoped>
.api-card { max-width: 1100px; width: 95%; height: 85vh; background: white; border-radius: 20px; overflow: hidden; display: flex; flex-direction: column; position: relative; }
.api-viewport-custom { flex: 1; display: flex; flex-direction: column; padding: 25px; overflow: hidden; }
.at-header-custom { display: flex; align-items: center; gap: 20px; padding-bottom: 20px; border-bottom: 1px solid #f0f3f6; }
.btn-action-custom { padding: 12px 35px; border-radius: 50px; border: none; background: #2ecc71; color: white; font-weight: 700; cursor: pointer; }
.btn-action-custom.is-playing { background: #e74c3c; }
.tab-viewport-centered { flex: 1; overflow: auto; margin-top: 20px; border: 1px solid #f0f0f0; border-radius: 8px; padding: 20px; }
.close-api-btn { position: absolute; top: 15px; right: 15px; border: none; background: #f1f5f9; border-radius: 50%; width: 36px; height: 36px; cursor: pointer; z-index: 10; font-size: 20px; }
</style>