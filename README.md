# .NET Core Email Sender Demo
 This is a .NET Core 3.1 Web API and Class Library VS demonstration solution for sending emails using [jstedfast](https://github.com/jstedfast)'s [MailKit](https://github.com/jstedfast/MailKit).
 
 ## Table of Contents

* [Install & Setup](#installation-and-setup)
  * [Download](#download)
  * [Open](#open)
  * [Configure](#configuration)
  * [Start](#start)
* [Usage](#usage)
  * [Lite Webpage](#lite-webpage)
  * [Postman](#postman)
* [Dependencies](#dependencies)
 
## Installation and Setup
 
### Download
- Go to the repo's [main page](../../), click the "Code" dropdown and select "[Download ZIP](../../archive/refs/heads/main.zip)" or "[Open with GitHub Desktop](x-github-client://openRepo/https://github.com/Tr-st-n/DotNetCore-Email-Sender-Demo)".
- You can also [use this repository as a template](../../generate) if you wish.
![Download or open in GitHub Desktop](https://i.imgur.com/8I6TxCx.gif)

### Open
- Open the Visual Studio solution [EmailDemo.sln](/EmailDemo/EmailDemo.sln) in the [EmailDemo folder](/EmailDemo/).
![Open EmailDemo.sln](https://i.imgur.com/jSIgu62.gif)

### Configuration
#### EmailWebAPI
1. Open the [appsettings.json](/EmailDemo/EmailWebAPI/appsettings.json) file and replace the faux settings under the "EmailConfig" section with valid credentials.
![appsettings.json](https://i.imgur.com/Ex6iwdg.gif)
2. (Optional) Head to the [nlog.config](/EmailDemo/EmailWebAPI/nlog.config) file and change the log file directory by editing the "fileName" attribute under the "targets" section (defaults to `C:\EmailDemoLogs\`).
![nlog.config](https://i.imgur.com/tQJdXJc.gif)
#### LiteWebPage
3. (Optional) If you make any changes to the [launchsettings.json](/EmailDemo/EmailWebAPI/Properties/launchSettings.json) or the controller routing and want to use the the web page supplied as a simple client to interact with the web API, make sure to update the `const SERVER_API_METHOD_URL` of the [index.html](/EmailDemo/Index.html).
![update index.html SERVER_API_METHOD_URL](https://i.imgur.com/0aDlUcO.gif)

### Start
- If you've made all the changes you need to and your credentials in the [appsettings.json](/EmailDemo/EmailWebAPI/appsettings.json) are valid, you should be good to go!
  ![run in IIS Express](https://i.imgur.com/BwfgKUF.png)
- **NOTE:** You will likely receive a "Security Warning" while starting up the project for the first time regarding installing a root certificate. This is normal. Click "Yes".

### Caveats
- If you are using GMail as your SMTP host, you will need to go into your [account security settings](https://myaccount.google.com/security) and turn ON "Less secure app access".
![less secure app access](https://i.imgur.com/iM2fEmg.png)

## Usage

### Lite webpage

- To use the lite webpage as a client, open the [index.html](/EmailDemo/Index.html) in a chromium browser while your app is running on IIS Express. If you change any controller routing or the socket that the app runs on, make sure to update the `const SERVER_API_METHOD_URL` in the [index.html](/EmailDemo/Index.html).
![lite webpage demo](https://i.imgur.com/gIB9WTE.gif)

### Postman
1. Create a new `POST` request sending the content-type `form-data` to `https://localhost:{PORT}/{CONTROLLER}`. Default URL: `https://localhost:44358/Email`. URL can be changed in [launchsettings.json](/EmailDemo/EmailWebAPI/Properties/launchSettings.json)).
![postman ss 1](https://i.imgur.com/UIpIyPN.png)
2. The request body should have a minimum of two key value pairs: 1) `recipients` and 2) `subject` OR `textbody`. The `subject` and `textbody` value(s) should be plain text. The `recipients` value should be a json object. Below is an example of a valid `recipients` value:
    ```json
    [
     {
      "Name":"John",
      "Address":"j0hn@test.tld"
     },
     {
      "Name":"Jim",
      "Address":"jimothy1@case.tld"
     }
    ]
    ```
    ![postman ss 2](https://i.imgur.com/H3UfkiA.gif)
  3. Optional data: you can add email attachment files for your request directly to Postman. Any unique key will work.
    ![postman ss 3](https://i.imgur.com/9XSqaEc.png)
  4. Make sure your app is running and then press "Send"!

## Dependencies
The following projects rely on the dependences:
- **EmailCore** (Class Library)
  - Frameworks
    1. Microsoft.NETCore.App (3.1.0)
  - Packages
    1. [MailKit (2.15.0)](https://www.nuget.org/packages/MailKit/2.15.0)
    2. [Microsoft.AspNetCore.Http.Features (5.0.10)](https://www.nuget.org/packages/Microsoft.AspNetCore.Http.Features/5.0.10)
    3. [Microsoft.Extensions.Logging.Abstractions (5.0.0)](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/5.0.0)
- **EmailWebAPI** (Web API)
  - Frameworks
    1. Microsoft.AspNetCore.App (3.1.10)
    2. Microsoft.NETCore.App (3.1.0)
  - Packages
    1. [Newtonsoft.Json (13.0.1)](https://www.nuget.org/packages/Newtonsoft.Json/13.0.1)
    2. [NLog (4.7.11)](https://www.nuget.org/packages/NLog/4.7.11)
    3. [NLog.Extensions.Logging (1.7.4)](https://www.nuget.org/packages/NLog.Extensions.Logging/1.7.4)
    4. [NLog.Web.AspNetCore (4.14.0)](https://www.nuget.org/packages/NLog.Web.AspNetCore/4.14.0)
  - Projects
    1. EmailCore
