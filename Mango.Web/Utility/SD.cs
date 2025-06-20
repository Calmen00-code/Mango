namespace Mango.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }

        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_CUSTOMER = "CUSTOMER";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
