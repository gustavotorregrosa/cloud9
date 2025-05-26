#!/bin/bash
set -e
cd $(dirname "$0")/..
git pull
sudo docker compose -f frontend-docker-compose.yaml up -d --build