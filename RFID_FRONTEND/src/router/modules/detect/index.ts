import type { RouteRecordRaw } from 'vue-router';
import Layout from '@/layouts/index.vue';
const detectRoutes: RouteRecordRaw[] = [
    {
        path: '/detect',
        name: 'Detect',
        component: Layout,
        children: [
            {
                path: '/detect',
                name: 'Detect',
                component: () => import('@/views/detect/index.vue'),
            },
        ],
    },
];

export default detectRoutes;
