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

4. Database Setup
- Update Database connection if needed.
- Enable and add a migration
- Update database.

5. Create Account and Login


<!-- USAGE EXAMPLES -->
## Usage
During registration, the user is asked to answer some questions about their running preferences. This and the zipcode they provide are used to display forecast events on the calendar along with upcoming runs the user has.

### Race Plans
User can create a training plan with general information about the race they are training for, including the date.  They can then assign runs to include with this plan.

If the user elects to make the plan public, other users can use it as a template to create their own plan.  They will need to enter new information about the race, but the runs assigned to the template will copy over (with out the actual distance and time) to the new plan 
and are scheduled at the same time interval before the new race date as the existing date.  The below code snippet shows this calculation.

```sh
            List<RunCreate> templateRuns = new List<RunCreate>();
            foreach (var item in existingRace.ListOfRuns)
            {
                var runCreate = new RunCreate()
                {
                    RacePlanId = entity.Id,
                    PlannedDistance = item.PlannedDistance,
                    ScheduleDateTime = entity.RaceDate-(existingRace.RaceDate-item.ScheduledDateTime),
                    Description = item.Description,
                    LocationId = item.LocationId
                };
                var savedRun = await runService.CreateRunAsync(runCreate);

```
### Runs
Once users create a training plan, they can assign runs to it.  Runs consist of general information such as Planned Distance, Schedule Date and Time, Location, etc.

The estimated time to complete the run is calculated based off the user's pace that they provide and the planned distance for the run.

Once the run is complete, the user can update the run with the actual time and distance.

### Locations
Runs can also be assigned to user-created locations.  Location objects contain information for reference such as location, maximum distance, path types.

### Forecast Events
The application uses the WeatherUnlockedAPI to fetch the forecast for the upcoming days.  Using this information, the applcation creates events when the user would not want to run based off their preferences.  These events include excessive heat, excessive precipitation, morning, and lack of light.  The code below creates events when their is excessive heat.

The forecast events are displayed on a calendar along with upcoming runs so that the user can schedule them accordingly.

```sh
foreach (var item in forecastDay.TimeFrames)
                {
                    if (!_likesHeat)
                    {
                        if (item.FeelsLike_F >= 89 && !isHot)
                        {
                            startHeat = item.Time / 100;
                            isHot = true;
                        }
                        if (item.FeelsLike_F < 89 && isHot)
                        {
                            var endHeat = item.Time / 100;
                            isHot = false;
                            var startTime = forecastDay.Date.AddHours(startHeat);
                            var endTime = forecastDay.Date.AddHours(endHeat);

                            var heatEvent = new ForecastEvent()
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                Description = "Too hot"
                            };
                            listForecastEvents.Add(heatEvent);
                        }
                    }
```


### FullCalendar plugin

The FullCalendar plugin is used in a number of views to display and dynamically add/edit runs, as well as forecast events created as explained above.  

<!-- ROADMAP -->
## Roadmap
- Proposed additional feautures.
- Integration with third-party calendar and fitness-tracker api's/services.
- Forecasts based on run locations
- Logic to provide suggested run times based on forecast, user preferences and previous commitments.


<!-- CONTACT -->
## Contact

- Jeremy Hansen - https://www.linkedin.com/in/jeremy-hansen/

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
