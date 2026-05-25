# Stage 1: Build Tailwind CSS (node:20-alpine — Node pre-installed, fast)
FROM node:20-alpine AS css-build
WORKDIR /app
COPY package*.json tailwind.config.js postcss.config.js ./
RUN npm ci
COPY wwwroot/css/tailwind-input.css ./wwwroot/css/
COPY . .
RUN npm run build:css

# Stage 2: Build Blazor WASM
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
RUN dotnet workload install wasm-tools
COPY *.csproj .
RUN dotnet restore
COPY . .
COPY --from=css-build /app/wwwroot/css/tailwind.css ./wwwroot/css/tailwind.css
RUN dotnet publish -c Release -o out

# Stage 3: Serve with nginx
FROM nginx:alpine
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
COPY publish/nginx.template /etc/nginx/templates/default.conf.template
