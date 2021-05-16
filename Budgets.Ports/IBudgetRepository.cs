using Budgets.Model;
using System.Collections.Generic;

namespace Budgets.Ports
{
    public interface IBudgetRepository
    {
        IEnumerable<Budget> GetBudgets();

        void Save(IEnumerable<Budget> budgets);
        void Save(Budget budget);
    }
}
