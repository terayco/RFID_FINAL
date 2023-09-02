export enum URL {
    money = '/money',
    withdraw = '/withdraw',
}

export interface depositResponse {
    data: string;
}
export interface withdrawResponse {
    data: string;
}
export interface MoneyDataType {
    member_ID: string;
    operation_money: string;
    operation_type: string;
}
