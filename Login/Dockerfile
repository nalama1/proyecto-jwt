# build
FROM node:alpine as builder
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
RUN npm run build


# Etapa 2: Servir la aplicación con Nginx
FROM nginx:alpine
EXPOSE 80
COPY ./conf/default.conf /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/* 
COPY --from=0 /app/dist/login/browser /usr/share/nginx/html
RUN chmod -R 755 /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]