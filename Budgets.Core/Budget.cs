using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgets.Core
{
    public class Budget : ICloneable
    {
        public string Name { get; private init; }

        public uint BudgetInitial { get; private init; }

        private List<Expenditure> _Expenditures;
        public IEnumerable<Expenditure> Expenditures { get => _Expenditures.AsReadOnly(); }
        
        public double BudgetActual { get => BudgetInitial - Expenditures.Sum(e => e.Value); }

        public Budget(uint budgetInitial, string name)
        {
            _Expenditures = new List<Expenditure>();
            BudgetInitial = budgetInitial;
            Name = name;
        }

        public object Clone() => MemberwiseClone();

        public Budget CloneBudget() => Clone() as Budget;

        public Checker AddExpenditure(Expenditure expenditure)
        {
            if (expenditure == null)
                return Checker.CreateCheckerError("This expenditure does not exist.");

            if (string.IsNullOrWhiteSpace(expenditure.Name))
                return Checker.CreateCheckerError("This expenditure cannot added because the name is required.");

            _Expenditures.Add(expenditure);
            return Checker.CheckerValid;
        }
    }
}