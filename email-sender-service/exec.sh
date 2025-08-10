#!/bin/bash

# Example bash script for email-sender-service

echo "Starting email sender service..."
cd /app
mvn clean install
mvn exec:java -Dexec.mainClass="dev.torregrosa.cloud9.App"