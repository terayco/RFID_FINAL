import type { RouteRecordRaw } from 'vue-router';
import Layout from '@/layouts/index.vue';
const managementRoutes: RouteRecordRaw[] = [
    {
        path: '/management',
        name: 'Management',
        component: Layout,
        children: [
            {
                path: '/management',
                name: 'Management',
                component: () => import('@/views/management/index.vue'),
            },
        ],
    },
];

export default managementRoutes;
