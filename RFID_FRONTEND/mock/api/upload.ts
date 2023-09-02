import { MockMethod } from 'vite-plugin-mock';
export default [
    {
        url: '/api/upload', // 注意，这里只能是string格式
        method: 'post',
        response: () => {
            return {
                code: 0,
                status: 200,
                data: {
                    url: 'https://img-blog.csdnimg.cn/20210120165105959.png',
                    type: '1',
                },
                msg: '上传成功',
            };
        },
    },
] as MockMethod[]; // 定义数据格式
