import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: [{ find: "@", replacement: "/src" }],
  },
  server: {
    proxy: {
      "/api": {
        target: "https://localhost:7218",
        // target: "http://localhost:5200",
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
