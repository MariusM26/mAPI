#FROM node:18.17.0
#
#RUN mkdir -p /home/app
#
#COPY ./web-mapi /home/app
#
#WORKDIR /home/app/src
#
#RUN npm install --force
#
#CMD ["npm", "start"]


FROM node:18.17.0

# Setăm directorul de lucru
RUN mkdir -p /home/app
WORKDIR /home/app

# Copiem fișierul de env pentru build (prod)
COPY ./web-mapi/.env.production .env

# Copiem aplicația
COPY ./web-mapi /home/app

# Navigăm în directorul corect
WORKDIR /home/app/src

# Instalăm dependințele
RUN npm install --force

# Build React cu variabilele din .env
RUN npm run build

# Servim aplicația cu un server static (ex: serve sau nginx)
# Dacă vrei să rulezi cu `npm start`, atunci trebuie să ai un server custom

# Dacă folosești create-react-app: serve aplicația static
RUN npm install -g serve
CMD ["serve", "-s", "build"]
