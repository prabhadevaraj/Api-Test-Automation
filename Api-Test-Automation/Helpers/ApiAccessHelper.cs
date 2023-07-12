using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ngr.Api_Tests.Helpers
{
    public static class apiAccessHelper
    {
        private static IConfiguration apiConfig;
        private static string accessToken;
        private static string tokenGrantType;
        public static IRestResponse lastResposne;
        private static string baseUrl;

        static apiAccessHelper()
        {
            apiConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            accessToken = "";
            tokenGrantType = "";
            baseUrl = apiConfig.GetSection("apiSettings").GetValue<string>("baseUri");
        }
        public static IRestResponse genAPIRequest(string endPoint, Method reqMethod, Dictionary<string, string> paramList, Dictionary<string, string> additionalHeaderList, string reqBody)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(endPoint, reqMethod);
            if (additionalHeaderList != null)
            {
                foreach (string headerKey in additionalHeaderList.Keys)
                {
                    request.AddHeader(headerKey, additionalHeaderList[headerKey]);
                }
            }
            if (paramList != null)
            {
                foreach (string paramKey in paramList.Keys)
                {
                    request.AddParameter(paramKey, paramList[paramKey]);
                }
            }
            if (reqBody != null)
            {
                request.AddParameter("text/json", reqBody.ToString(), ParameterType.RequestBody);
            }
            request.AddHeader("Authorization", "Bearer " + accessToken);
            IRestResponse resp = client.Execute(request);
            lastResposne = resp;
            return resp;
        }
        public static void retrieveAccessToken(string grantType, System.Net.HttpStatusCode expectedRespCode = System.Net.HttpStatusCode.OK)
        {
            var client = new RestClient(apiConfig.GetSection("apiSettings").GetValue<string>("tokenUri"));
            var request = new RestRequest(Method.POST);

            switch (grantType)
            {
                case "password":
                    request.AddParameter("grant_type", "password", ParameterType.GetOrPost);
                    request.AddParameter("username", apiConfig.GetSection("apiSettings").GetValue<string>("APITokenUsername"), ParameterType.GetOrPost);
                    request.AddParameter("password", apiConfig.GetSection("apiSettings").GetValue<string>("APITokenPassword"), ParameterType.GetOrPost);
                    request.AddParameter("scope", apiConfig.GetSection("apiSettings").GetValue<string>("APIAccessScope"), ParameterType.GetOrPost);
                    request.AddParameter("client_id", apiConfig.GetSection("apiSettings").GetValue<string>("APIAccessClientID"), ParameterType.GetOrPost);
                    break;
                case "clientCredential":
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Accept", "application/json");
                    JObject tokenRequestJSONBody = new JObject();
                    tokenRequestJSONBody.Add("username", apiConfig.GetSection("apiSettings").GetValue<string>("APITokenUsername"));
                    tokenRequestJSONBody.Add("password", apiConfig.GetSection("apiSettings").GetValue<string>("APITokenPassword"));
                    request.AddParameter("text/json", tokenRequestJSONBody.ToString(), ParameterType.RequestBody);
                    break;
                default:
                    throw new ArgumentException(string.Format("{0} is not a recognized grant type", grantType));
            }

            IRestResponse resp = client.Execute(request);
            if (resp.StatusCode == expectedRespCode)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(resp.Content);
                switch (grantType)
                {
                    case "password":
                        accessToken = jsonResponse.access_token;
                        break;
                    case "clientCredential":
                        accessToken = jsonResponse.accessToken;
                        break;
                    default:
                        throw new ArgumentException(string.Format("{0} is not a recognized grant type", resp.StatusCode.ToString()));
                }
                tokenGrantType = grantType;
            }
            else
            {
                throw new System.Net.WebException(string.Format("Reponse {0} to token request is not as expected.  Expecting {1}", resp.StatusCode.ToString(), expectedRespCode));
            }
        }
        public static string getBaseAPIUrl()
        {
            return baseUrl;
        }
    }
}
