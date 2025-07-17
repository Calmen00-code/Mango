namespace Mango.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }

        public static string ShoppingCartAPIBase { get; set; }
        public static string OrderAPIBase { get; set; }

        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_CUSTOMER = "CUSTOMER";
        public const string TOKEN_COOKIE = "JWT_TOKEN";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
