# TimeBasedOtpBt

##### Tooling
Tools required to run **TimeBasedOTPBT** solution:
1. .Net Core SDK
2. Visual Studio Community 2019

Tools required to run **totpbtui** project:
1. Node.js
2. Npm

##### How to run the applications
1. Clone the repo
2. Open TimeBasedOTPBT.sln in Visual Studio 2019
3. Run the solution from Visual Studio 2019
4. Open a command prompt as administrator
5. With command prompt go to TimeBasedOtpBt/totpbtui
6. Run **npm install**
7. Run **npm start**

##### Improvements
1. There is only one project. There were problems migrating the sqlite database, so instead of multiple projects, the solution contains one project with multiple folders representing the layers of the application (Persistence, Business Logic, Presentation)
2. JWT token is generated in controller, while it should have been generated inside the services layer.
3. The TOTP does not have a fixed expiry time, it varies between requests, but it generates one password on 30 seconds basis.
