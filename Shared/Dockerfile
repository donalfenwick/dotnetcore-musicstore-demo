FROM microsoft/aspnetcore-build:2.1.300-preview1 AS build-env

# create an image with containing the shared class library projects so that each of the 
# individual web apps apps can use it as a base image during build 

WORKDIR /Shared/Common
COPY ./Common/*.csproj ./
RUN dotnet restore

WORKDIR /Shared/Database
COPY ./Database/*.csproj ./
RUN dotnet restore

WORKDIR /Shared/DatabaseMySqlMigrations
COPY ./DatabaseMySqlMigrations/*.csproj ./
RUN dotnet restore

WORKDIR /Shared/Common
COPY ./Common/ ./

WORKDIR /Shared/Database
COPY ./Database/ ./

WORKDIR /Shared/DatabaseMySqlMigrations
COPY ./DatabaseMySqlMigrations/ ./

WORKDIR /Shared
RUN dotnet build Common/MusicStoreDemo.Common.csproj \
    && dotnet build Database/MusicStoreDemo.Database.csproj \
    && dotnet build DatabaseMySqlMigrations/DatabaseMySqlMigrations.csproj
