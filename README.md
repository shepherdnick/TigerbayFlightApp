# TigerbayFlightApp

Author: Nick Shepherd
Date: 21/10/2018

To use:
-------

- Clone/download the repository and run it in Visual Studio.  I used Visual Studio 2017 Community to develop the solution.  
- It is written in ASP.NET Web Forms.
- I wrote my own REST Client to be able to communicate with the API.
- I ran out of time to write any unit tests, but it was my intention to write some.
- You should not need any third party resources - I used bootstrap to style, but included all the relevant source files.
- If required, Default.aspx should be set as the start page (right click on it in Solution Explorer and select "Set as Start Page" from the context menu.

Architecture:
-------------

- The solution will populate the drop down menus for origin and destination selection from the API
- When the user has selected their values, they click the Search button.
- If there are search results to display, it populates a repeater which displays the result.
- If there are more results to display, a Next Page button appears at the end of the list.
- If there are previous results to display, a Previous Page button appears at the end of the list.
- If there aren't any flights found, all other desinations are checked for their flight count and a cache is built so that subsequent requested destinations are validated before requesting data from the API.  The cache expires after 6 hours.
