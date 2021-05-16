using System;
using System.Collections.Generic;

namespace Budgets.Core
{
    public interface IBudget : IEquatable<IBudget>
    {
        internal Guid Id { get; }
        double BudgetActual { get; }
        uint BudgetInitial { get; }
        IEnumerable<Expenditure> Expenditures { get; }
        string Name { get; }

        Checker AddExpenditure(Expenditure expenditure);
        Checker ChangeInitialBudget(uint budgetInitial);
        internal IBudget CloneBudget();
        Checker RemoveExpenditure(Expenditure expenditure);
        Checker<Expenditure> ReplaceExpenditure(Expenditure expenditureToReplace, Expenditure expenditureReplace);

        internal Checker ChangeName(string name);
    }
}