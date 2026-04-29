<template>
  <div class="add-pin-container">
    <div class="form-card">
      <h1>Dodaj nową tabulaturę</h1>
      
      <form @submit.prevent="handleSubmit" class="pin-form">
        <!-- Tytuł -->
        <div class="form-group">
          <label for="title">Tytuł utworu</label>
          <input v-model="form.title" type="text" id="title" required placeholder="np. Deep Purple - Smoke on the water">
        </div>

        <!-- Opis -->
        <div class="form-group">
          <label for="description">Opis (opcjonalnie)</label>
          <textarea v-model="form.description" id="description" rows="3"></textarea>
        </div>

        <div class="form-row">
          <!-- Instrument -->
          <div class="form-group">
            <label for="instrument">Instrument</label>
            <select v-model.number="form.instrument" id="instrument">
              <option :value="0">Gitara</option>
              <option :value="1">Bas</option>
              <option :value="2">Perkusja</option>
            </select>
          </div>

          <!-- Widoczność -->
          <div class="form-group">
            <label for="visibility">Widoczność</label>
            <select v-model.number="form.visibility" id="visibility">
              <option :value="0">Publiczny</option>
              <option :value="1">Prywatny</option>
            </select>
          </div>
        </div>

        <!-- Plik (Guitar Pro) -->
        <div class="form-group">
          <label for="file">Plik tabulatury (.gp3, .gp4, .gp5, .gpx)</label>
          <input type="file" id="file" @change="handleFileChange" accept=".gp3,.gp4,.gp5,.gpx,.gp" required class="file-input">
        </div>

        <!-- Gatunki (Uproszczone jako multi-select) -->
        <div class="form-group">
          <label>Gatunki</label>
          <div class="genre-selection">
            <label v-for="genre in availableGenres" :key="genre.id" class="checkbox-label">
              <input type="checkbox" :value="genre.id" v-model="form.genreIds">
              {{ genre.name }}
            </label>
          </div>
        </div>

        <div class="form-actions">
          <button type="button" @click="$router.push('/')" class="btn-cancel">Anuluj</button>
          <button type="submit" :disabled="isSubmitting" class="btn-submit">
            {{ isSubmitting ? 'Wysyłanie...' : 'Dodaj Pin' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const apiUrl = import.meta.env.VITE_API_URL;

const isSubmitting = ref(false);
const selectedFile = ref(null);

// Przykładowe gatunki (w realnej apce pobierane z API)
const availableGenres = [
  { id: 'genre-1', name: 'Rock' },
  { id: 'genre-2', name: 'Metal' },
  { id: 'genre-3', name: 'Jazz' },
  { id: 'genre-4', name: 'Blues' }
];

const form = ref({
  title: '',
  description: '',
  instrument: 0,
  visibility: 0,
  genreIds: []
});

const handleFileChange = (event) => {
  selectedFile.value = event.target.files[0];
};

const handleSubmit = async () => {
  if (!selectedFile.value) return alert("Wybierz plik!");

  isSubmitting.value = true;
  
  // Używamy FormData, aby wysłać plik i pola tekstowe razem
  const formData = new FormData();
  formData.append('Title', form.value.title);
  formData.append('Description', form.value.description);
  formData.append('Instrument', form.value.instrument);
  formData.append('Visibility', form.value.visibility);
  formData.append('File', selectedFile.value); // Plik binarny
  
  // Przesyłanie tablicy ID gatunków (zależy jak Twój kontroler przyjmuje dane)
  form.value.genreIds.forEach(id => {
    formData.append('GenreIds', id);
  });

  try {
    const response = await fetch(`${apiUrl}/api/pins`, {
      method: 'POST',
      body: formData,
      // Nie ustawiamy Content-Type! Przeglądarka sama wstawi multipart/form-data z boundary
    });

    if (response.ok) {
      alert("Pin dodany pomyślnie!");
      router.push('/');
    } else {
      const errorData = await response.json();
      alert("Błąd: " + JSON.stringify(errorData));
    }
  } catch (error) {
    console.error("Błąd połączenia:", error);
    alert("Błąd połączenia z serwerem.");
  } finally {
    isSubmitting.value = false;
  }

  console.log("Dane do wysłania:", {
    ...form.value,
    file: selectedFile.value ? selectedFile.value.name : null
  });
};
</script>

<style scoped>
.add-pin-container {
  display: flex;
  justify-content: center;
  padding: 40px 20px;
}

.form-card {
  background: white;
  padding: 30px;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.1);
  width: 100%;
  max-width: 600px;
}

h1 { margin-bottom: 25px; color: #24292e; font-size: 24px; }

.form-group { margin-bottom: 20px; display: flex; flex-direction: column; gap: 8px; }

.form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 20px; }

label { font-weight: 600; color: #444; font-size: 14px; }

input[type="text"], textarea, select {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 16px;
}

.genre-selection {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  background: #f9f9f9;
  padding: 15px;
  border-radius: 8px;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 14px;
  background: white;
  padding: 5px 10px;
  border: 1px solid #eee;
  border-radius: 20px;
  cursor: pointer;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 15px;
  margin-top: 30px;
}

.btn-submit {
  background: #2ecc71;
  color: white;
  border: none;
  padding: 12px 25px;
  border-radius: 6px;
  font-weight: bold;
  cursor: pointer;
}

.btn-submit:disabled { background: #95a5a6; }

.btn-cancel {
  background: #eee;
  border: none;
  padding: 12px 25px;
  border-radius: 6px;
  cursor: pointer;
}
</style>