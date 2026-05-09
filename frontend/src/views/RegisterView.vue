<template>
  <div class="register-container">
    <div class="register-card">
      <h1>Rejestracja</h1>
      <form @submit.prevent="handleRegister" class="register-form">
        <div class="form-group">
          <label for="email">Email</label>
          <input v-model="form.email" type="email" id="email" required placeholder="Wpisz email..." />
        </div>
        <div class="form-group">
          <label for="displayName">Nazwa użytkownika</label>
          <input v-model="form.displayName" type="text" id="displayName" required placeholder="Wpisz nazwę..." />
        </div>
        <div class="form-group">
          <label for="password">Hasło</label>
          <input v-model="form.password" type="password" id="password" required placeholder="Wpisz hasło..." />
        </div>
        <div v-if="errorMessage" class="error-msg">{{ errorMessage }}</div>
        <button type="submit" :disabled="isLoading" class="btn-submit">
          {{ isLoading ? 'Rejestracja...' : 'Zarejestruj się' }}
        </button>
      </form>
      <div class="login-link">
        Masz już konto? <RouterLink to="/login">Zaloguj się</RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const apiUrl = import.meta.env.VITE_API_URL;

const form = ref({ email: '', displayName: '', password: '' });
const errorMessage = ref('');
const isLoading = ref(false);

const handleRegister = async () => {
  isLoading.value = true;
  errorMessage.value = '';

  try {
    const response = await fetch(`${apiUrl}/api/Users/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form.value)
    });

    if (response.ok) {
      const data = await response.json();
      // Po poprawnej rejestracji backend zwraca token (w AuthResponseDto), więc możemy od razu zalogować użytkownika
      sessionStorage.setItem('token', data.accessToken);
      
      // Zapisujemy nazwę użytkownika, by wyświetlić inicjały
      if (data.user && data.user.displayName) {
        sessionStorage.setItem('userName', data.user.displayName);
      } else if (data.user && data.user.email) {
        sessionStorage.setItem('userName', data.user.email);
      }

      // Przekierowanie na feed
      router.push('/');
    } else {
      const errorData = await response.json();
      errorMessage.value = errorData.message || 'Błąd podczas rejestracji. Sprawdź poprawność danych.';
    }
  } catch (error) {
    console.error('Błąd rejestracji:', error);
    errorMessage.value = 'Błąd połączenia z serwerem.';
  } finally {
    isLoading.value = false;
  }
};
</script>

<style scoped>
.register-container { display: flex; justify-content: center; align-items: center; height: 100vh; background-color: #f4f7f9; }
.register-card { background: white; padding: 40px; border-radius: 12px; box-shadow: 0 4px 20px rgba(0,0,0,0.1); width: 100%; max-width: 400px; text-align: center; }
h1 { margin-top: 0; margin-bottom: 25px; color: #24292e; }
.register-form { display: flex; flex-direction: column; gap: 20px; text-align: left; }
.form-group { display: flex; flex-direction: column; gap: 8px; }
label { font-weight: 600; color: #444; font-size: 14px; }
input { padding: 10px; border: 1px solid #ddd; border-radius: 6px; font-size: 16px; }
.btn-submit { background: #2ecc71; color: white; border: none; padding: 12px; border-radius: 6px; font-weight: bold; cursor: pointer; font-size: 16px; margin-top: 10px; }
.btn-submit:disabled { background: #95a5a6; cursor: not-allowed; }
.error-msg { color: #e74c3c; font-size: 14px; text-align: center; }
.login-link { margin-top: 20px; font-size: 14px; color: #666; }
.login-link a { color: #2ecc71; text-decoration: none; font-weight: bold; }
.login-link a:hover { text-decoration: underline; }
</style>
