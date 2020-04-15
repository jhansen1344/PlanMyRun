<!--
*** Thanks for checking out this README Template. If you have a suggestion that would
*** make this better, please fork the repo and create a pull request or simply open
*** an issue with the tag "enhancement".
*** Thanks again! Now go create something AMAZING! :D
***
***
***
*** To avoid retyping too much info. Do a search and replace for the following:
*** github_username, repo, twitter_handle, email
-->





<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
<!-- PlanMyRun -->
<br />
<p align="center">
https://planmyrun.azurewebsites.net/

  <h3 align="center">PlanMyRun</h3>
</p>

<!-- TABLE OF CONTENTS -->
## Table of Contents

* [About the Project](#about-the-project)
  * [Built With](#built-with)
* [Getting Started](#getting-started)
  * [Installation](#installation)
* [Usage](#usage)
* [Roadmap](#roadmap)
* [Contact](#contact)
* [Acknowledgements](#acknowledgements)



<!-- ABOUT THE PROJECT -->
## About The Project
PlanMyRun is a .Net Entity FrameWork MVC application utilizing an n-tier architecture built during the phase of the Jan.-April 2020 Full-time Software Development bootcamp at ElevenFifty Academy. 

Through the application, users can create run training plans for upcoming races. Using the WeatherUnlockedAPI the application will display the forecast for upcoming days along with the runs that are scheduled for those days. The runs will block out time(s) based on the forecast and user preferences and allow the user to schedule/re-schedule runs.

### Built With

* [.Net Framework]()
* [Bootstrap]()
* [WeatherUnlockedAPI]()
* [FullCalendarPlugIn]()
* [Moment.js]()
* [Razor Pages]()





<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Installation
 
1. Clone the repo
```sh
git clone https://github.com/jhansen1344/PlanMyRun.git
```
2. Restoring NuGetPackages
```sh
nuget restore PlanMyRun.sln
```
3. Registering With WeatherUnlocked
Register with WeatherUnlocked and insert the AppId and AppKey into lines 40 and 41 in the ForecastService.
```sh
        readonly string uri = "http://api.weatherunlocked.com/api/forecast/";
        private string appId = "exampleAppId";
        private string appKey = "exampleAppKey";
```

3. Database Setup
- Update Database connection if needed.
- Enable and add a migration
- Update database.

4. Create Account and Login


<!-- USAGE EXAMPLES -->
## Usage

### Race Plans

### Runs

### Locations

### Forecast Events

<!-- ROADMAP -->
## Roadmap
- Proposed additional feautures.
- Integration with third-party calendar and fitness-tracker api's/services.
- Forecasts based on run locations
- Logic to provide suggested run times based on forecast, user preferences and previous commitments.


<!-- CONTACT -->
## Contact

- Jeremy Hansen - jhansen1344@gmail.com

<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements

### FullCalendar Plugin Resources
Full Calendar plug in

https://fullcalendar.io/docs
https://www.toshalinfotech.com/Blogs/ID/115/How-to-Integrate-Full-calendar-with-MVC-application
https://stackoverflow.com/questions/43437575/jquery-fullcalendar-callback-is-not-defined-error/43473348

Add extra fields to full calendar

https://stackoverflow.com/questions/3585481/add-extra-fields-to-fullcalendar
https://fullcalendar.io/docs/eventRender

### RazorPage Resources
multiple models in view for forecast and runs

https://www.c-sharpcorner.com/article/asp-net-mvc5-multiple-view-models-in-single-view/

Partial views

https://www.tutorialsteacher.com/mvc/partial-view-in-asp.net-mvc

CRUD Users

https://www.yogihosting.com/aspnet-core-identity-create-read-update-delete-users/

### Formatting and Parsing
Display Decimal with no zero's (hight/low temp, precip)

https://stackoverflow.com/questions/20848969/what-is-the-proper-data-annotation-to-format-my-decimal-property

Specifying format for date from weather api

https://stackoverflow.com/questions/21256132/deserializing-dates-with-dd-mm-yyyy-format-using-json-net

### Data Validation

DateTime compare

https://docs.microsoft.com/en-us/dotnet/api/system.datetime.compare?view=netframework-4.8

ZipCode validation

https://stackoverflow.com/questions/14942602/c-validation-for-us-or-canadian-zip-code

### JSON and AJAX
Json

https://stackoverflow.com/questions/21578814/how-to-receive-json-as-an-mvc-5-action-method-parameter

AntiForgery Token

https://stackoverflow.com/questions/4074199/jquery-ajax-calls-and-the-html-antiforgerytoken/4074289#4074289
https://forums.asp.net/t/1991886.aspx?Using+AntiForgeryToken+in+ajax+action+call+in+the+index+view

### Bootstrap, CSS, Styling
Navbar styling

https://getbootstrap.com/docs/4.4/components/navbar/

Color scheme palette

https://coolors.co/browser/best/1

styling css list

https://www.bitdegree.org/learn/css-list-style

Updating bootstrap

https://stackoverflow.com/questions/48550955/mvc-bootstrap-navbar-not-working-after-running-nuget-updates

### ReadMe
https://github.com/othneildrew/Best-README-Template
