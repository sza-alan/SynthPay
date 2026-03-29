FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SynthPay.Transactions.Api/SynthPay.Transactions.Api.csproj", "SynthPay.Transactions.Api/"]
COPY ["SynthPay.Transactions.Application/SynthPay.Transactions.Application.csproj", "SynthPay.Transactions.Application/"]
COPY ["SynthPay.Transactions.Domain/SynthPay.Transactions.Domain.csproj", "SynthPay.Transactions.Domain/"]
COPY ["SynthPay.Transactions.Infrastructure/SynthPay.Transactions.Infrastructure.csproj", "SynthPay.Transactions.Infrastructure/"]
COPY ["SynthPay.Contracts/SynthPay.Contracts.csproj", "SynthPay.Contracts/"]

RUN dotnet restore "SynthPay.Transactions.Api/SynthPay.Transactions.Api.csproj"

COPY . .
WORKDIR "/src/SynthPay.Transactions.Api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SynthPay.Transactions.Api.dll"]