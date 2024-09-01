# CodeSnippets
This was written as a school project as an example of a WPF application

# Setup
To get this project up and running, set the connection string to your Microsoft SQL Server Database (the database does not have exist yet). 

Then, delete everything in the migrations folder of the CodeSnippets.Data project. Click Tools > NuGet Package Manager > Package Manager Console to open up the console to run EF Core operations. Make sure to set the Default to be CodeSnippets.Data first and then run Add-Migration Initial and Update-Database
