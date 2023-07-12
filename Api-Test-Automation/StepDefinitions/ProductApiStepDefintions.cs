using TechTalk.SpecFlow;
using Ngr.Api_Tests;
using Ngr.Api_Tests.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using Ngr.Api_Tests.Models;
using TechTalk.SpecFlow.Assist;

namespace ApiTest.StepDefinitions
{
    [Binding]
    public sealed class ProductsApiStepDefintions
    {
        private readonly ScenarioContext _scenarioContext;
        private string url = "";
        public int productId = 0;
        public ProductsApiStepDefintions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("Send Request to get list of Products")]
        public void SendRequestToGetListOfProductss()
        {
           url = ProductsEndpointMapper.GetProductsEndPointUrl(ProductsEndpointMapper.apiEndPointType.products);
           apiAccessHelper.genAPIRequest(url, RestSharp.Method.GET, null, null, null);

        }
        [Then(@"The returned response is not empty")]
        public void TheReturnedResponseIsNotEmpty()
        {
            ProductsListRespModel productsListResponse = JsonConvert.DeserializeObject<ProductsListRespModel>(apiAccessHelper.lastResposne.Content);
            long numOfProducts = productsListResponse.total;
            List<Products> productsList = productsListResponse.products;
            foreach (Products products in productsList)
            {
                Assert.IsTrue(numOfProducts > 90, "Total number of product is not more than 90");
                Assert.NotNull(products.id, String.Format("Id for product {0} is unexpectedly Null", products.id));
                Assert.NotNull(products.title, String.Format("Title for product {0} is unexpectedly Null", products.title));
                Assert.NotNull(products.description, String.Format("Description for product {0} is unexpectedly Null", products.description));
                Assert.NotNull(products.price, String.Format("Price for product {0} is unexpectedly Null", products.price));
            //Can add more validation based on the expectation
            }
        }
        [Given(@"Send Request to add a new product")]
        public void SendRequestToAddANewProduct(Table table)
        {
            string json = "";
            Assert.AreEqual(table.Rows.Count, 1, "The expected table row does not have a single row");
            AddNewProductReqModel addNewProductModel = table.CreateInstance<AddNewProductReqModel>();
            var AddNewProductReqModel = new AddNewProductReqModel()
            {
                title = addNewProductModel.title,
                description = addNewProductModel.description,
                price = addNewProductModel.price,
                discountPercentage = addNewProductModel.discountPercentage,
                rating = addNewProductModel.rating,
                stock = addNewProductModel.stock,
                brand = addNewProductModel.brand,
                category = addNewProductModel.category,
                thumbnail = addNewProductModel.thumbnail,
                images  = addNewProductModel.images,
            };
            url = ProductsEndpointMapper.GetProductsEndPointUrl(ProductsEndpointMapper.apiEndPointType.addProduct);
            json = JsonConvert.SerializeObject(addNewProductModel);
            apiAccessHelper.genAPIRequest(url, RestSharp.Method.POST, null, null, json);
        }
        [Then(@"The returned response has created productId")]
        public void TheReturnedResponseHasCreatedProductId()
        {
            AddNewProductRespModel response = JsonConvert.DeserializeObject<AddNewProductRespModel>(apiAccessHelper.lastResposne.Content);
            int productIdCreated = (int)response.id;
            Assert.NotZero(productIdCreated, "Newly Created product id is unexpectedly zero");
            _scenarioContext.Add("productId", productIdCreated);
            productId = productIdCreated;
        }

    }
}
