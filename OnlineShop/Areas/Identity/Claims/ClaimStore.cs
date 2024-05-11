using System.Security.Claims;

namespace PresentationLayer.Areas.Identity.Claims
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
                 new Claim(ClaimTypesStore.AddProduct,true.ToString()),
                 new Claim(ClaimTypesStore.DeleteProduct,true.ToString()),
                 new Claim(ClaimTypesStore.ProductEdit,true.ToString()),
                 new Claim(ClaimTypesStore.ProductsList,true.ToString()),
                 new Claim(ClaimTypesStore.ProductDetails,true.ToString()),
        };
    }
    public static class ClaimTypesStore
    {
        public const string ProductsList = "ProductsList";
        public const string AddProduct = "AddProduct";
        public const string DeleteProduct = "DeleteProduct";
        public const string ProductDetails = "ProductDetails";
        public const string ProductEdit = "ProductEdit";
    }
}
