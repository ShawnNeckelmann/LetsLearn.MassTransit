using BurgerLink.Preparation.Contracts.Commands;
using MassTransit;

namespace BurgerLink.Preparation.Activities.PrepareItem;

public class PrepareItemActivity : IExecuteActivity<IPrepareItemActivityArguments>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<IPrepareItemActivityArguments> context)
    {
        var item = $"prepared-{context.Arguments.ItemName}";

        await context.Publish<ItemPrepared>(new
        {
            PreparedOrderItem = item,
            context.Arguments.OrderName
        });

        return context.Completed();
    }
}