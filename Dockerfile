FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet test -c Release --collect "Code coverage"
RUN dotnet build -c Release -o out
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
# INSTALL NEWRELIC
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=195cb6e62921d83376172a83bb2389a5c9bedaac \
NEW_RELIC_APP_NAME=AdminInmuebles.Docker
# install newrelic with apt
RUN echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list
RUN apt-get update
RUN apt-get install -y gnupg2
RUN curl -s https://download.newrelic.com/548C16BF.gpg | apt-key add -
RUN apt-get install -y newrelic-netcore20-agent
ENV TZ America/Santiago
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AdminInmuebles.dll"]