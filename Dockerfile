FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

RUN dotnet workload install wasm-tools

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM nginx:alpine
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
