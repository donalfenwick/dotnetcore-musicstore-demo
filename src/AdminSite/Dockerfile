# use the libs image as a base layer with all other dependancies pre cached
FROM musicstore-sharedlibs AS build-env

# start building app
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY ./*.csproj ./package.json ./
RUN dotnet restore \
    && npm install gulp-cli -g  \
    && npm install 

# Copy everything else and build
COPY ./ ./
RUN gulp default-ci \
    && dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
#ENV ASPNETCORE_URLS "http://0.0.0.0:5600/"
ENTRYPOINT ["dotnet", "MusicStoreDemo.AdminSite.dll"]