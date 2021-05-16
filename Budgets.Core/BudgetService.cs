using Budgets.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgets.Core
{
    public class BudgetService
    {
        private readonly IBudgetRepository _BudgetRepository;
        private readonly IBudgetFactory _BudgetModelFactory;

        public BudgetService(IBudgetRepository budgetRepository,
            IBudgetFactory budgetFactory)
        {
            _BudgetRepository = budgetRepository ?? throw new ArgumentNullException(nameof(budgetRepository));
            _BudgetModelFactory = budgetFactory ?? throw new ArgumentNullException(nameof(budgetFactory));
        }

        public IEnumerable<IBudget> GetBudgets()
        {
            return _BudgetRepository.GetBudgets()
                ?.Select( bm => _BudgetModelFactory.CreateBudget(bm))?.ToList() ?? new List<IBudget>();
        }
    }
}
