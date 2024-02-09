FROM node:18-alpine as development
ENV NODE_ENV development
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
RUN npm run build
EXPOSE 5173
EXPOSE 4173
CMD [ "npm", "run", "dev" ]