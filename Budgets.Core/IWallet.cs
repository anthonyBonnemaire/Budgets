using System.Collections.Generic;

namespace Budgets.Core
{
    public interface IWallet
    {
        IEnumerable<IBudget> Budgets { get; }
        double WalletActual { get; }
        long WalletInitial { get; }

        Checker AddBudget(IBudget budget);
        Checker ChangeBudgetName(string oldName, string newName);
        Checker<IBudget> DuplicateBudget(string name);
    }
}