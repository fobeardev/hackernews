# HackerNews
This is example code utilizing the hacker news api. It is an Asp.Net Core app with an Angular front end. The Angular portion uses Angular CDK / Material to present a virtual scrolling list. The virtual scrolling isn't perfect, for example the recent items change often. This means if a call is made to fetch the first 30 results, the next 30 might not be the same 31-60 items from the first call. To fix that I could implement some caching in the future - and possibly a hard refresh button.

## Running

I removed `Microsoft.AspNetCore.SpaServices` from development process due to a bug with its handling of Angular 9

- Run the backend: 
    1. Visual Studio
        - Load the solution and press F5
    2. DotNet CLI
        - Make sure to install https://docs.microsoft.com/en-us/dotnet/core/sdk
        - Run the backend: `dotnet run`
- Run the frontend: 
    - cd to `/ClientApp` folder 
    - run `npm start`

Note: 

The ClientApp has a file `src/proxy.conf.json` which is used in `angular.json` for when `ng serve` (development is used)

This should proxy requests made from http://localhost:4200/api to https://localhost:44374/api

If you are getting 500 errors, it is most likely this

## Future Opportunities for Improvement

- Host on Azure
- Add search feature
- Add caching
- Change between recent / top / comments etc
