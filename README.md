# CompanyAPI
A basic company api and client


Pre: Ensure .net 5 for iis is installed on the server you will be hosting these apps,
     If not download and installed .net 5 for iis https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-aspnetcore-5.0.5-windows-hosting-bundle-installer

1. Create an empty Database in your Sql Server
   Update API's appsettings.json DB string to match the empty database in your sql server,
   The default string is "Server=localhost\\SQLEXPRESS;Database=company_db;User Id=Test;Password=Test;"

2. Deploy API using IIS -> add new website
   Navigate to the API swagger address eg. "localhost:<port>/swagger" to confirm it is up and running

3. Update Client appsettings.json's companyAPI string to match the API's url in IIS,
   The deault value is "http://localhost:40/api/Company/",
   Make sure to include the "/api/Company/" on the end.   

4. Deploy Client to IIS -> add new website
   Navigate to the Clients web address,
   If the "Overview" section of the page is filled with data then the Client successfully connected to the API
