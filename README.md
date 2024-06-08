# Ham Logger Gateway
[![CC BY-NC 4.0][cc-by-nc-shield]][cc-by-nc]

This work is licensed under a
[Creative Commons Attribution-NonCommercial 4.0 International License][cc-by-nc].

[![CC BY-NC 4.0][cc-by-nc-image]][cc-by-nc]

[cc-by-nc]: https://creativecommons.org/licenses/by-nc/4.0/
[cc-by-nc-image]: https://licensebuttons.net/l/by-nc/4.0/88x31.png
[cc-by-nc-shield]: https://img.shields.io/badge/License-CC%20BY--NC%204.0-lightgrey.svg

## Overview

The Ham Logger Gateway is designed to facilitate the processing of logging messages from different Ham Radio software. It supports both UDP and TCP protocols, providing a robust and efficient example of handling and processing messages.

## Features

- Asynchronous TCP and UDP server to handle multiple client connections concurrently.
- Efficient memory management using `MemoryPool`.
- Extensible architecture for easy customization.
- Comprehensive logging for monitoring and debugging.
- Blacklist functionality to block unwanted connections.
- Rate limiting to manage client request rates and prevent abuse.

## Requirements
- [.NET 8 SDK](https://docs.microsoft.com/en-us/dotnet/core/install/)
   - Installation instruction for [Windows](https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60), [Mac](https://docs.microsoft.com/en-us/dotnet/core/install/macos), [Linux](https://docs.microsoft.com/en-us/dotnet/core/install/linux)

## Installation
To install and run the Ham Logger Gateway, follow these steps:

1. Clone the repository:
    ```sh
    git clone https://github.com/KC3PIB/HamLoggerGateway.git
    cd HamLoggerGateway
    ```

2. Build the project:
    ```sh
    dotnet build
    ```

3. Configure N1MM:
   - Go to N1MM -> Config -> Broadcast Data
   - Check the types of data you wish to receive
   - Use 127.0.0.1:12060 or the values you have set in `config.json`

4. Run the project:
    ```sh
    dotnet run --project src/HamLoggerGateway.Example/HamLoggerGateway.Example.csproj
    ```

This example project will display all N1MM messages received as JSON in the console.

## Configuration

The Ham Logger Gateway can be configured through the `ServerSettings` class. Update the settings according to your network configuration and requirements.

Example configuration:
```csharp
var settings = new ServerSettings
{
    Address = "127.0.0.1",
    Port = 5000,
    BufferSize = 16384,
    EnableReuseAddress = true
};
