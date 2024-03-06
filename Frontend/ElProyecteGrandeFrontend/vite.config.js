import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  base: "/",
  plugins: [react()],
  preview: {
    port: 4173,
    strictPort: true,
  },
  server: {
    port: 4173,
    strictPort: true,
    host: true,
    origin: "http://127.0.0.1:4173",
  },
});
