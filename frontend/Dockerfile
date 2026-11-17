# ==============================================================================
# TEMA 4: CONTENERIZACIÓN DEL FRONTEND (REACT + VITE -> NGINX)
# Estrategia: Multi-Stage Build (Node.js Build -> Servidor Web Nginx Alpine)
# ==============================================================================

# ------------------------------------------------------------------------------
# ETAPA 1 (Build): Compilación y Empaquetado de Assets con Node.js / pnpm
# ------------------------------------------------------------------------------
FROM node:22-alpine AS build
WORKDIR /app

# Habilitar corepack (soporte nativo para pnpm / npm) y copiar manifiestos
RUN corepack enable
COPY package*.json ./

# Instalar dependencias y compilar el bundle estático de producción
RUN npm install
COPY . .
ENV VITE_API_URL=/api
RUN npm run build

# ------------------------------------------------------------------------------
# ETAPA 2 (Producción): Extracción de estáticos (dist) hacia Nginx Ultraligero
# ------------------------------------------------------------------------------
FROM nginx:1.27-alpine AS production

# 1. Copiar configuración personalizada de Nginx (Reverse Proxy /api + SPA)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# 2. Extraer únicamente la carpeta /app/dist generada en la Etapa 1
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
