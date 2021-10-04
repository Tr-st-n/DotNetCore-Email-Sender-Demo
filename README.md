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
* [Solution Breakdown](#solution-breakdown)
  * [EmailCore](#emailcore)
  * [EmailWebAPI](#emailwebapi)
  * [LiteWebPage](#litewebpage)
* [Disclaimers](#disclaimers)
 
## Installation and Setup
 
### Download
- Go to the repo's [main page](../../), click the "Code" dropdown and select "[Download ZIP](../../archive/refs/heads/main.zip)" or "[Open with GitHub Desktop](x-github-client://openRepo/https://github.com/Tr-st-n/DotNetCore-Email-Sender-Demo)".
- You can also [use this repository as a template](../../generate) if you wish.
![Download or open in GitHub Desktop](https://i.imgur.com/8I6TxCx.gif)

### Open
- Open the Visual Studio solution [EmailDemo.sln](/EmailDemo/EmailDemo.sln) in the [EmailDemo folder](/EmailDemo/).
![Open EmailDemo.sln](https://i.imgur.com/jSIgu62.gif)

### Configuration
#### EmailWebAPI Config
1. Open the [appsettings.json](/EmailDemo/EmailWebAPI/appsettings.json) file and replace the faux settings under the "EmailConfig" section with valid credentials.
![appsettings.json](https://i.imgur.com/Ex6iwdg.gif)
2. (Optional) Head to the [nlog.config](/EmailDemo/EmailWebAPI/nlog.config) file and change the log file directory by editing the "fileName" attribute under the "targets" section (defaults to `C:\EmailDemoLogs\`).
![nlog.config](https://i.imgur.com/tQJdXJc.gif)
#### LiteWebPage Config
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
- [**EmailCore**](/EmailDemo/EmailCore) (Class Library)
  - Frameworks
    1. Microsoft.NETCore.App (3.1.0)
  - Packages
    1. [MailKit (2.15.0)](https://www.nuget.org/packages/MailKit/2.15.0)
    2. [Microsoft.AspNetCore.Http.Features (5.0.10)](https://www.nuget.org/packages/Microsoft.AspNetCore.Http.Features/5.0.10)
    3. [Microsoft.Extensions.Logging.Abstractions (5.0.0)](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/5.0.0)
- [**EmailWebAPI**](/EmailDemo/EmailWebAPI) (Web API)
  - Frameworks
    1. Microsoft.AspNetCore.App (3.1.10)
    2. Microsoft.NETCore.App (3.1.0)
  - Packages
    1. [Newtonsoft.Json (13.0.1)](https://www.nuget.org/packages/Newtonsoft.Json/13.0.1)
    2. [NLog (4.7.11)](https://www.nuget.org/packages/NLog/4.7.11)
    3. [NLog.Extensions.Logging (1.7.4)](https://www.nuget.org/packages/NLog.Extensions.Logging/1.7.4)
    4. [NLog.Web.AspNetCore (4.14.0)](https://www.nuget.org/packages/NLog.Web.AspNetCore/4.14.0)
  - Projects
    1. [EmailCore](/EmailDemo/EmailCore)

## Solution Breakdown
### EmailCore
- [Repo](/EmailDemo/EmailCore)
- Namespace: `EmailCore`
- **Summary**: Sends emails using the settings in the [appsettings.json](/EmailDemo/EmailWebAPI/appsettings.json) and logs and returns results.
- Classes & Interfaces:
    | File Name                                  | Kind            | Summary  |
    |--------------------------------------------|-----------------|----------|
    | Addressee.cs                               | Public Class    | Properties hold a `string` `Name` and an email `Address`. Contains a method `Valid()` that returns a boolean (if `Address` is a syntactically valid email address). |
    | BaseSenderResult.cs                        | Abstract Class  | Abstract class containing a `bool` `Successful`. Returned by `ISender` interface. |
    | EmailConfig.cs                             | Public Class    | Used as a singleton for holding our email configuration from `appsettings.json`. |
    | ISender.cs                                 | Interface       | Interface contracting a class to accept a `Message` argument and return a `BaseSenderResult`. |
    | Message.cs                                 | Public Class    | All of the information in an email: recipients (`List<MimeKit.MailboxAddress>` `To`), `string` `Subject`, `string` `Body`, attachments (`IFormFileCollection` `Attachments`). |
    | RichSenderResult.cs                        | Public Class    | Inherits from BaseSenderResult. Implements property containing `List<SenderError>` `Errors` for controller to return rich feedback to client. |
    | Sender.cs                                  | Public Class    | Implements `ISender` interface. Sends an email and returns the results in a `BaseSenderResult` that can be cast to a `RichSenderResult` optionally. |
    | SenderError.cs                             | Public Class & Enum | `SenderError`: has `string` `Message` and `SenderErrorKind` `Kind` members. `SenderErrorKind` is an enum that contains `Generic` and `TimeOut` constants. |

### EmailWebAPI
- [Repo](/EmailDemo/EmailWebAPI)
- Namespace: `EmailWebAPI`
- **Summary**: Web API with `Email` Controller that allows clients to `POST` `form-data` content-type requests in order to send emails.
- Files of interest:
    | File Name & Directory                      | Kind               | Summary |
    |--------------------------------------------|--------------------|---------|
    | Properties/launchSettings.json             | json config        | Launch settings for project including application URL. |
    | Controllers/EmailController.cs             | API Controller     | Sends emails, returns results. |
    | appsettings.json                           | json config        | Application settings; includes email host/user credentials/information. |
    | nlog.config                                | xml config         | Settings for NLog logging including log file directory. |

### LiteWebPage
- [Index.html repo](/EmailDemo/Index.html), [site.css repo](/EmailDemo/site.css)
- **Summary**: Webpage containing HTML, CSS and JS that sends POST requests to EmailWebAPI using AJAX.
- Remarks: If you update the URL/routing of the web API, make sure to update the `const SERVER_API_METHOD_URL` in [index.html](/EmailDemo/Index.html).

## Disclaimers
This repository is a demonstration only and should not be used in production. User input is not stringently validated and is susceptible to abuse. The web API and lite webpage client are currently configured to use [CORS](https://www.google.com/search?q=Cors) for demonstration purposes. Ask your doctor if this code is right for you.
