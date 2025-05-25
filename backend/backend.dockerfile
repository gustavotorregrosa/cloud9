FROM ubuntu:22.04

# Prevent interactive prompts during package installation
ENV DEBIAN_FRONTEND=noninteractive

WORKDIR /app

# Copy all files including backend.start.sh
COPY ./backend/backend.start.sh .


# Update and install basic packages
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        ca-certificates \
        curl \
        wget \
        vim \
        nano \
        net-tools \
        iputils-ping \
        sudo \
        locales && \
    rm -rf /var/lib/apt/lists/*

# Set locale
RUN locale-gen en_US.UTF-8
ENV LANG=en_US.UTF-8
ENV LANGUAGE=en_US:en
ENV LC_ALL=en_US.UTF-8

# Install dependencies for .NET
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        apt-transport-https \
        gnupg && \
    rm -rf /var/lib/apt/lists/*

# Add Microsoft package signing key and repository
RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb

# Install .NET 8 SDK and runtime
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        dotnet-sdk-8.0 \
        aspnetcore-runtime-8.0 && \
    rm -rf /var/lib/apt/lists/*

# Install dotnet-ef tool globally
RUN dotnet tool install --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

EXPOSE 5206

ENTRYPOINT ["tail", "-f", "/dev/null"]

# RUN chmod +x ./backend.start.sh
# ENTRYPOINT ["./backend.start.sh"]
