import { post } from '@/utils/http/axios';
import { URL } from './types';

const initInfoApi = (memberInfo: any) => {
    return post({
        url: URL.init,
        data: memberInfo,
    });
};

export { initInfoApi };
