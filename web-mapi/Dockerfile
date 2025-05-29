FROM node:18.17.0

RUN mkdir -p /home/app

COPY ./web-mapi /home/app

WORKDIR /home/app/src

RUN npm install --force

CMD ["npm", "start"]