import { MockMethod } from 'vite-plugin-mock';
export default [
    {
        url: '/api/management', // 注意，这里只能是string格式
        method: 'get',
        response: () => {
            return {
                code: 0,
                status: 200,
                data: [
                    {
                        member_ID: '1',
                        member_name: 'Tom',
                        sex: '男',
                        age: '25',
                        balance: '100',
                        card_status: '0',
                    },
                    {
                        member_ID: '2',
                        member_name: 'Tom',
                        sex: '男',
                        age: '25',
                        balance: '101',
                        card_status: '1',
                    },
                    {
                        member_ID: '1',
                        member_name: 'Tom',
                        sex: '男',
                        age: '25',
                        balance: '99',
                        card_status: '2',
                    },
                    {
                        member_ID: '1',
                        member_name: 'Tom',
                        sex: '男',
                        age: '25',
                        balance: '100',
                        card_status: '0',
                    },
                    {
                        member_ID: '1',
                        member_name: 'Tom',
                        sex: '男',
                        age: '25',
                        balance: '200',
                        card_status: '0',
                    },
                    {
                        member_ID: '1',
                        member_name: 'Tom',
                        sex: '男',
                        age: '25',
                        balance: '100',
                        card_status: '0',
                    },
                    {
                        member_ID: '1',
                        member_name: 'Tom',
                        sex: '男',
                        age: '25',
                        balance: '100',
                        card_status: '0',
                    },
                ],
                msg: '上传成功',
            };
        },
    },
] as MockMethod[]; // 定义数据格式
