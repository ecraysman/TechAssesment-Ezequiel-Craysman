
Assumptions to the excercise

-DTO is not used here => For the sake of simplicity, I chose to avoid using DTOs to protect internal info.
In the real world, this should never happen.

Pending Things
-Idempotency is being validated in some cases. I Should have verified all possibilities.

Test Coverage
-The project needs way more testing. I've added just 4 basic test cases that are BASIC.
My current job got more intense these days and i've been unable to allocate enough time to complete this tech assesment.



How To Run
-> Generate initial database => 
#Using visual studio:
select project DB.Models => Run from the Package Manager Console => update-database
(If it thows an error, try Update-Database -Context DB.Models.Databasecontext)

#Command line:
Run => dotnet ef database update


RUNNING


-> Run from source => select API project as startup project and run on IIS-Express
-> Tests => Select test.API and run tests collection 
-> Swagger Use=> Use the online-autodocumentation and run examples.

==> Data Examples
To create an affiliate, use this in the body:
{
  "name": "Juan Perez",
  "email": "pepe@pepe.com"
}


To create an AffiliateCode, using the id in the previous response:
{

  "affiliateId": "REPLACEWITHPREVIOUSGUID",
  "affiliateCode": "AFFILIATE333333",
  "expirationDate": "2024-10-25T18:58:52.201Z",
  "availableAmount": 3
}

To create a customer:
{
  "Name": "Pedro",
  "Email": "pedro@customer.com",
  "affiliateId": "REPLACEWITHPREVIOUSGUID",
  "affiliateCodeUsed": "REPLACEWITHPREVIOUSAFFILIATECODE"
}


To list all the customers for an affiliate:
URL/api/Affiliates/AFFILIATE-GUID/Customers

