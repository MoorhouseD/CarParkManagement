# CarParkManagement

Simple in-memory car park API.

## How to run

- Requires .NET 10 SDK.
- From solution root run `dotnet run --project CarParkManagement/CarParkManagement.csproj`.
- Development configuration (charges and standing charge) is in `CarParkManagement/appsettings.Development.json`.

## Endpoints

- POST `/parking`
  - Body: `{ "VehicleReg": string, "VehicleType": int }` where `VehicleType` is 1=SmallCar,2=MediumCar,3=LargeCar
  - Returns: `{ "VehicleReg": string, "SpaceNumber": int, "TimeIn": DateTime }`

- GET `/parking`
  - Returns: `{ "AvailableSpaces": int, "OccupiedSpaces": int }`

- POST `/parking/exit`
  - Body: `{ "VehicleReg": string }`
  - Returns: `{ "VehicleReg": string, "VehicleCharge": decimal, "TimeIn": DateTime, "TimeOut": DateTime }`

## Assumptions

- In-memory array used for parking spaces, no DB persistence.
- Should not round up for partial minute/timeframe charges unless configured otherwise.
- Limited parking spaces, set in appsettings JSON.
- Charges are configured via appsettings JSON.

## Tests

- Unit tests are under `CarParkManagement.Test` and can be run from the solution root with:

  `dotnet test`

- To run the specific test project:

  `dotnet test ./CarParkManagement.Test/CarParkManagement.Test.csproj`

## Docker

A `Dockerfile` is included to build a container image. Example commands:

- Build image (from repository root):

  `docker build -t carparkmanagement .`

- Run container (maps container port 8080 to host):

  `docker run -e ASPNETCORE_URLS="http://+:8080" -p 8080:8080 carparkmanagement`

Notes:
- The container exposes ports 8080 and 8081; map whichever you need.
- Set `ASPNETCORE_ENVIRONMENT=Development` when running the container to use `appsettings.Development.json` if desired.

## Notes

- Controller actions accept JSON bodies matching the required request shapes.
- Uses Scalar API for API Documentation.