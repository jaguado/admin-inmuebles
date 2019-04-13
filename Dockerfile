FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY . ./
RUN dotnet restore

# Copy everything else and build
RUN dotnet test -c Release /p:CollectCoverage=true
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
#RUN apt-get install -y newrelic-netcore20-agent
ENV TZ America/Santiago
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AdminInmuebles.dll"]