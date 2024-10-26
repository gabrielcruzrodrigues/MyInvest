FROM node:18
WORKDIR /app
COPY package*.json ./
RUN npm install --legacy-peer-deps
RUN npm install -g @angular/cli@18 chokidar-cli
COPY . .
EXPOSE 9090
CMD ["ng", "serve", "--port", "9090", "--host", "0.0.0.0", "--poll", "2000"]