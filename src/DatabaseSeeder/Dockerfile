# use the libs image as a base layer with all other dependancies pre cached
FROM musicstore-sharedlibs AS build-env

# start building app
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY ./*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish --no-restore -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "DatabaseSeeder.dll"]