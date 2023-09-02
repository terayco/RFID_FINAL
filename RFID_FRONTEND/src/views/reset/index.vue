<script setup lang="ts">
import { reactive, ref, watch } from 'vue';
import { ElMessage, FormInstance, FormRules } from 'element-plus';
import { addMemberApi } from '@/api/management';
import { useWebSocket } from '@/hook/useWebSocket';
import { useRouter, useRoute } from 'vue-router';
import { initInfoApi } from '@/api/reset';
const router = useRouter();
const route = useRoute();
localStorage.setItem('currentPath', route.path);
const wsServer = import.meta.env.VITE_APP_WS_URL;
const { sendMessage, back } = useWebSocket(wsServer);
const addMemberInfo = reactive<RuleForm>({
    member_ID: '',
    member_name: '',
    sex: '男',
    age: 18,
    balance: 0,
    card_status: '正常',
});
interface RuleForm {
    member_ID: string;
    member_name: string;
    age: number;
    sex: string;
    balance: number;
    card_status: string;
}
const ruleFormRef = ref<FormInstance>();
const validateInput = (value: string) => {
    const pattern = /[^\u4e00-\u9fa5]/; // 使用正则表达式匹配非中文字符
    if (!pattern.test(value)) {
        ElMessage.error('工号不能包含中文字符，请修改!');
        addMemberInfo.member_ID = '';
    }
};

const handleBlur = () => {
    validateInput(addMemberInfo.member_ID);
};

const rules = reactive<FormRules<RuleForm>>({
    member_ID: [
        {
            required: true,
            message: '请输入员工号',
        },
        {
            type: 'string',
        },
    ],
    sex: [
        {
            required: true,
            message: '请选择性别',
            trigger: 'change',
        },
    ],
    balance: [
        {
            required: true,
            message: '请输入余额',
        },
        { type: 'number', min: 0.01 },
    ],
    member_name: [
        {
            type: 'string',
            required: true,
            message: '请输入员工姓名',
            trigger: 'blur',
        },
    ],
    age: [
        {
            required: true,
            message: '请输入年龄',
        },
        { type: 'number', min: 18 },
    ],
});

const sexOptions = [
    {
        value: '男',
        label: '男',
    },
    {
        value: '女',
        label: '女',
    },
];
watch(back, (newVal) => {
    if (newVal) {
        router.push('/detect');
    }
});

const confirmAdd = async () => {
    addMemberInfo.card_status =
        addMemberInfo.card_status === '正常'
            ? '0'
            : addMemberInfo.card_status === '挂失'
            ? '1'
            : '2';
    let obj = {
        member_ID: addMemberInfo.member_ID,
        member_name: addMemberInfo.member_name,
        age: addMemberInfo.age.toString(),
        sex: addMemberInfo.sex,
        balance: addMemberInfo.balance.toString(),
        card_status: addMemberInfo.card_status,
    };
    sendMessage(
        '2' +
            ',' +
            obj.member_ID +
            ',' +
            obj.member_name +
            ',' +
            obj.sex +
            ',' +
            obj.balance +
            ',' +
            obj.card_status
    );
    await initInfoApi(obj);
    await router.push('/detect');
};
</script>

<template>
    <div class="reset-form">
        <el-form
            ref="ruleFormRef"
            :model="addMemberInfo"
            :rules="rules"
            label-width="120px"
            :size="'default'"
            status-icon
        >
            <el-form-item label="工号" prop="member_ID">
                <el-input
                    label="工号"
                    v-model="addMemberInfo.member_ID"
                    ref="formRef"
                    placeholder="工号"
                    @blur="handleBlur"
                    clearable
                />
            </el-form-item>
            <el-form-item label="姓名" prop="member_name">
                <el-input
                    v-model="addMemberInfo.member_name"
                    placeholder="姓名"
                    clearable
                />
            </el-form-item>
            <el-form-item label="性别" prop="sex">
                <el-select v-model="addMemberInfo.sex" placeholder="性别">
                    <el-option
                        v-for="item in sexOptions"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value"
                    />
                </el-select>
            </el-form-item>
            <el-form-item label="年龄" prop="age">
                <el-input-number
                    v-model="addMemberInfo.age"
                    :min="18"
                    :max="65"
                    :precision="0"
                />
            </el-form-item>
            <el-form-item label="余额" prop="balance">
                <el-input-number
                    v-model="addMemberInfo.balance"
                    :min="0.01"
                    :precision="2"
                />
            </el-form-item>
            <el-form-item label="状态" prop="card_status">
                <el-radio-group v-model="addMemberInfo.card_status">
                    <el-radio-button label="正常" />
                    <el-radio-button label="挂失" />
                    <el-radio-button label="黑卡" />
                </el-radio-group>
            </el-form-item>
        </el-form>
        <el-button type="primary" @click="confirmAdd"> 确认 </el-button>
    </div>
</template>

<style scoped lang="less">
.el-input /deep/ {
    width: 250px;
}
.reset-form {
    margin-top: 10%;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
}
</style>
