#!/bin/sh
dotnet /app/BitesByte_API/BitesByte_API.dll &
dotnet /app/OrderService/OrderService.dll &
wait
