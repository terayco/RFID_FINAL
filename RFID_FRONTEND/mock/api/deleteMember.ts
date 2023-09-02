import { MockMethod } from 'vite-plugin-mock';
export default [
    {
        url: '/api/deleteMember', // 注意，这里只能是string格式
        method: 'post',
        response: () => {
            return {
                code: 0,
                status: 200,
                data: '',
                msg: '删除成功',
            };
        },
    },
] as MockMethod[]; // 定义数据格式
