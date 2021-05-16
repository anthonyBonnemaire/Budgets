using System.Collections.Generic;
using System.Linq;

namespace Budgets.Core
{
    public class WalletRoot : IWallet
    {
        private List<IBudget> _Budgets = new List<IBudget>();
        public IEnumerable<IBudget> Budgets { get => _Budgets.AsReadOnly(); }
        public long WalletInitial { get => Budgets.Sum(b => b.BudgetInitial); }
        public double WalletActual { get => Budgets.Sum(b => b.BudgetActual); }

        public WalletRoot()
        {
            _Budgets = new List<IBudget>();
        }

        public Checker AddBudget(IBudget budget)
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

        public Checker<IBudget> DuplicateBudget(string name)
        {
            var budgetClone = _Budgets.FirstOrDefault(b => b.Name == name)?.CloneBudget();
           
            if (budgetClone == null)
                return Checker<IBudget>.CreateCheckerError("Nothing budget exist with the name so you can't duplicate");
           
            var numberAddToName = _Budgets.Count(b => b.Name.StartsWith(name)) - 1;
            budgetClone.ChangeName($"{name} Clone {numberAddToName}");
            
            AddBudget(budgetClone);
            return Checker<IBudget>.CreateCheckerValidWithValue(budgetClone);
        }

        public Checker ChangeBudgetName(string oldName, string newName)
        {
            var budgetWithOldName = Budgets.FirstOrDefault(b => b.Name == oldName);
            if (budgetWithOldName == null)
                return Checker.CreateCheckerError($"Nothing budget exist with this name ({oldName})");
            if (Budgets.Any(b => b != budgetWithOldName && b.Name == newName))
                return Checker.CreateCheckerError($"This name ({newName}) is always uses. So you cannot change the name.");


            return budgetWithOldName.ChangeName(newName);

        }
    }
}