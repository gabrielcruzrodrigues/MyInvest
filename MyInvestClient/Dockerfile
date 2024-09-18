FROM node:18
WORKDIR /app
COPY package*.json ./
RUN npm install
RUN npm install -g @angular/cli@17 chokidar-cli
COPY . .
CMD ["ng", "serve", "--port", "9090", "--host", "0.0.0.0", "--poll", "2000"]

#usar: docker-compose up --build
#Com essa configuração, o Docker irá monitorar as mudanças no seu código e recompilar automaticamente quando necessário, similar ao dotnet watch run