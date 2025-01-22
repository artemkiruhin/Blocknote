import { createRouter, createWebHistory } from 'vue-router';
import AuthPage from '@/components/AuthPage.vue';
import Notepage from '@/components/Notepage.vue';
import {getToken} from "./models/cookie-manager.js";

const routes = [
    {
        path: '/',
        name: 'Auth',
        component: AuthPage,
        meta: { requiresAuth: false },
    },
    {
        path: '/notes',
        name: 'Notes',
        component: Notepage,
        meta: { requiresAuth: true },
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to, from, next) => {
    const isAuthenticated = !!getToken();
    if (to.meta.requiresAuth && !isAuthenticated) {
        next({ name: 'Auth' });
    } else {
        next();
    }
});

export default router;