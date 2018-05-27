# use the libs image as a base layer with all other dependancies pre cached
FROM musicstore-sharedlibs AS build-env

# start building app
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY ./*.csproj ./
RUN dotnet restore

WORKDIR /app/ClientApp
COPY ./ClientApp/package.json ./ClientApp/package-lock.json ./
RUN apt-get update \
    && apt-get install -y bzip2 \ 
    && npm install phantomjs-prebuilt \
    && npm install --no-audit

WORKDIR /app
# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out /p:ClientAppEnv=localdocker

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
#ENV ASPNETCORE_URLS "http://0.0.0.0:5600/"
ENTRYPOINT ["dotnet", "MusicStoreDemo.dll"]