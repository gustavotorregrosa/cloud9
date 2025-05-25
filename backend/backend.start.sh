#!/bin/bash
set -e
cd /app/
dotnet ef database update --context ApplicationDbContext -- --environment Development
dotnet run -- --environment "Development"