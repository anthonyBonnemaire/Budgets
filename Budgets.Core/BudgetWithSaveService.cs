using Budgets.Model;
using Budgets.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgets.Core
{
    public class BudgetWithSaveService : IBudget
    {
        Guid IBudget.Id => _Budget.Id;
        public double BudgetActual => _Budget.BudgetActual;

        public uint BudgetInitial => _Budget.BudgetInitial;

        public IEnumerable<Expenditure> Expenditures => _Budget.Expenditures;

        public string Name => _Budget.Name;

        private readonly IBudgetRepository _BudgetRepository;
        private readonly IExpenditureRepository _ExpenditureRepository;
        private readonly IBudget _Budget;
        public BudgetWithSaveService(IBudgetRepository budgetRepository, IExpenditureRepository expenditureRepository, IBudget budget)
        {
            _BudgetRepository = budgetRepository;
            _ExpenditureRepository = expenditureRepository;
            _Budget = budget;
        }

        public Checker AddExpenditure(Expenditure expenditure)
        {
            var checker = _Budget.AddExpenditure(expenditure);
            SaveIfCheckerIsValid(checker);
            return checker;
        }

        private void SaveIfCheckerIsValid(Checker checker)
        {
            if (checker.IsValid)
                Save();
        }

        private void Save()
        {
            _BudgetRepository.Save(new Budget(_Budget.Id, BudgetInitial, Name));
            var expedituresSave = _Budget.Expenditures
                .Select(e => new Model.Expenditure(e.Name, e.Value, _Budget.Id, e.CreationDate)).ToList();
            _ExpenditureRepository.Save(expedituresSave);
        }

        public Checker ChangeInitialBudget(uint budgetInitial)
        {
            var checker = _Budget.ChangeInitialBudget(budgetInitial);
            SaveIfCheckerIsValid(checker);
            return checker;
        }

        IBudget IBudget.CloneBudget()
        {
            var budgetClone = _Budget.CloneBudget();
            Save();
            return budgetClone;
        }

        public Checker RemoveExpenditure(Expenditure expenditure)
        {
            var checker = _Budget.RemoveExpenditure(expenditure);
            SaveIfCheckerIsValid(checker);
            return checker;
        }

        public Checker<Expenditure> ReplaceExpenditure(Expenditure expenditureToReplace, Expenditure expenditureReplace)
        {
            var checker = _Budget.ReplaceExpenditure(expenditureToReplace, expenditureReplace);
            SaveIfCheckerIsValid(checker);
            return checker;
        }

        Checker IBudget.ChangeName(string name)
        {
            var checker = _Budget.ChangeName(name);
            SaveIfCheckerIsValid(checker);
            return checker;
        }

        public bool Equals(IBudget other)
        {
            return _Budget.Equals(other);
        }
    }
}
