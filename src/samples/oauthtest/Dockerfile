# use the libs image as a base layer with all other dependancies pre cached
FROM musicstore-sharedlibs AS build-env

# start building app
WORKDIR /samples/app
# Copy csproj and restore as distinct layers
COPY ./*.csproj ./
RUN dotnet restore 

# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /samples/app/out .

ENTRYPOINT ["dotnet", "oauthtest.dll"]