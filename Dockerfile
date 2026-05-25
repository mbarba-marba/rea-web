FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

RUN dotnet workload install wasm-tools

# Tailwind standalone CLI — no Node.js required
RUN curl -sL https://github.com/tailwindlabs/tailwindcss/releases/download/v3.4.17/tailwindcss-linux-x64 \
    -o /usr/local/bin/tailwindcss \
    && chmod +x /usr/local/bin/tailwindcss

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN tailwindcss -i ./wwwroot/css/tailwind-input.css -o ./wwwroot/css/tailwind.css --minify
RUN dotnet publish -c Release -o out

FROM nginx:alpine
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
COPY publish/nginx.template /etc/nginx/templates/default.conf.template
