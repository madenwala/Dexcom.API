# API for Dexcom
The Dexcom API enables the development of innovative apps that amplify the value and utility of CGM data. 

## Dexcom.Api
The [Dexcom.Api](src/Dexcom.Api) project enables .NET based applications to access the Dexcom API.

## Dexcom.Api.Uwp
This project is a helper library for Windows 10 applications. The key file in here is the `DexcomAuthProviderForWindows` which an implementation of the `Dexcom.Api.IDexcomAuthProvider` interface. This class inherits the interface and encapsulates the necessary logic on Windows 10 to call `Windows.Security.Authentication.Web.WebAuthenticationBroker` and have the user authenticate themselves and return back the necessary authorization token.

## Dexcom.Uwp
This is a sample UWP project that shows you how to integrate the Dexcom.Api project into a Windows 10 application.

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
