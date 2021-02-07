## Quick start
- Open RandomNumbersApp.sln in Visual Studio;
- Build;
- Once execute _Update-Database_ in Package Manager Console;
  - Note: This command creates default db.
- Run via IIS Express.

## Settings
- appsettings.json
  - MatchSettings -> DurationInSec. Default: 15 sec.
    
## Game
I changed a little original rules. I do not have pre-defined games. The game starts when first Participant presses "Play Now".

!!! Do not forget after Register process press Confirmation link.

## Annotation by author
Game should work as expected by requirements.
This coding task took about my whole day. Cause I do not create each day new Web Project including .Net Core + Built-in auth + Angular. :) 
- I spent some time to read more about built-in Authentication / Authorization mechanism in .Net Core, cause on my previous projects we used partially hand-made stuff. So I left built-in auth without changes & any customization.
- I spent some time to read / deal with Entity Framework and Migrations, cause last 4+ years I had been using ORM NHibernate mainly.
- I should say that I have some ideas How to improve solution and possible weak points, but good now better than perfect tomorrow :).

Enjoy playing and win :)
