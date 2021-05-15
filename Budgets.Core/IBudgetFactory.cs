namespace Budgets.Core
{
    public interface IBudgetFactory
    {
        Budget CreateBudget(Model.Budget budgetModel);
    }
}