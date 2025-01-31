# Use official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY *.sln ./
COPY BitesByte_API/BitesByte_API.csproj BitesByte_API/
COPY OrderService/OrderService.csproj OrderService/
RUN dotnet restore

# Copy entire project files
COPY . ./

# Publish BitesByte_API
RUN dotnet publish BitesByte_API/BitesByte_API.csproj -c Release -o /app/out_BitesByte_API

# Publish OrderService
RUN dotnet publish OrderService/OrderService.csproj -c Release -o /app/out_OrderService

# Use runtime-only image for final deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy built files from builder stage
COPY --from=build /app/out_BitesByte_API /app/BitesByte_API
COPY --from=build /app/out_OrderService /app/OrderService

# Expose ports for both APIs
EXPOSE 8080

# Start both APIs using a script
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh
CMD ["/app/entrypoint.sh"]

