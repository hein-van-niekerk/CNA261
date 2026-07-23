#Base image to compile and built the app
#Including additional libraries and tools from csproj
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#Create a working directory in the build stage image
WORKDIR /src
#csproj file contains important data on dependencies which should be restored.
COPY DemoApi.csproj .
#dotnet restore will retrieve all dependencies mentioned above.
RUN dotnet restore
#Copy the entire rest of the source code to work directory for publishing.
#We want a final app, not debug version
COPY . .
#This command below will compile the app and publish it to a folder called /app/publish in the build stage image.
RUN dotnet publish -c Release -o /app/publish

#Runtime base image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#work directory in the runtime stage image
WORKDIR /app
#copy the published app from the build stage image to the runtime stage image
COPY --from=build /app/publish .
#The EXPOSE instruction informs Docker that the container listens on the specified network ports at runtime.
EXPOSE 8080
#What to start when the app runs in the container, dotnet is needed to run and we run DemoApi.dll which comes from the build image.
ENTRYPOINT ["dotnet", "DemoApi.dll"]
