FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SynthPay.Ledger.Worker/SynthPay.Ledger.Worker.csproj", "SynthPay.Ledger.Worker/"]
COPY ["SynthPay.Contracts/SynthPay.Contracts.csproj", "SynthPay.Contracts/"]

RUN dotnet restore "SynthPay.Ledger.Worker/SynthPay.Ledger.Worker.csproj"

COPY . .
WORKDIR "/src/SynthPay.Ledger.Worker"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SynthPay.Ledger.Worker.dll"]