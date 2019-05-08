# API for Dexcom for .NET based applications
The Dexcom API enables the development of innovative apps that amplify the value and utility of CGM data. 

## Dexcom.Api
The [Dexcom.Api](src/Dexcom.Api) project enables .NET based applications to access the Dexcom API.

## Dexcom.Uwp
The [Dexcom.Uwp](src/Dexcom.Uwp) is a sample UWP project that shows you how to integrate the Dexcom.Api project into a Windows 10 application.

### Configure Dexcom.Uwp project
This is the Dexcom sample for Windows 10. To run this solution, you'll need to create a `Credentials.cs` file within `src\Dexcom.Uwp\` folder and place the following code within the file:
```csharp
public static class Credentials
{
    public const string CLIENT_ID = "<INSERT_HERE>";
    public const string CLIENT_SECRET = "<INSERT_HERE>";
    public const string CALLBACK_URL = "<INSERT_HERE>";
}
```
