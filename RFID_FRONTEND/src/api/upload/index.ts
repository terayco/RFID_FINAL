import { post } from '@/utils/http/axios';
import { uploadRes, URL } from './types';

const uploadApi = (formData: FormData) => {
    return post<uploadRes>({
        url: URL.uplaod,
        data: formData,
        headers: {
            'Content-Type': 'multipart/form-data',
        },
        transformRequest: [
            function (data) {
                return data;
            },
        ],
    });
};

export { uploadApi };
