# FILE:         Dockerfile
# CONTRIBUTOR:  Jack Burns
# COPYRIGHT:    Copyright (c) 2017-2018 by Loopie, LLC.  All rights reserved.
#
# .NET Core based image that implements a given service.
#
# ARGUMENTS:
#
#       APPNAME     - Name of the application binary/script (e.g "automap")
#                     This makes it easier to clone and reuse this Dockerfile.

ARG APPNAME
FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview3-alpine3.9 AS build
STOPSIGNAL SIGTERM
ARG APPNAME
EXPOSE 80 7933

ENV TZ=America/Los_Angeles
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# copy csproj and restore as distinct layers
WORKDIR /$APPNAME
COPY *.sln .
COPY $APPNAME/*.csproj ./$APPNAME/
COPY . .
RUN dotnet restore $APPNAME/$APPNAME.csproj

# copy everything else and build app
COPY $APPNAME/. ./$APPNAME/
WORKDIR /$APPNAME/$APPNAME
RUN dotnet publish -c Release -o out

# Runtime Layer
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview3-alpine3.9 As runtime
ARG APPNAME
WORKDIR /$APPNAME
COPY --from=build /$APPNAME/$APPNAME/out ./

ENTRYPOINT ["dotnet", "CadenceClientLinux.dll"]