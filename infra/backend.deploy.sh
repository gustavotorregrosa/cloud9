#!/bin/bash
set -e
cd ..
docker compose -f backend-docker-compose.yaml up -d --build