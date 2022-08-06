FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG PROJECT_NAME
WORKDIR /source

COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

RUN dotnet restore ./src/$PROJECT_NAME/$PROJECT_NAME.csproj

COPY ./src/. ./src/
RUN dotnet publish ./src/$PROJECT_NAME/$PROJECT_NAME.csproj -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
ARG PROJECT_NAME
ENV PROJECT_NAME=$PROJECT_NAME
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT dotnet $PROJECT_NAME.dll