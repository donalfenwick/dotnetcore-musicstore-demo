FROM microsoft/dotnet:2.1-sdk AS build-env

# create an image with containing the shared class library projects so that each of the 
# individual web apps apps can use it as a base image during build 

#setup node
ENV NODE_VERSION=8.9.4 \
    NODE_DOWNLOAD_SHA=21fb4690e349f82d708ae766def01d7fec1b085ce1f5ab30d9bda8ee126ca8fc

#adding node to dotnet core image as per https://github.com/aspnet/Announcements/issues/298
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs \
    # install phantomJS
    && apt-get -qq update \ 
    && apt-get -qq -y install tree \ 
    && apt-get install -y bzip2 \ 
    && npm install phantomjs-prebuilt --no-save \
    #install gulp on the command line
    && npm install gulp-cli -g 

WORKDIR /root/.aspnet/https
COPY ./DevCerts/ ./

#copy dependencies and restore in a layer which can be cached by docker
WORKDIR /Shared/Database
COPY ./Database/*.csproj ./
RUN dotnet restore

WORKDIR /Shared/Common
COPY ./Common/*.csproj ./
RUN dotnet restore

#restore copy the rest of the files required for building the libs
WORKDIR /Shared/Database
COPY ./Database/ ./

WORKDIR /Shared/Common
COPY ./Common/ ./

#RUN tree ./
WORKDIR /Shared
RUN dotnet build Common/MusicStoreDemo.Common.csproj --no-restore  \
    && dotnet build Database/MusicStoreDemo.Database.csproj --no-restore  
