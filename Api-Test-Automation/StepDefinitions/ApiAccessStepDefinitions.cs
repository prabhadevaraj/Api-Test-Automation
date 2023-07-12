using TechTalk.SpecFlow;
using NUnit.Framework;
using ApiTest.Helpers;
using Ngr.Api_Tests.Helpers;

namespace ApiTest.StepDefinitions
{
    [Binding]
    public sealed class apiAccessStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public apiAccessStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("Access Token with grant type '(password|clientCredential)' is retrieved")]
        public void GivenAccessTokenIsGranted(string grantType)
        {
            apiAccessHelper.retrieveAccessToken(grantType);
        }
        [Then("The response code receives is (.*)")]
        public void ThenResponseCodeValidation(string expectedResponseCode)
        {
            System.Net.HttpStatusCode expRespCode;
            switch (expectedResponseCode)
            {
                case "OK":
                    expRespCode = System.Net.HttpStatusCode.OK;
                    break;
                case "Created":
                    expRespCode = System.Net.HttpStatusCode.Created;
                    break;
                case "NoContent":
                    expRespCode = System.Net.HttpStatusCode.NoContent;
                    break;
                case "NotFound":
                    expRespCode = System.Net.HttpStatusCode.NotFound;
                    break;
                case "InternalServerErr":
                    expRespCode = System.Net.HttpStatusCode.InternalServerError;
                    break;
                case "Unauthorized":
                    expRespCode = System.Net.HttpStatusCode.Unauthorized;
                    break;
                case "Forbidden":
                    expRespCode = System.Net.HttpStatusCode.Forbidden;
                    break;
                case "BadRequest":
                    expRespCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case "Conflict":
                    expRespCode = System.Net.HttpStatusCode.Conflict;
                    break;
                default:
                    Assert.Fail("Expected response " + expectedResponseCode + " is not known");
                    expRespCode = System.Net.HttpStatusCode.MethodNotAllowed;       // Any abitrary error code to assign the var. with value
                    break;
            }
            Assert.AreEqual(expRespCode, apiAccessHelper.lastResposne.StatusCode, "HTTP response code is not as expected");
        }
    }
}
