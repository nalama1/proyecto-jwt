FROM mcr.microsoft.com/mssql/server:2019-latest
ENV ACCEPT_EULA=Y

USER root
RUN mkdir -p /var/opt/mssql/data /usr/src/sqlscripts && \
    chmod -R 777 /var/opt/mssql/data && \
    chmod -R 755 /usr/src/sqlscripts

COPY base_datos_proyecto-jwt.sql /usr/src/sqlscripts/base_datos_proyecto-jwt.sql
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl apt-transport-https gnupg && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev && \
    ln -s /opt/mssql-tools/bin/* /usr/local/bin/ && \
    apt-get clean && rm -rf /var/lib/apt/lists/*


USER 10001
CMD /bin/bash -c "/opt/mssql/bin/sqlservr & sleep 30 && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P \"$MSSQL_SA_PASSWORD\" -i /usr/src/sqlscripts/base_datos_proyecto-jwt.sql && wait"



# -S sqlserver es el nombre del servicio de base de datos en .yml
# \"$${DB_PASSWORD}\"
# \"$DB_PASSWORD\"
# \"$MSSQL_SA_PASSWORD\"
