<template>
    <div>
        <div class="sub-title">员工信息</div>
        <el-table :data="tableData" max-height="280px" style="width: 100%">
            <el-table-column label="工号">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span v-show="!scope.row.isEdit">{{
                            scope.row.member_ID
                        }}</span>
                        <el-input
                            v-show="scope.row.isEdit"
                            v-model="editMember.member_ID"
                            placeholder="工号"
                            @blur="handleBlurT"
                            clearable
                        />
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="姓名">
                <template #default="scope">
                    <span v-show="!scope.row.isEdit">{{
                        scope.row.member_name
                    }}</span>
                    <el-input
                        v-show="scope.row.isEdit"
                        v-model="editMember.member_name"
                        placeholder="姓名"
                        clearable
                    />
                </template>
            </el-table-column>
            <el-table-column label="性别">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span v-show="!scope.row.isEdit">{{
                            scope.row.sex
                        }}</span>
                        <el-select
                            v-show="scope.row.isEdit"
                            v-model="editMember.sex"
                            placeholder="性别"
                        >
                            <el-option
                                v-for="item in sexOptions"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value"
                            />
                        </el-select>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="年龄">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span v-show="!scope.row.isEdit">{{
                            scope.row.age
                        }}</span>
                        <el-input-number
                            v-show="scope.row.isEdit"
                            v-model="editMember.age"
                            :min="18"
                            :max="65"
                        />
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="钱包余额(￥)" prop="balance">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span v-show="!scope.row.isEdit">{{
                            scope.row.balance
                        }}</span>
                        <el-input-number
                            v-show="scope.row.isEdit"
                            v-model="editMember.balance"
                            :min="0.01"
                            :precision="2"
                        />
                    </div>
                </template>
            </el-table-column>
            <el-table-column
                label="卡号状态"
                :filters="[
                    { text: '正常', value: '0' },
                    { text: '挂失', value: '1' },
                    { text: '黑卡', value: '2' },
                ]"
                :filter-method="filterTag"
                filter-placement="bottom-end"
            >
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <el-tag
                            v-show="!scope.row.isEdit"
                            class="card-status"
                            :type="
                                scope.row.card_status === '0'
                                    ? 'success'
                                    : 'danger'
                            "
                        >
                            {{
                                scope.row.card_status === '0'
                                    ? '正常'
                                    : scope.row.card_status === '1'
                                    ? '挂失'
                                    : '黑卡'
                            }}</el-tag
                        >
                        <el-tag
                            v-show="scope.row.isEdit"
                            v-for="(tag, index) in tagArray"
                            :key="tag.name"
                            class="tag-card"
                            :class="
                                editMember.card_status === index + ''
                                    ? 'active'
                                    : ''
                            "
                            :type="tag.type"
                            @click="selectTag(index)"
                        >
                            {{ tag.name }}
                        </el-tag>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="操作" fixed="right">
                <template #default="scope">
                    <el-button
                        v-show="scope.row.isEdit"
                        size="small"
                        @click="confirmEdit(scope.$index, scope.row)"
                        >确认</el-button
                    >
                    <el-button
                        v-show="!scope.row.isEdit"
                        size="small"
                        @click="handleEdit(scope.$index, scope.row)"
                        >编辑</el-button
                    >
                    <el-button
                        v-show="scope.row.isEdit"
                        size="small"
                        @click="cancelEdit(scope.$index, scope.row)"
                        >取消编辑</el-button
                    >
                    <el-popconfirm
                        title="确定删除?"
                        @confirm="deleteWorker(scope.row.member_ID)"
                    >
                        <template #reference>
                            <el-button
                                v-show="!scope.row.isEdit"
                                size="small"
                                type="danger"
                                >删除</el-button
                            >
                        </template>
                    </el-popconfirm>
                </template>
            </el-table-column>
        </el-table>
        <el-button size="default" @click="addMember" style="margin-top: 10px"
            >增加员工</el-button
        >
        <div class="sub-title second-title">操作记录</div>
        <el-table :data="historyData" max-height="280px" style="width: 100%">
            <el-table-column label="操作ID">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span>{{ scope.row.ID }}</span>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="操作人">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span>{{ scope.row.member_name }}</span>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="工号">
                <template #default="scope">
                    {{ scope.row.member_ID }}
                </template>
            </el-table-column>
            <el-table-column label="操作类型">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span></span>
                        <el-tag
                            :type="
                                scope.row.operation_type === '0'
                                    ? ''
                                    : 'success'
                            "
                        >
                            {{
                                scope.row.operation_type === '0'
                                    ? '充值'
                                    : '消费'
                            }}
                        </el-tag>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="金额(￥)" prop="operation_money">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span>{{ scope.row.operation_money }}</span>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="操作时间">
                <template #default="scope">
                    <div style="display: flex; align-items: center">
                        <span>{{ scope.row.operation_time }}</span>
                    </div>
                </template>
            </el-table-column>
        </el-table>
        <el-dialog
            v-model="dialogVisible"
            title="增加员工信息"
            width="30%"
            draggable
        >
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
                        @blur="handleBlur"
                        placeholder="工号"
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
            </el-form>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="dialogVisible = false">取消</el-button>
                    <el-button type="primary" @click="confirmAdd">
                        确认
                    </el-button>
                </span>
            </template>
        </el-dialog>
    </div>
