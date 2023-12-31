import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path from 'path'; // 需要安装 @types/node
import { createSvgIconsPlugin } from 'vite-plugin-svg-icons';
import { UserConfig, ConfigEnv, loadEnv } from 'vite';
import { viteMockServe } from 'vite-plugin-mock';
import eslintPlugin from 'vite-plugin-eslint';
import * as process from 'process';
import { VitePWA } from 'vite-plugin-pwa';
// https://vitejs.dev/config/
// console.log(process.cwd()) // D:\vite-vue3-ts // 当前工作目录
const config = loadEnv('development', './');
export default defineConfig({
    plugins: [
        vue(),
        createSvgIconsPlugin({
            // 指定需要缓存的图标文件夹
            iconDirs: [path.resolve(process.cwd(), 'src/assets/icons')],
            // 指定symbolId格式
            symbolId: 'icon-[dir]-[name]', // icon-文件夹名-文件名
        }),
        viteMockServe({
            mockPath: './mock', // mock文件夹路径
            localEnabled: false, // 是否开启开发环境
        }),
        eslintPlugin({
            include: ['src/**/*.{vue,js,ts,jsx,tsx}'],
        }),
        VitePWA({
            includeAssets: ['favicon.svg'],
            manifest: false,
            registerType: 'autoUpdate',
            workbox: {
                runtimeCaching: [
                    {
                        urlPattern: /someInterface/i, // 接口缓存 此处填你想缓存的接口正则匹配
                        handler: 'CacheFirst',
                        options: {
                            cacheName: 'interface-cache',
                        },
                    },
                    {
                        urlPattern: /(.*?)\.(js|css|ts)/, // js /css /ts静态资源缓存
                        handler: 'CacheFirst',
                        options: {
                            cacheName: 'js-css-cache',
                        },
                    },
                    {
                        urlPattern:
                            /(.*?)\.(png|jpe?g|svg|gif|bmp|psd|tiff|tga|eps)/, // 图片缓存
                        handler: 'CacheFirst',
                        options: {
                            cacheName: 'image-cache',
                        },
                    },
                ],
            },
        }),
    ],
    server: {
        hmr: {
            overlay: false, // 关闭热更新的错误提示
        },
        port: 8080, // 端口号
        cors: true, // 允许跨域
        host: '0.0.0.0', // 允许从ip启动,
        open: true, // 启动时自动打开浏览器
        proxy: {
            '/api': {
                target: config.VITE_APP_API_BASEURL,
                changeOrigin: true,
                // rewrite: (path) => path.replace(/^\/api/, ''),
            },
        },
    },
    resolve: {
        // alias: {
        //     "@": path.resolve(__dirname, "src"),  // 配置别名,
        // },
        alias: [
            {
                find: '@',
                replacement: path.resolve(__dirname, 'src'),
            },
            {
                find: 'vue-i18n',
                replacement: 'vue-i18n/dist/vue-i18n.cjs.js', //解决i8n警告
            },
        ],
    },
    // 配置less
    css: {
        preprocessorOptions: {
            less: {
                charset: false,
                additionalData: '@import "./src/styles/global.less";', // 不能少“ ; ”
            },
        },
    },
    // 配置打包
    build: {
        outDir: 'dist', // 打包后的目录
        assetsDir: 'assets', // 静态资源目录
        minify: 'esbuild', // 压缩方式
        sourcemap: false, // 是否开启sourcemap,
        chunkSizeWarningLimit: 500, // 限制单个文件的大小,
        rollupOptions: {
            output: {
                entryFileNames: 'js/[name].js',
                chunkFileNames: 'js/[name].js',
                assetFileNames: 'assets/[name].[ext]',
                manualChunks(id) {
                    if (id.includes('node_modules')) {
                        return id
                            .toString()
                            .split('node_modules/')[1]
                            .split('/')[0]
                            .toString();
                    }
                },
            },
        },
    },
});
