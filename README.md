# Hot Sauce Database

#### By Noah Kise

#### A C# web application to allow users to upload hot sauces and tag them with flavor tags.

## Technologies Used

* C#
* .NET 6 SDK
* Entity Framework Core
* MySQL
* CSS
* Razor Pages
* Authentication / Authorization

## Description

This is a simple web application to allow users to maintain a database of hot sauces and organize them by flavor tags. A user must be logged in to create, update, or delete a sauce. Flavor tags are automatically added to the database when a user creates a sauce, or when they edit a sauce.

## Setup/Installation Requirements

* .NET must be installed. Latest version can be found [here](https://dotnet.microsoft.com/en-us/).
* To run locally on your computer, clone the main branch of this [repository](https://github.com/NoahKise/hot-sauce-database).
* In your terminal, navigate to the root folder of this project and run `dotnet restore`.
* Open MySQL Workbench. Latest version can be downloaded [here](https://dev.mysql.com/downloads/workbench/).
* Create a new file in the "HotSauce" directory called appsettings.json. NOTE: If you plan to use this project as a jumping off point for further development, you must ensure that appsettings.json is added to your .gitignore file and committed prior to creating this file.
* In `appsettings.json`, enter the following, replacing `USERNAME` and `PASSWORD` to match the settings of your local MySQL server. Replace `DATABASE-NAME` with whatever you would like to name your database.
  
```
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;database=DATABASE-NAME;uid=USERNAME;pwd=PASSWORD;"
    }
}
```
* In your terminal, navigate to the "HotSauce" directory and run `dotnet ef database update` to create a local database schema.
* To view the project in a web browser, navigate to the "HotSauce" directory and run `dotnet watch run`.

## Known Bugs

* When editing a sauce, if you remove a flavor tag, the selected flavor tag and every flavor tag the sauce has below it in the list will be removed from the sauce. Consequently, if you attempt to add flavor tags and remove flavor tags in the same edit session, the added tags will not be successful as their input fields are created below existing tags, at least one of which is being removed.
    * Currently I do not have a fix for this very frustrating and baffling bug.  Users should be advised that if they wish to remove flavor tags, they must do so, save changes, and then in a new edit session add back in any flavor tags that were unintentionally removed.

## License

Code licensed under [GPL](LICENSE.txt)

Any suggestions for ways I can improve this app? Reach out to me at noah@kisefamily.com

Copyright (c) February 16 2024 Noah Kise