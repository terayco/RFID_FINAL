import { MockMethod } from 'vite-plugin-mock';
export default [
    {
        url: '/api/history', // 注意，这里只能是string格式
        method: 'get',
        response: () => {
            return {
                code: 0,
                status: 200,
                data: [
                    {
                        ID: '1',
                        member_name: 'TOm',
                        member_ID: '2020201',
                        operation_type: '1',
                        operation_money: '25',
                        operation_time: '2023-6-08 15:30:17',
                    },
                    {
                        ID: '1',
                        member_name: 'TOm',
                        member_ID: '2020201',
                        operation_type: '1',
                        operation_money: '29',
                        operation_time: '2023-6-08 15:30:17',
                    },
                    {
                        ID: '1',
                        member_name: 'TOm',
                        member_ID: '2020201',
                        operation_type: '0',
                        operation_money: '28',
                        operation_time: '2023-6-08 15:30:17',
                    },
                    {
                        ID: '1',
                        member_name: 'TOm',
                        member_ID: '2020201',
                        operation_type: '0',
                        operation_money: '26',
                        operation_time: '2023-6-08 15:30:17',
                    },
                    {
                        ID: '1',
                        member_name: 'TOm',
                        member_ID: '2020201',
                        operation_type: '0',
                        operation_money: '25',
                        operation_time: '2023-6-08 15:30:17',
                    },
                ],
                msg: '上传成功',
            };
        },
    },
] as MockMethod[]; // 定义数据格式
