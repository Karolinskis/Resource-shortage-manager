# Resource-shortage-manager

## Assumptions

- **Shortages file size**: It is assumed that the shortages file is not too big because currently the whole file is being loaded into memory.
- **Input Validation**: It is assumed that the user inputs are valid and follow the expected format. Basic validation is performed, but extensive validation is not implemented.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

## Running the Solution

```sh
cd ResourceShortageManager
dotnet run
```

## Running unit tests

To run the unit tests, use the following command:

```sh
cd ResourceShortageManager.Tests
dotnet test
```
