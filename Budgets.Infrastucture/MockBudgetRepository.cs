using Budgets.Model;
using Budgets.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgets.Infrastucture
{
    public class MockBudgetRepository : IBudgetRepository
    {
        internal static Guid IdBudgetEssence = Guid.NewGuid();
        internal static Guid IdBudgetLoisir = Guid.NewGuid();
        internal static Guid IdBudgetRestaurant = Guid.NewGuid();
        internal static Guid IdBudgetNourriture = Guid.NewGuid();

        private static readonly List<Budget> _BudgetsStatic = new List<Budget>
        {
            new Budget(IdBudgetEssence, 50, "Budget Essence"),
            new Budget(IdBudgetLoisir, 250, "Budget Loisir"),
            new Budget(IdBudgetRestaurant, 100, "Budget Restaurant"),
            new Budget(IdBudgetNourriture, 500, "Budget Nourriture"),
        };

        private List<Budget> _Budgets;

        public MockBudgetRepository(IEnumerable<Budget> budgets)
        {
            _Budgets = budgets?.ToList();
        }

        public MockBudgetRepository()
        {
            _Budgets = _BudgetsStatic;
        }

        public IEnumerable<Budget> GetBudgets()
        {
            return _Budgets?.AsReadOnly() ?? new List<Budget>().AsReadOnly();
        }

        public void Save(IEnumerable<Budget> budgets)
        {
        }

        public void Save(Budget budget)
        {
        }
    }
}
