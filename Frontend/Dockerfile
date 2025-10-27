# Etapa de build Angular
FROM node:20 AS build
WORKDIR /app

# Copia dependências
COPY Frontend/package*.json ./
RUN npm install

# Copia o restante do projeto
COPY Frontend/ .

# Build do Angular (sintaxe moderna)
RUN npm run build -- --configuration production

# Etapa final (Nginx)
FROM nginx:alpine

# Remove configuração padrão do Nginx
RUN rm /etc/nginx/conf.d/default.conf

# Copia build do Angular - CORRIGIDO para incluir /browser
COPY --from=build /app/dist/ordo-tasks/browser /usr/share/nginx/html

# Copia configuração customizada do Nginx
COPY Frontend/nginx.conf /etc/nginx/conf.d/

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]