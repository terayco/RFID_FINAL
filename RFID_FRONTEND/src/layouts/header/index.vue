<template>
    <div class="header-box">
        <div class="header-left">
            <IconFont
                class="github-icon"
                icon="bi:github"
                @click="goGithub"
            ></IconFont>
        </div>
        <div class="header-center">{{ title }}</div>
        <div class="header-right">
            <div class="switch-area">
                <IconFont
                    class="switch-icon"
                    icon="icon-park:switch"
                    @click="goManagement"
                />
            </div>
            <div class="theme-area">
                <IconFont icon="noto-v1:sun" />
                <el-switch v-model="isDark" @click="toggleTheme" />
                <IconFont icon="noto-v1:night-with-stars" />
            </div>
            <div class="full-area" @click="toggleScreen">
                <IconFont
                    icon="icon-park-outline:full-screen-one"
                    v-show="!full"
                />
                <IconFont
                    icon="fluent:full-screen-minimize-24-filled"
                    v-show="full"
                />
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue';
import { isFull, toggleFull } from 'be-full';
import { useDark, useToggle } from '@vueuse/core';
import { useRouter } from 'vue-router';
//
// const store = usePageStore();
//
// const instance = getCurrentInstance();
const router = useRouter();
// const toggleCollapse = () => {
//     const collapseState = store.$state.isCollapse;
//     instance?.proxy?.$Bus.emit('toggle-collapse', !collapseState);
// };
const goGithub = () => {
    window.open('https://github.com/yibaikuai/vue3-ts-template');
};
let full = ref(false);
let title = ref('');
const currentRoute = router.currentRoute.value.path;
currentRoute === '/management'
    ? (title.value = '后台管理')
    : (title.value = 'RFID监测');
const toggleScreen = () => {
    full.value = !isFull();
    toggleFull();
};
const isDark = useDark();
const toggleDark = useToggle(isDark);
const toggleTheme = () => {
    toggleDark(isDark.value);
};
const goManagement = () => {
    const currentRoute = router.currentRoute.value.path;
    currentRoute === '/management'
        ? router.push('/detect')
        : router.push('/management');
    currentRoute === '/management'
        ? (title.value = 'RFID检测')
        : (title.value = '后台管理');
};
</script>

<style scoped lang="less">
.header-box {
    width: 100%;
    height: 48px;
    display: flex;
    justify-content: space-between;
    .header-center {
        flex: 1;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 18px;
        color: var(--text-color);
    }
    .header-left {
        flex: 1;
        display: flex;
        justify-content: flex-start;
        align-items: center;
        .menu-icon {
            font-size: 37px;
            cursor: pointer;
            color: var(--text-color);
            &:hover {
                color: var(--theme-color);
            }
        }
        .github-icon {
            cursor: pointer;
            font-size: 22px;
            margin-left: 12px;
            color: var(--text-color);
            &:hover {
                color: var(--theme-color);
            }
        }
    }
    .header-right {
        flex: 1;
        align-items: center;
        display: flex;
        justify-content: right;
        .switch-area {
            color: var(--text-color);
            .switch-icon {
                color: var(--text-color);
            }
            display: flex;
            align-items: center;
            margin-right: 14px;
            cursor: pointer;
        }
        .theme-area {
            display: flex;
            align-items: center;
            margin-right: 10px;
            font-size: 17px;
            .el-switch {
                margin: 0 5px;
            }
        }
        .full-area {
            width: 48px;
            height: 48px;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            color: var(--text-color);
            &:hover {
                background-color: var(--hover-color);
            }
        }
    }
}
</style>
