FROM mcr.microsoft.com/mssql/server:latest

USER root

RUN apt-get -y update && \
    apt-get install -y dos2unix

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=P@ssword123!

# Create app directory
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

# Bundle app source
COPY . /usr/src/app

RUN dos2unix *

# Grant permissions for the script to be executable
RUN chmod +x /usr/src/app/db-init.sh
RUN chmod +x /usr/src/app/entrypoint.sh

# Switch back to mssql user and run the entrypoint script
USER mssql
ENTRYPOINT /bin/bash ./entrypoint.sh
