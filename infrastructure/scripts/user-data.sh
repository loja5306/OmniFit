#!/bin/bash
dnf update -y
systemctl enable docker --now

mkdir -p /usr/local/lib/docker/cli-plugins
curl -SL "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/lib/docker/cli-plugins/docker-compose
chmod +x /usr/local/lib/docker/cli-plugins/docker-compose

mkdir -p /opt/omnifit

cat > /opt/omnifit/docker-compose.yml << 'COMPOSE'
services:
  db:
    image: postgres:18-alpine
    container_name: omnifit_db
    restart: always
    environment:
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
      POSTGRES_DB: ${POSTGRES_DB}
    volumes:
      - postgres_data:/var/lib/postgresql/data

  backend:
    image: ${ECR_REGISTRY}/omnifit-backend:latest
    container_name: omnifit_api
    restart: always
    ports:
      - "80:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}
      - Jwt__Key=${JWT_KEY}
      - Jwt__Issuer=omnifit_api
      - Jwt__Audience=omnifit_web
      - AllowedOrigins__0=${FRONTEND_URL}
    depends_on:
      - db

volumes:
  postgres_data:
COMPOSE

cat > /opt/omnifit/.env << ENVFILE
POSTGRES_USER=$(aws ssm get-parameter --name /omnifit/db-user --with-decryption --query Parameter.Value --output text --region eu-west-2)
POSTGRES_PASSWORD=$(aws ssm get-parameter --name /omnifit/db-password --with-decryption --query Parameter.Value --output text --region eu-west-2)
POSTGRES_DB=$(aws ssm get-parameter --name /omnifit/db-name --query Parameter.Value --output text --region eu-west-2)
JWT_KEY=$(aws ssm get-parameter --name /omnifit/jwt-key --with-decryption --query Parameter.Value --output text --region eu-west-2)
ECR_REGISTRY=$(aws ssm get-parameter --name /omnifit/ecr-registry --query Parameter.Value --output text --region eu-west-2)
FRONTEND_URL=$(aws ssm get-parameter --name /omnifit/frontend-url --query Parameter.Value --output text --region eu-west-2)
ENVFILE

cd /opt/omnifit
docker compose up -d db