FROM ubuntu:latest

# Install required packages
RUN apt-get update && \
    apt-get install -y maven openjdk-17-jdk && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy source code
COPY . /app
COPY exec.sh /app

# Compile the package
# RUN mvn package -pl dev.torregrosa.cloud9
RUN chmod +x exec.sh

CMD ["./exec.sh"]