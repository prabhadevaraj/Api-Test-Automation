using ApiTest.Helpers;
using Ngr.Api_Tests.Helpers;

namespace Ngr.Api_Tests
{
    public static class ProductsEndpointMapper
    {
        public enum apiEndPointType
        {
            products,
            addProduct
        }

        public static string GetProductsEndPointUrl(apiEndPointType endpoint)
        {
            string endpointUrl = apiAccessHelper.getBaseAPIUrl();
            switch (endpoint)
            {
                case apiEndPointType.products:
                    endpointUrl += "/products/";
                    break;
                case apiEndPointType.addProduct:
                    endpointUrl += "/products/add";
                    break;
            }
            return endpointUrl;
        }

    }
}
