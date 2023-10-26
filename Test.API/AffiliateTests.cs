using DB.Models;
using API.Controllers;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Test.API
{
    public partial class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateAffiliateAndFail()
        {
            var Affiliates = new AffiliatesController(new AffiliateContext());

            var task =  Affiliates.PostAffiliates( new DB.Models.Affiliates { });
            
            task.Wait(); //Tests must be run sync to validate the entire process, so we wait until task is done before continuing.
            var response = task.Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.ToString(), Is.Not.Empty);

            //Serialize-DeserializeObject to get generic objects
            var responseJson = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(response));

            Assert.That(responseJson.Result.Value.Status.ToString(), Is.EqualTo("500"));
            

        }

        [Test]
        public void ListAffiliates()
        {
            var Affiliates = new AffiliatesController(new AffiliateContext());

            var task = Affiliates.GetAffiliates();

            task.Wait(); //Tests must be run sync to validate the entire process, so we wait until task is done before continuing.
            var response = task.Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.ToString(), Is.Not.Empty);

            
            Assert.That(response.Value.Count(), Is.GreaterThan(-1));


        }


        [Test]
        
        public void ListCustomers()
        {
            var Customers = new CustomersController(new AffiliateContext());

            var task = Customers.GetCustomers();

            task.Wait(); //Tests must be run sync to validate the entire process, so we wait until task is done before continuing.
            var response = task.Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.ToString(), Is.Not.Empty);


            Assert.That(response.Value.Count(), Is.GreaterThan(-1));


        }


        [Test]
        public void ListAffiliateCodes()
        {
            var AffiliateCodes = new AffiliateCodesController(new AffiliateContext());

            var task = AffiliateCodes.GetAffiliateCodes();

            task.Wait(); //Tests must be run sync to validate the entire process, so we wait until task is done before continuing.
            var response = task.Result;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.ToString(), Is.Not.Empty);

            
            Assert.That(response.Value.Count(), Is.GreaterThan(-1));


        }

    }
}