import { post } from '@/utils/http/axios';
import { URL, depositResponse, withdrawResponse, MoneyDataType } from './types';

const goDepositApi = (moneyData: MoneyDataType) => {
    return post<depositResponse>({
        url: URL.money,
        data: moneyData,
    });
};
const goWithdrawApi = (moneyData: MoneyDataType) => {
    return post<withdrawResponse>({
        url: URL.withdraw,
        data: moneyData,
    });
};

export { goDepositApi, goWithdrawApi };
