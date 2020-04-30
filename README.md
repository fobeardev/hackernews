# HackerNews
This is a technical assessment utilizing the hacker news api. It is an Asp.Net Core app with an Angular front end. The Angular portion uses Angular CDK / Material to present a virtual scrolling list. The virtual scrolling isn't perfect, for example the recent items change often. This means if a call is made to fetch the first 30 results, the next 30 might not be the same 31-60 items from the first call. To fix that I could implement some caching in the future - and possibly a hard refresh button.

## Running

To utilize Angular during development I added the `Microsoft.AspNetCore.SpaServices` NuGet package that includes AngularCli middleware. There seems to be a bug with this MiddleWare and Angular 9 (latest). Since I generated a 9 project I had to trick it because it appears to be looking for a specific message from angular that might have changed from 8 (the listening on host... message). I hate to reference random links for issues but here is where the information is coming from: https://github.com/angular/angular-cli/issues/16961.

### Option 1: run from Visual Studio

Load the solution and press F5

### Option 2: run from command line / terminal

- Make sure to install https://docs.microsoft.com/en-us/dotnet/core/sdk
- run `dotnet run`

## Future Opportunities for Improvement

- Host on Azure
- Add search feature
- Add caching
- Change between recent / top / comments etc
