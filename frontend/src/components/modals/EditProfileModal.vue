<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-card">
      <div class="modal-header"><h3>Edytuj profil</h3></div>
      <div class="modal-body edit-form">
        <div class="form-group">
          <label>Nazwa użytkownika</label>
          <input v-model="form.displayName" type="text" />
        </div>
        <div class="form-group">
          <label>Email</label>
          <input v-model="form.email" type="email" />
        </div>
        <div class="form-group">
          <label>O sobie</label>
          <textarea v-model="form.bio" rows="3"></textarea>
        </div>
      </div>
      <div class="modal-footer">
        <button @click="$emit('close')" class="btn-modal-cancel">Anuluj</button>
        <button @click="$emit('save', form)" class="btn-modal-confirm save">Zapisz zmiany</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
const props = defineProps(['user']);
const emit = defineEmits(['close', 'save']);

const form = ref({
  displayName: props.user.displayName || props.user.DisplayName || '',
  email: props.user.email || props.user.Email || '',
  bio: props.user.bio || props.user.Bio || ''
});
</script>

<style scoped>
.modal-overlay { position: fixed; inset: 0; background: rgba(0, 0, 0, 0.6); display: flex; justify-content: center; align-items: center; z-index: 9999; }
.modal-card { background: white; padding: 30px; border-radius: 20px; width: 90%; max-width: 450px; }
.edit-form .form-group { display: flex; flex-direction: column; gap: 8px; margin-bottom: 15px; }
.edit-form input, .edit-form textarea { width: 100%; padding: 10px; border: 1px solid #e2e8f0; border-radius: 8px; box-sizing: border-box; }
.modal-footer { display: flex; gap: 12px; justify-content: flex-end; margin-top: 20px; }
.btn-modal-cancel { background: #f1f5f9; border: none; padding: 10px 20px; border-radius: 8px; cursor: pointer; }
.btn-modal-confirm.save { background: #2ecc71; color: white; border: none; padding: 10px 20px; border-radius: 8px; cursor: pointer; font-weight: 600; }
</style>