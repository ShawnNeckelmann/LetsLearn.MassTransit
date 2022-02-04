namespace BurgerLink.Preparation.Activities.PrepareItem;

public interface IPrepareItemActivityArguments
{
    string ItemName { get; set; }

    string OrderName { get; set; }
}