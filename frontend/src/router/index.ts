import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: 'home',
      path: '/',
      component: () => import('@/views/HomeView.vue'),
      meta: { requiresAuth: true }
    },
    {
      name: 'add-pin',
      path: '/add-pin',
      component: () => import('@/views/AddPinVIew.vue'),
      meta: { requiresAuth: true }
    },
    {
      name: 'login',
      path: '/login',
      component: () => import('@/views/LoginView.vue'),
    },
    {
      name: 'register',
      path: '/register',
      component: () => import('@/views/RegisterView.vue'),
    },
    {
      name: 'forgot-password',
      path: '/forgot-password',
      component: () => import('@/views/ForgotPasswordView.vue'),
    },
    {
      name: 'reset-password',
      path: '/reset-password',
      component: () => import('@/views/ResetPasswordView.vue'),
    },
    {
      name: 'profile',
      path: '/profile/:id',
      component: () => import('@/views/ProfileView.vue'),
      meta: { requiresAuth: true }
    },
  ],
})

router.beforeEach((to, from, next) => {
  const token = sessionStorage.getItem('token');
  
  if (to.meta.requiresAuth && !token) {
    next({ name: 'login' });
  } else if ((to.name === 'login' || to.name === 'register') && token) {
    next({ name: 'home' });
  } else {
    next();
  }
});

export default router

