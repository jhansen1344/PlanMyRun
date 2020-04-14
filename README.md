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
<!-- AuctionExpress -->
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

AuctionExpress is a .Net Entity Framework WebAPI utilizing an n-tier architecture built during the BlueBadge phase of the Jan.-April 2020 Full-time Software Development bootcamp at ElevenFifty Academy.

Through the Api, registered users can post products for sale in an online auction format, as well as bid on currently open auctions.  Once the auctions are over, the products sellers can create transactions, simulating a third-party service that would handle credit card transactions.

The repo also includes a simple front-end built using razor pages to access and demonstrate the api end-points.


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
git clone https://github.com/ngahm/AuctionExpress.git
```
2. Restoring NuGetPackages
```sh
nuget restore AuctionExpress.sln
```
3. Database Setup
- Update Database connection if needed.
- Enable and add a migration
- Update database.

4. Create Account and Login


<!-- USAGE EXAMPLES -->
## Usage

Account Registration and Tokens
Using the Account/Register api endpoint, a user can register and gain access to user-specific endpoints.  A list of endpoint access restrictions can be found in the Roles section.

Tokens can be requested through /token and sending a registered user name and password.  The included front-end requests a token through the "Login" button and stores it as a cookie for use while the user is "signed in."  
```sh
public ActionResult Login(LoginBindingModel model, string returnUrl)
        {
                var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ),
                        new KeyValuePair<string, string>( "username", model.UserName ),
                        new KeyValuePair<string, string> ( "Password", model.Password )
                    };
                var content = new FormUrlEncodedContent(pairs);
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44320/");
                var response = client.PostAsync("token", content).Result;
                var token= response.Content.ReadAsStringAsync().Result;
                Response.Cookies.Add(CreateCookie(token));
                //Response.Flush();
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetOpenProduct", "ProductView");
                }
                ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);

                return View();
            }
```

Example of including stored token during api requests.
```sh
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44320/api/");
                string token = DeserializeToken();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var responseTask = client.GetAsync("product");
                responseTask.Wait();

```
Clicking "Logout"  sets the cookie expiration to the previous day.
```sh
public ActionResult LogOff()
        {
            if (Request.Cookies["UserToken"] != null)
            {
                Response.Cookies["UserToken"].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Login","AccountView");
        }
```
### Roles
There are three important user roles that the api uses to grant/restrict access to api endpoints:Admin, ActiveUser, InActiveUser.  

Upon registration, a user is automatically added into the ActiveUser role, allowing them to perform actions such as post and bid on products.

Throught the Account/Deactivate endpoint, a user is moved to the InActiveUser role and are no longer allowed to perform actions such as post and bid on products while maintaining the integrity of the database.

The Admin role is used for administration purposes such as assigning users to roles and creating product categories.

Below is a detailed view into which endpoints can be accessed based on user roles.

Non-signedin users/inactiveusers
- Account
- RegisterLogin
- GetOpenAuctions
- GetOpenAuctionsByCategory
- Getspecific auction byid
- Getlist of all cateogries
- get specific category

Admin
- All Admin endpoints
- Post new category
- Update Category
- Delete Category
- GetAllTransactions
- GetAllBids

ActiveUser
- DeactivateUser
- PostProduct
- Get User's products
- UpdateProduct
- DeleteProduct
- All Bid endpoints
- All Transaction endpoints
- GetAllProducts

### Products
Users can post products they want to sell with the following information.
```sh
public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int ProductQuantity { get; set; }
        public DateTimeOffset ProductStartTime { get; set; } = DateTimeOffset.Now;
        [Required]
        public DateTimeOffset ProductCloseTime { get; set; }
        public bool ProductIsActive
        {
            get
            {
                if (DateTimeOffset.Now < ProductStartTime || DateTimeOffset.Now > ProductCloseTime)
                {
                    return false;
                }
                else
                    return true;
            }
        }

        [ForeignKey(nameof(ProductCategoryCombo))]
        public int? ProductCategoryId { get; set; }
        public virtual Category ProductCategoryCombo { get; set; }

        [ForeignKey(nameof(Seller))]
        [Required]
        public string ProductSeller { get; set; }
        public virtual ApplicationUser Seller { get; set; }

        public virtual ICollection<Bid> ProductBids { get; set; }

        public double HighestBid
        {
            get
            {
                if (ProductBids.Count >0)
                {
                    var item = ProductBids.Max(x => x.BidPrice);
                    return item;
                }
                    return 0;
            }
        }
        public double MinimumSellingPrice { get; set; }
    }
```

### Bids
Users can also bid on products.  When a user attempts to place a bid, it is evaluated against the existing product to determine if the bid is valid.
```sh
        public string ValidateBid(BidCreate bid)
        {
            var prodDetail = GetProductById(bid.ProductId);

            if (prodDetail == null)
                return "Product has been removed or does not exist.";
            if (prodDetail.ProductSeller == _userId.ToString())
                return "Users can not bid on products they are selling.";
            if (!prodDetail.ProductIsActive)
                return "Auction is closed";
            if (prodDetail.MinimumSellingPrice > bid.BidPrice)
                return "Bid must be higher than produt's minimum selling price.";
            if (prodDetail.HighestBid > bid.BidPrice)
                return "Bid must be higher than current selling price.";
            return "";
        }
```
### Categories
Basic class for grouping similar products together.  Administered by users in the Admin role.

### Transactions
When an auction ends, the product owner can create and administer a transaction based off the winning bid.  This is to simulate a third party service that would handle payment processing from the information of the winning bidder.

<!-- ROADMAP -->
## Roadmap
- Proposed additional feautures.
- Integration with third-party calendar and fitness-tracker api's/services.
- Forecasts based on 


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
