namespace BurgerLink.Ui.Features.Orders.Models
{
    public class Order
    {

        public Guid OrderId { get; set; }
        public List<string> OrderItemIds { get; set; }
        public string OrderName { get; set; }

        public static Order RandomGenerated()
        {
            var retval = new Order
            {
                OrderName = GenerateRandomString(5),
                OrderId = Guid.NewGuid(),
                OrderItemIds = new List<string>()
            };

            return retval;
        }

        private static string GenerateRandomString(int stringLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                randomString[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomString);
        }
    }
}
