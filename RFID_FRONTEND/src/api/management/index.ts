import { get, post } from '@/utils/http/axios';
import { URL, WorkerMan, History } from './types';

// 定义参数类型接口

const getManagementApi = () => {
    return get<WorkerMan[]>({
        url: URL.management,
    });
};
const getHistoryApi = () => {
    return get<History[]>({
        url: URL.history,
    });
};
const deleteMemberApi = (member_ID: string) => {
    return post({
        url: URL.deleteMember,
        data: {
            member_ID,
        },
    });
};
const addMemberApi = (memberInfo: any) => {
    return post({
        url: URL.addMember,
        data: memberInfo,
    });
};
const editMemberApi = (memberInfo: any) => {
    return post({
        url: URL.editMembers,
        data: memberInfo,
    });
};
export {
    getManagementApi,
    getHistoryApi,
    deleteMemberApi,
    editMemberApi,
    addMemberApi,
};
