<template>
  <div class="users-view">
    <h2>Manage Users</h2>
    <div class="search-box">
      <input type="text" v-model="searchTerm" placeholder="Search users..." />
    </div>
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Username</th>
          <th>Email</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in filteredUsers" :key="user.id">
          <td>{{ user.id }}</td>
          <td>{{ user.username }}</td>
          <td>{{ user.email }}</td>
          <td>
            <button @click="deleteUser(user.id)">Delete</button>
            <button @click="banUser(user.id)" v-if="!user.isBanned">Ban</button>
            <button @click="deleteUserPins(user.id)">Delete Pins</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';

interface User {
  id: number;
  username: string;
  email: string;
  isBanned: boolean;
}

export default {
  name: 'UsersView',
  setup() {
    const users = ref<User[]>([]);
    const searchTerm = ref('');

    const fetchUsers = async () => {
      try {
        const response = await axios.get('/api/admin/users', {
          params: { criteria: searchTerm.value }
        });
        users.value = response.data;
      } catch (error) {
        console.error('Error fetching users:', error);
      }
    };

    const deleteUser = async (id: number) => {
      try {
        await axios.delete(`/api/admin/users/${id}`);
        await fetchUsers();
      } catch (error) {
        console.error('Error deleting user:', error);
      }
    };

    const banUser = async (id: number) => {
      try {
        await axios.post(`/api/admin/users/${id}/ban`);
        await fetchUsers();
      } catch (error) {
        console.error('Error banning user:', error);
      }
    };

    const deleteUserPins = async (id: number) => {
      try {
        await axios.delete(`/api/admin/users/${id}/pins`);
        alert('User pins deleted successfully');
      } catch (error) {
        console.error('Error deleting user pins:', error);
      }
    };

    const filteredUsers = computed(() => {
      return users.value.filter(user =>
        user.username.toLowerCase().includes(searchTerm.value.toLowerCase()) ||
        user.email.toLowerCase().includes(searchTerm.value.toLowerCase())
      );
    });

    onMounted(fetchUsers);

    return {
      users,
      searchTerm,
      filteredUsers,
      deleteUser,
      banUser,
      deleteUserPins,
    };
  },
};
</script>

<style scoped>
.users-view {
  padding: 20px;
}

.search-box {
  margin-bottom: 20px;
}

.search-box input {
  padding: 8px;
  width: 300px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  padding: 10px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background-color: #f2f2f2;
}

button {
  padding: 5px 10px;
  margin-right: 5px;
  background-color: #dc3545;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

button:hover {
  background-color: #c82333;
}
</style>