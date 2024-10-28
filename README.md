# Movie Challenge
The application is a .Net Core server with a React UI using the MUI component library. It's actually the first time I've worked with React to build something like this. I've tried to hit most of the user requirements, though I have not implemented anything for actors. There is also some additional functionality in the UI that lets the user view posters, which I did to try things out.
## The Database
I've seeded the movies CSV into a SQL lite db using EF Core migrations, and split the genres into their own entitiy with a many to many relationship with the movies. Were it a production setup, this could be substituted for a proper SQL Server DB.
## The API
The API can be hit indendently of the UI. It currently does not have any authentication, so it can even be queried via a browser URL pretty simply currently.

The following example searches for movies with Batman in the title, sorted descending by title, which has the science fiction genre, and shows the third page of 10 results.
`/movie?pageSize=10&page=2&orderBy=title&orderAscending=false&searchModel.title=Batman&searchModel.genres=15`

You can do `movie/getGenres` to list all the genres to find IDs to search by.

Thge idea is that all pagniated results from the API would follow a format where they accept pageSize etc, but then have a more specific searchModel embeded in the request for particular search requirements for that entity. A swagger etc would be a good next step to improve the API.
## The UI
I've used the MUI component library to achieve the material look, and I've used flexbox in places to make things responsive. Many of my styles are currently embedded in-line, which is a possible room for improvement. You can search on the title, filter genres (its a multiselect), and sort on the Title and Release Date columns. The pagination resets when the core filters are changed so that it reflects the new data set in a less confusing way.
## Running the app
It should be fairly straightforward to run once pulled down. You will probably need to set multiple startup projects so that it runs the `.server` project and the `.client` project.
Its possible that the migrations will not auto-run. If so, target the DAL in Package Manager and run `update-database`.

Whilst the API is spinning up, the UI will error, so you will need to wait for it to finish loading and then manually refresh currently. If VS decides to break on any errors in the UI it can stop the API from starting until they are allowed to continue (initially it never did this for me, but then started doing so when I updated VS to a later version). 
## Deployment
I tried publishing it to Docker Desktop, but ran into some snags with the Vite files. However, it has been made using the default VS template, so I think it should be a simple application to deploy for someone with a bit more React experience.
## Unit Testing
I've not got round to any, but the app is structured with dependency injection of a MovieService using an interface, which means it would be simple to set some unit tests up.
## Exception handling
I implemented global exception handling on the server to standardise the format to the UI, as well as automatically log them all (though the logger is not wired up currently). In most cases the UI will mask the server exception, but I created a custom UiFriendlyException which can be used to communicate user friendly messages that are safe for the UI layer to show in toasts etc. To demonstrate the error handling toast in the UI you can do the following:

-If you search `throw exception` via the UI title search, a generic exception will be thrown on the server, which the UI will mask with a custom message.

-If you search `throw custom exception`, a UiFriendlyException will be thrown, which the UI will render with a custom message provided by the back-end.
