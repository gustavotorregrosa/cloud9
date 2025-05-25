#!/bin/bash

# Exit on error
set -e

# Update migrations
dotnet ef database update --context ApplicationDbContext -- --environment Development

# Run the application with both appsettings files (Docker has priority)
dotnet run -- --environment "Development"