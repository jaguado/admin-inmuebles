# BUILD ANGULAR APP
FROM node:10-alpine as node
COPY template/package.json template/package-lock.json ./
RUN npm ci && mkdir /ng-app && mv ./node_modules ./ng-app
WORKDIR /ng-app
COPY template/. .
RUN npm run build

# BUILD NETCORE APP
FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app
COPY *.sln ./
COPY src/*.csproj ./src/
COPY Tests/AdminInmueblesTests/*.csproj ./Tests/AdminInmueblesTests/
RUN dotnet restore
COPY . ./
RUN dotnet test -c Release /p:CollectCoverage=true
RUN dotnet publish -c Release -o out

# BUILD RUNTIME IMAGE WITH NEWRELIC
FROM microsoft/dotnet:aspnetcore-runtime
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=195cb6e62921d83376172a83bb2389a5c9bedaac \
NEW_RELIC_APP_NAME=AdminInmuebles.Docker
COPY src/newrelic/ ./newrelic/
RUN dpkg -i ./newrelic/newrelic-netcore20-agent_*.deb
ENV TZ America/Santiago
WORKDIR /app
COPY --from=build-env /app/src/out ./
COPY --from=node /ng-app/wwwroot ./wwwroot/
ENTRYPOINT ["dotnet", "AdminInmuebles.dll"]