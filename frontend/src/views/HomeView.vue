<script setup>
import { ref, onMounted, computed, reactive } from 'vue';
import { useRouter } from 'vue-router';
import TopBar from '@/components/TopBar.vue';
import Pin from '@/components/Pin.vue';

const pins = ref([]);
const loading = ref(true);
const apiUrl = import.meta.env.VITE_API_URL;
const router = useRouter();

// --- STAN FILTRÓW ---
const filters = reactive({
  search: '',
  instrument: 'all',
  genre: 'all'
});

// Mapowanie instrumentów (dostosuj numery do swojego backendu)
const instrumentMap = {
  0: 'Gitara',
  1: 'Bass',
  2: 'Perkusja'
};

// Pobieranie unikalnych gatunków z pobranych pinów
const availableGenres = computed(() => {
  const genres = new Set();
  pins.value.forEach(pin => {
    pin.pinGenres.forEach(pg => genres.add(pg.genre.name));
  });
  return Array.from(genres).sort();
});

// --- LOGIKA FILTROWANIA ---
const filteredPins = computed(() => {
  return pins.value.filter(pin => {
    const matchesSearch = pin.title.toLowerCase().includes(filters.search.toLowerCase());
    const matchesInstrument = filters.instrument === 'all' || pin.instrument.toString() === filters.instrument;
    const matchesGenre = filters.genre === 'all' || pin.pinGenres.some(pg => pg.genre.name === filters.genre);
    
    return matchesSearch && matchesInstrument && matchesGenre;
  });
});

const fetchPins = async () => {
  try {
    const token = sessionStorage.getItem('token');
    const response = await fetch(`${apiUrl}/api/Pins`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    
    if (response.status === 401) {
      router.push('/login');
      return;
    }
    if (!response.ok) throw new Error('Błąd połączenia');
    
    const data = await response.json();
    pins.value = data.map(pin => ({
      ...pin,
      filePath: `${apiUrl}/api/Pins/files/${pin.filename}`,
      pinGenres: pin.genres.map(g => ({ genre: { name: g } }))
    }));

  } catch (error) {
    console.error("Błąd:", error);
    loadMockData();
  } finally {
    loading.value = false;
  }
};

const loadMockData = () => {
  pins.value = [
    { id: "1", title: "Deep Purple - Smoke on the water", instrument: 0, filePath: "/sample.gp5", pinGenres: [{ genre: { name: "Rock" } }] },
    { id: "2", title: "Ocipieje", instrument: 1, filePath: "/heavy.gp5", pinGenres: [{ genre: { name: "Metal" } }] }
  ];
};

const handleSearch = (query) => {
  filters.search = query;
};

const resetFilters = () => {
  filters.instrument = 'all';
  filters.genre = 'all';
  filters.search = '';
};

onMounted(() => {
  if (apiUrl) fetchPins();
  else {
    loadMockData();
    loading.value = false;
  }
});
</script>

<template>
  <div class="app-wrapper">
    <TopBar @search="handleSearch" />
    
    <main class="main-content">
      <!-- PANEL FILTROWANIA -->
      <div class="filter-panel">
        <div class="filter-group">
          <label>Instrument:</label>
          <select v-model="filters.instrument">
            <option value="all">Wszystkie</option>
            <option v-for="(name, val) in instrumentMap" :key="val" :value="val">
              {{ name }}
            </option>
          </select>
        </div>

        <div class="filter-group">
          <label>Gatunek:</label>
          <select v-model="filters.genre">
            <option value="all">Wszystkie</option>
            <option v-for="genre in availableGenres" :key="genre" :value="genre">
              {{ genre }}
            </option>
          </select>
        </div>

        <button class="reset-btn" @click="resetFilters">Wyczyść</button>
        
        <div class="results-count">
          Znaleziono: {{ filteredPins.length }}
        </div>
      </div>

      <!-- Loader -->
      <div v-if="loading" class="loader">Ładowanie riffów...</div>

      <!-- Komunikat o braku wyników -->
      <div v-else-if="filteredPins.length === 0" class="no-results">
        Nie znaleziono pinów spełniających kryteria.
      </div>

      <!-- Siatka pinów - Używamy filteredPins zamiast pins -->
      <div v-else class="pin-grid">
        <Pin 
          v-for="item in filteredPins" 
          :key="item.id" 
          :pin="item" 
        />
      </div>
    </main>
  </div>
</template>

<style scoped>
.filter-panel {
  background: white;
  padding: 15px 20px;
  margin-bottom: 20px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  gap: 20px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  align-items: center;
  gap: 10px;
}

.filter-group label {
  font-weight: bold;
  font-size: 0.9rem;
  color: #444;
}

.filter-group select {
  padding: 8px 12px;
  border-radius: 6px;
  border: 1px solid #ddd;
  background-color: #fff;
  outline: none;
}

.reset-btn {
  padding: 8px 15px;
  background: #f0f0f0;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.2s;
}

.reset-btn:hover {
  background: #e0e0e0;
}

.results-count {
  margin-left: auto;
  color: #888;
  font-size: 0.9rem;
}

.loader, .no-results {
  text-align: center;
  padding: 50px;
  font-size: 1.2rem;
  color: #666;
}

.main-content {
  padding: 20px;
  max-width: 1400px;
  margin: 0 auto;
}

.pin-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 25px;
}

/* Responsywność dla małych ekranów */
@media (max-width: 600px) {
  .filter-panel {
    flex-direction: column;
    align-items: flex-start;
  }
  .results-count {
    margin-left: 0;
  }
}
</style>