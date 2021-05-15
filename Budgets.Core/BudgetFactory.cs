using Budgets.Ports;
using System;

namespace Budgets.Core
{
    public class BudgetFactory : IBudgetFactory
    {

        private readonly IExpenditureRepository _ExpenditureRepository;


        public BudgetFactory(IExpenditureRepository expenditureRepository)
        {
            _ExpenditureRepository = expenditureRepository ?? throw new ArgumentNullException(nameof(expenditureRepository));
        }


        public Budget CreateBudget(Budgets.Model.Budget budgetModel)
        {
            if (budgetModel == null)
                return Budget.BudgetEmpty;

            var budget = new Budget(budgetModel.Initial, budgetModel.Name);

            var expendituresModels = _ExpenditureRepository.GetExpendituresByBudget(budgetModel.Id);
            if (expendituresModels == null)
                return budget;

            foreach(var expenditure in expendituresModels)
            {
                budget.AddExpenditure(new Expenditure(expenditure.Name, expenditure.Value));
            }

            return budget;
        }
    }
}
