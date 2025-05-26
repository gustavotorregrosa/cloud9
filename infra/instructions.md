docker network create ecommerce-net
docker compose -f backend-docker-compose.yaml up -d --build

ssh ubuntu@172.27.0.1 'bash /home/ubuntu/cloud9/infra/backend.deploy.sh'