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


        public IBudget CreateBudget(Budgets.Model.Budget budgetModel)
        {
            if (budgetModel == null)
                return BudgetRoot.BudgetEmpty;

            var budget = new BudgetRoot(budgetModel.Id, budgetModel.Initial, budgetModel.Name);

            var expendituresModels = _ExpenditureRepository.GetExpendituresByBudget(budgetModel.Id);
            if (expendituresModels == null)
                return budget;

            foreach(var expenditure in expendituresModels)
            {
                budget.AddExpenditure(new Expenditure(expenditure.Name, expenditure.Value, expenditure.CreationDate));
            }

            return budget;
        }
    }
}
