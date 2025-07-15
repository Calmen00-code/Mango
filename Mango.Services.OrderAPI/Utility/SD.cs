namespace Mango.Services.OrderAPI.Utility
{
    public class SD
    {
        public static string ProductAPIBase { get; set; }

        public const string STATUS_PENDING = "PENDING";
        public const string STATUS_APPROVED = "APPROVED";
        public const string STATUS_READY_FOR_PICKUP = "READY_FOR_PICKUP";
        public const string STATUS_COMPLETED = "COMPLETED";
        public const string STATUS_REFUNDED = "REFUNDED";
        public const string STATUS_CANCELLED = "CANCELLED";

        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_CUSTOMER = "CUSTOMER";
    }
}
