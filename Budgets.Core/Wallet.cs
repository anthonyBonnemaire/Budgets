using System.Collections.Generic;
using System.Linq;

namespace Budgets.Core
{
    public class Wallet
    {
        private List<Budget> _Budgets = new List<Budget>();
        public IEnumerable<Budget> Budgets { get => _Budgets.AsReadOnly(); }
        public long WalletInitial { get => Budgets.Sum(b => b.BudgetInitial); }
        public double WalletActual { get => Budgets.Sum(b => b.BudgetActual); }

        public Wallet()
        {
            _Budgets = new List<Budget>();
        }

        public Checker AddBudget(Budget budget)
        {
            if (budget == null)
                return Checker.CreateCheckerError("This budget does not exist.");

            if (!_Budgets.Contains(budget))
            {
                _Budgets.Add(budget);
                return Checker.CheckerValid;
            }

            return Checker.CreateCheckerError("This budget has already been added.");
        }

        public Checker<Budget> DuplicateBudget(string name)
        {
            var budgetClone = _Budgets.FirstOrDefault(b => b.Name == name)?.CloneBudget();
            if (budgetClone == null)
                return Checker<Budget>.CreateCheckerError("Nothing budget exist with the name so you can't duplicate");

            AddBudget(budgetClone);
            return Checker<Budget>.CreateCheckerValidWithValue(budgetClone);
        }
    }
}