</template>

<script lang="ts" setup>
import {
    addMemberApi,
    deleteMemberApi,
    editMemberApi,
    getHistoryApi,
    getManagementApi,
} from '@/api/management';
import { onMounted, reactive, ref } from 'vue';
import { History, WorkerMan } from '@/api/management/types';
import { ElMessage } from 'element-plus';
import type { FormInstance, FormRules } from 'element-plus';
import { useRoute } from 'vue-router';
const route = useRoute();
localStorage.setItem('currentPath', route.path);
const tableData = ref<WorkerMan[]>([]);
const historyData = ref<History[]>([]);
let dialogVisible = ref(false);
let editMember = reactive({
    ID: '',
    member_ID: '',
    member_name: '',
    sex: '男',
    age: 18,
    balance: 100.0,
    card_status: '',
});
const addMemberInfo = reactive<RuleForm>({
    member_ID: '',
    member_name: '',
    sex: '男',
    age: 18,
    balance: 0,
    card_status: '0',
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
const tagArray = ref([
    { name: '正常', type: 'success' },
    { name: '挂失', type: 'danger' },
    { name: '黑卡', type: 'danger' },
]);
let onEdit = ref(false);
const initTableData = async () => {
    try {
        tableData.value = (await getManagementApi()).map((item) => {
            return { ...item, isEdit: false };
        });
    } catch (error) {
        /* empty */
    }
};
const initHistory = async () => {
    try {
        historyData.value = await getHistoryApi();
    } catch (error) {
        /* empty */
    }
};
onMounted(() => {
    initTableData();
    initHistory();
});
const selectTag = (index: number) => {
    editMember.card_status = index.toString();
};
const handleEdit = (index: number, row: any) => {
    if (!onEdit.value) {
        onEdit.value = true;
    } else {
        ElMessage.warning('请先完成当前编辑！');
        return;
    }
    editMember.ID = row.ID;
    editMember.age = Number(row.age);
    editMember.balance = Number(row.balance);
    editMember.member_name = row.member_name;
    editMember.sex = row.sex;
    editMember.card_status = row.card_status;
    editMember.member_ID = row.member_ID;
    row.isEdit = true;
};
const confirmEdit = async (index: number, row: any) => {
    if (hasEmptyProperty(editMember)) {
        ElMessage.warning('请完善所有信息！');
        return;
    }
    let obj = {
        ID: editMember.ID,
        age: editMember.age.toString(),
        balance: editMember.balance.toString(),
        member_name: editMember.member_name,
        sex: editMember.sex,
        card_status: editMember.card_status,
        member_ID: editMember.member_ID,
    };
    try {
        await editMemberApi(obj);
        ElMessage.success('编辑成功');
        await initTableData();
        row.isEdit = false;
        onEdit.value = false;
    } catch (err) {
        console.log(err);
    }
};
const hasEmptyProperty = (obj: Record<string, any>): boolean => {
    for (const key in obj) {
        // eslint-disable-next-line no-prototype-builtins
        if (obj.hasOwnProperty(key)) {
            const value = obj[key];
            if (value === null || value === undefined || value === '') {
                return true;
            }
        }
    }
    return false;
};
const filterTag = (value: string, row: any) => {
    return row.card_status === value;
};
const cancelEdit = (index: number, row: any) => {
    row.isEdit = false;
    onEdit.value = false;
};
const deleteWorker = async (member_ID: string) => {
    await deleteMemberApi(member_ID);
    onEdit.value = false;
    ElMessage.success('删除成功');
    await initTableData();
};
const addMember = () => {
    dialogVisible.value = true;
};
interface RuleForm {
    member_ID: string;
    member_name: string;
    age: number;
    sex: string;
    balance: number;
    card_status: string;
}
const ruleFormRef = ref<FormInstance>();
const rules = reactive<FormRules<RuleForm>>({
    member_ID: [
        {
            required: true,
            message: '请输入员工号',
            trigger: 'blur',
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
const confirmAdd = async () => {
    dialogVisible.value = false;
    let obj = {
        member_ID: addMemberInfo.member_ID,
        member_name: addMemberInfo.member_name,
        age: addMemberInfo.age.toString(),
        sex: addMemberInfo.sex,
        balance: addMemberInfo.balance.toString(),
        card_status: '0',
    };
    await addMemberApi(obj);
    await initTableData();
};
const validateInput = (value: string) => {
    const pattern = /[^\u4e00-\u9fa5]/; // 使用正则表达式匹配非中文字符
    if (!pattern.test(value)) {
        ElMessage.error('工号不能包含中文字符，请修改!');
    }
};
const handleBlur = () => {
    validateInput(addMemberInfo.member_ID);
};
const handleBlurT = () => {
    validateInput(editMember.member_ID);
};
</script>
<style>
.sub-title {
    font-weight: bold;
    margin-bottom: 5px;
}
.second-title {
    margin-top: 10px;
}
.tag-card {
    margin: 5px;
}
.tag-card:hover {
    border-color: var(--theme-color);
    cursor: pointer;
}
.active {
    border-color: var(--theme-color);
}
.addInfo {
    display: flex;
    flex-direction: column;
}
</style>
