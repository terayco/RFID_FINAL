import { ref, Ref, onMounted } from 'vue';

export function useWebSocket(url: string): {
    incomingMessages: Ref<string[]>;
    sendMessage: (message: any) => void;
    back: Ref<boolean>;
} {
    const incomingMessages: Ref<string[]> = ref([]);
    let ws: WebSocket | null = null;
    const back = ref(false);
    localStorage.setItem('back', back.value + '');
    onMounted(() => {
        ws = new WebSocket(url);
        back.value = false;
        ws.onopen = () => {
            // incomingMessages.value.push('连接已建立');
        };

        ws.onmessage = (event) => {
            console.log(event.data);
            // 将数据按逗号分割，成新数组

            const resArr = event.data.split(',');
            // console.log(resArr);
            incomingMessages.value = resArr;
            // console.log(incomingMessages);
            if (
                incomingMessages.value.length > 1 &&
                incomingMessages.value[0] === '1'
            ) {
                localStorage.setItem('member_ID', incomingMessages.value[1]);
            }
            if (
                incomingMessages.value[0] === '0' &&
                localStorage.getItem('currentPath') === '/reset'
            ) {
                back.value = true;
                localStorage.setItem('back', back.value + '');
            } else {
                back.value = false;
                localStorage.setItem('back', back.value + '');
            }
        };

        ws.onclose = () => {
            // incomingMessages.value.push('连接已关闭');
        };
    });
    function sendMessage(message: string): void {
        if (ws && ws.readyState === WebSocket.OPEN) {
            ws.send(message);
        }
    }
    return {
        incomingMessages,
        sendMessage,
        back,
    };
}
