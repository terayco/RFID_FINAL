import { MockMethod } from 'vite-plugin-mock';
export default [
    {
        url: '/api/editMember', // 注意，这里只能是string格式
        method: 'post',
        response: () => {
            return {
                code: 0,
                status: 200,
                data: '',
                msg: '编辑成功',
            };
        },
    },
] as MockMethod[]; // 定义数据格式
