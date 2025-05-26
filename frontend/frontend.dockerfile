# Use official Node.js LTS image
FROM node:20-alpine

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY ./frontend/package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY ./frontend/ .

# Expose port (change if your app uses a different port)
EXPOSE 3000

# Start the application
CMD ["npm", "run", "dev"]