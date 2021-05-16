namespace Budgets.Core
{
    public interface IBudgetFactory
    {
        IBudget CreateBudget(Model.Budget budgetModel);
    }
}