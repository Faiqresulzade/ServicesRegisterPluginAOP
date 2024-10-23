# ServicesRegisterPlugin

`AutoDIRegister` is a dynamic service registration plugin for .NET, enabling automatic service registration based on custom attributes.
It supports different service lifetimes, such as `Singleton`, `Scoped`, and `Transient`, and provides configurable options for flexible dependency injection.

## Features

- Automatically registers services marked with custom attributes: `[Singleton]`, `[Scoped]`, and `[Transient]`.
- Configurable options to customize service registration behavior.
- Supports open generics and custom type filters.
- Handles registration conflicts gracefully based on configuration.

## Getting Started

### Installation

You can install the package via NuGet:

```bash
dotnet add package AutoDIRegister
