using System.Collections.Generic;

namespace Budgets.Core
{
    public class WalletWithSaveService : IWallet
    {
        public IEnumerable<IBudget> Budgets => throw new System.NotImplementedException();

        public double WalletActual => throw new System.NotImplementedException();

        public long WalletInitial => throw new System.NotImplementedException();

        private IWallet _Wallet;
        public WalletWithSaveService(IWallet wallet)
        {
            _Wallet = wallet;
        }

        public Checker AddBudget(IBudget budget)
        {
            if (budget is BudgetWithSaveService)
                return _Wallet.AddBudget(budget);

            return Checker.CreateCheckerError("This budget is not the good type because you cannot save");
        }

        public Checker ChangeBudgetName(string oldName, string newName)
        {
            return _Wallet.ChangeBudgetName(oldName, newName);
        }

        public Checker<IBudget> DuplicateBudget(string name)
        {
            return _Wallet.DuplicateBudget(name);
        }
    }
}
