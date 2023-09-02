export enum URL {
    management = '/memberInfo',
    history = '/recordInfo',
    addMember = '/insert',
    deleteMember = '/delete',
    editMembers = '/edit',
}

export interface WorkerMan {
    ID: number;
    member_ID: string;
    member_name: string;
    sex: string;
    age: string;
    balance: string;
    card_status: string;
}

export interface History {
    ID: string;
    member_name: string;
    member_ID: string;
    operation_type: string;
    operation_money: string;
    operation_time: string;
}
