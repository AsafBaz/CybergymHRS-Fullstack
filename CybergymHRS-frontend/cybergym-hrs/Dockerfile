# Step 1: Build Angular App
FROM node:18 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .

ENV NODE_OPTIONS=--openssl-legacy-provider
RUN npm run build --prod

# Step 2: Serve it using nginx
FROM nginx:alpine
COPY --from=build /app/dist/cybergym-hrs /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/nginx.conf
