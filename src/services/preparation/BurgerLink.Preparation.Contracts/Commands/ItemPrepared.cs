namespace BurgerLink.Preparation.Contracts.Commands;

public interface ItemPrepared
{
    public string OrderName { get; set; }
    public string PreparedOrderItem { get; set; }
}