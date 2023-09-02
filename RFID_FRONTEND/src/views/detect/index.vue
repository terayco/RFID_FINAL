<template>
    <div>
        <div class="detect-box">
            <el-row justify="center" align="middle">
                <el-col :sm="6" :lg="6" v-if="tmpState === '1'">
                    <el-result icon="success" title="读卡成功"> </el-result>
                </el-col>
                <el-col :sm="6" :lg="3" v-if="tmpState === '1'">
                    <div class="info-box">
                        <div class="info">卡主信息</div>
                        <div class="info">工号：{{ incomingMessages[1] }}</div>
                        <div class="info">姓名：{{ incomingMessages[2] }}</div>
                        <div class="info">性别：{{ incomingMessages[3] }}</div>
                        <div class="info">余额：{{ incomingMessages[4] }}</div>
                        <el-button @click="goResetCard">初始化IC卡</el-button>
                    </div>
                </el-col>
                <el-col :sm="12" :lg="12" v-if="tmpState === '2'">
                    <el-result
                        icon="error"
                        title="无权进入"
                        :sub-title="incomingMessages[1]"
                    >
                    </el-result>
                </el-col>
                <el-col :sm="12" :lg="12" v-if="tmpState === '0'">
                    <div class="RFID">
                        <IconFont
                            icon="ic:baseline-sensors"
                            class="RFID-icon"
                        />
                        请靠近您的RFID卡
                    </div>
                </el-col>
            </el-row>
        </div>
        <div class="operation-box" v-if="tmpState === '1'">
            <p>选择存入或消费的金额</p>
            <el-input-number
                v-model="payNum"
                :min="0.01"
                :precision="2"
                :max="60000.0"
            />
            <div style="margin-top: 15px">
                <el-button @click="goDeposit">充值</el-button>
                <el-button
                    @click="goWithdraw"
                    :disabled="incomingMessages[4] - payNum < 0"
                    >扣费</el-button
                >
            </div>
        </div>
    </div>
</template>
<script lang="ts" setup>
import { ref } from 'vue';
import { useWebSocket } from '@/hook/useWebSocket';
import { MoneyDataType } from '@/api/detect/types';
import { goDepositApi } from '@/api/detect';
import { useRoute, useRouter } from 'vue-router';
const router = useRouter();
const route = useRoute();
localStorage.setItem('currentPath', route.path);
let payNum = ref(0);
const wsServer = import.meta.env.VITE_APP_WS_URL;
const { sendMessage, incomingMessages } = useWebSocket(wsServer);
let tmpState = ref('1');
let moneyData: MoneyDataType = {
    member_ID: localStorage.getItem('member_ID')!,
    operation_money: payNum.value.toString(),
    operation_type: '0',
};
const goDeposit = async () => {
    console.log(payNum.value);
    moneyData.operation_type = '0';
    moneyData.operation_money = payNum.value.toString();
    sendMessage(moneyData.operation_type + ',' + payNum.value.toString());
    await goDepositApi(moneyData);
};
const goWithdraw = async () => {
    console.log(payNum.value);
    moneyData.operation_type = '1';
    moneyData.operation_money = payNum.value.toString();
    sendMessage(moneyData.operation_type + ',' + payNum.value.toString());
    await goDepositApi(moneyData);
};
const goResetCard = () => {
    router.push('/reset');
};
</script>
<style lang="less">
.info-box {
    display: flex;
    flex-direction: column;
    justify-content: space-evenly;
    align-items: center;
    .info {
        margin: 10px 0;
    }
}
.RFID {
    .RFID-icon {
        font-size: 100px;
    }
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
}
.detect-box {
    margin-top: 12%;
}
.operation-box {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
}
.el-result /deep/ {
    padding-top: 0;
}
</style>
