﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgets.Core
{
    public class Budget : ICloneable
    {
        public string Name { get; private set; }

        public uint BudgetInitial { get; private set; }

        private List<Expenditure> _Expenditures;
        public IEnumerable<Expenditure> Expenditures { get => _Expenditures.AsReadOnly(); }
        
        public double BudgetActual { get => BudgetInitial - SumExpendiure; }

        private double SumExpendiure {  get => Expenditures.Sum(e => e.Value); }

        public static readonly Budget BudgetEmpty = new Budget(0, "Budget par défaut"); 

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
                return Checker.CreateCheckerError("This expenditure is not indicated");

            if (string.IsNullOrWhiteSpace(expenditure.Name))
                return Checker.CreateCheckerError("This expenditure cannot added because the name is required.");

            _Expenditures.Add(expenditure);
            return Checker.CheckerValid;
        }

        public Checker ChangeInitialBudget(uint budgetInitial)
        {
            if ( (budgetInitial - SumExpendiure) < 0)
                return Checker.CreateCheckerError("You cannot change the budget because the budget actual want become negative.");

            BudgetInitial = budgetInitial;
            return Checker.CheckerValid;
        }

        public Checker RemoveExpenditure(Expenditure expenditure)
        {
            if(expenditure == null)
                return Checker.CreateCheckerError("This expenditure is not indicated");

            if (!_Expenditures.Contains(expenditure))
                return Checker.CreateCheckerError("This expenditure that you wanted you to remove does not exist in budget.");

            _Expenditures.Remove(expenditure);

            return Checker.CheckerValid;
        }

        public Checker<Expenditure> ReplaceExpenditure(Expenditure expenditureToReplace, Expenditure expenditureReplace)
        {
            if (expenditureToReplace == null || !_Expenditures.Contains(expenditureToReplace))
                return Checker<Expenditure>.CreateCheckerError("This expenditure that you wanted you to replace does not exist in budget.");

            if (expenditureReplace == null)
                return Checker<Expenditure>.CreateCheckerError("This expenditure is not indicated");

            var indexReplace = _Expenditures.IndexOf(expenditureToReplace);
            _Expenditures.RemoveAt(indexReplace);
            _Expenditures.Insert(indexReplace, expenditureReplace);

            return Checker<Expenditure>.CreateCheckerValidWithValue(expenditureReplace);
        }

        internal Checker ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Checker.CreateCheckerError("This name is not indicated");
            if (Name.Equals(name))
                return Checker.CreateCheckerError("This name is same");


            Name = name;

            return Checker.CheckerValid;
        }
    }
}