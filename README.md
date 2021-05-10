# KostalApiClient
Make possible to connect to your PIKO IQ/Plenticore Plus inverters. 

## Usage
Create a KostalApiClient.Session object, with the IP of your device. Then you can already use the different enpoint properties : Auth, Events, Info, LogData, ProcessData, Modules and Settings. 

```csharp
Session session = new Session( "{YOUR_HOST}");
InfoVersion infos = await session.Info.GetVersion();
```

Some of the endpoint call need an authentication. You can login to the API with the Login (and you Password as parameter) method of the Session object.

```csharp
await session.Login("{YOUR_PASSWORD}");
Me me = await session.Auth.GetMe();
List<Event> events = await session.Events.GetLastest("fr-fr", 10);
```

## Sample
Check the simple sample 

