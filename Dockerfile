FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Node.js for Tailwind CSS build
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs

RUN dotnet workload install wasm-tools

COPY *.csproj .
RUN dotnet restore

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build:css
RUN dotnet publish -c Release -o out

FROM nginx:alpine
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
COPY publish/nginx.template /etc/nginx/templates/default.conf.template
