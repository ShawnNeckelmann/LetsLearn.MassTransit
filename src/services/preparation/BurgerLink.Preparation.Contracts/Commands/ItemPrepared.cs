namespace BurgerLink.Preparation.Contracts.Commands
{
    public interface ItemPrepared
    {
        public string PreparedOrderItem { get; set; }
        public string OrderName { get; set; }
    }
}