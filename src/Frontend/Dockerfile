# use the libs image as a base layer with all other dependancies pre cached
FROM musicstore-sharedlibs AS build-env

# start building app
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY ./*.csproj ./
RUN dotnet restore

WORKDIR /app/ClientApp
COPY ./ClientApp/package.json ./ClientApp/package-lock.json ./
RUN npm install --no-audit

WORKDIR /app
# Copy everything else and build
COPY ./ ./
RUN dotnet publish --no-restore -c Release -o out /p:ClientAppEnv=localdocker

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /root/.aspnet/https /root/.aspnet/https

ENTRYPOINT ["dotnet", "MusicStoreDemo.dll"]