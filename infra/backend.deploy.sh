#!/bin/bash
set -e
cd $(dirname "$0")/..
docker compose -f backend-docker-compose.yaml up -d --build