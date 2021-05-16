using Budgets.Ports;
using System;

namespace Budgets.Core
{
    public class BudgetFactoryWithSave : IBudgetFactory
    {
        private readonly IBudgetFactory _BudgetFactory;
        private readonly IBudgetRepository _BudgetRepository;
        private readonly IExpenditureRepository _ExpenditureRepository;
        public BudgetFactoryWithSave(IBudgetFactory budgetFactory, IBudgetRepository budgetRepository, IExpenditureRepository expenditureRepository)
        {
            _BudgetFactory = budgetFactory;
            _BudgetRepository = budgetRepository;
            _ExpenditureRepository = expenditureRepository;
        }


        public IBudget CreateBudget(Budgets.Model.Budget budgetModel)
        {
            if (budgetModel == null)
                return BudgetRoot.BudgetEmpty;

            var budget = new BudgetWithSaveService(_BudgetRepository,_ExpenditureRepository, _BudgetFactory?.CreateBudget(budgetModel));

            return budget;
        }
    }
}
