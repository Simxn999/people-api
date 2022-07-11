# people-api

* Hämta alla personer i systemet
  * [GET] /api/people
  
* Hämta alla intressen som är kopplade till en specifik person
  * [GET] /api/people/{personID:int}/interests
  
* Hämta alla länkar som är kopplade till en specifik person
  * [GET] /api/people/{personID:int}/websites

* Koppla en person till ett nytt intresse
  * [POST] /api/interests
    {
      "title": "Title of interest",
      "description": "Description of interest"
    }
  * [POST] /api/interests/{interestID:int}/people/{personID:int}

* Lägga in nya länkar för en specifik person och ett specifikt intresse
  * [POST] /api/websites
    {
      "title": "Title of website",
      "link": "https://linktowebsite.com"
    }   
  * [POST] /api/websites/{websiteID:int}/people/{personID:int}
  * [POST] /api/websites/{websiteID:int}/interests/{interestID:int}    
