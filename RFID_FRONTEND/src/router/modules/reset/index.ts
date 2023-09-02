import type { RouteRecordRaw } from 'vue-router';
import Layout from '@/layouts/index.vue';
const resetRoutes: RouteRecordRaw[] = [
    {
        path: '/reset',
        name: 'Reset',
        component: Layout,
        children: [
            {
                path: '/reset',
                name: 'Reset',
                component: () => import('@/views/reset/index.vue'),
            },
        ],
    },
];

export default resetRoutes;
