using Budgets.Model;
using Budgets.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgets.Infrastucture
{
    public class MockExpenditureRepository : IExpenditureRepository
    {
        internal static readonly List<Expenditure> _ExpendituresStatic = new List<Expenditure>
        {
            new Expenditure("Essence Leclerc", 50, MockBudgetRepository.IdBudgetEssence),
            new Expenditure("Leclerc", 50, MockBudgetRepository.IdBudgetNourriture),
            new Expenditure("Auchan", 200.3, MockBudgetRepository.IdBudgetNourriture),
            new Expenditure("Auchan", 100.52, MockBudgetRepository.IdBudgetNourriture),
            new Expenditure("Auchan", 100.52, MockBudgetRepository.IdBudgetNourriture),
            new Expenditure("Sushi", 60, MockBudgetRepository.IdBudgetRestaurant),
            new Expenditure("Bowling", 10, MockBudgetRepository.IdBudgetLoisir),
            new Expenditure("Bowling", 15, MockBudgetRepository.IdBudgetLoisir),
            new Expenditure("Bowling", 10, MockBudgetRepository.IdBudgetLoisir),
        };

        private IEnumerable<Expenditure> _Expenditures;

        public MockExpenditureRepository(IEnumerable<Expenditure> expenditures)
        {
            _Expenditures = expenditures?.ToList();
        }

        public MockExpenditureRepository()
        {
            _Expenditures = _ExpendituresStatic;
        }

        public IEnumerable<Expenditure> GetExpendituresByBudget(Guid idBudget)
        {
            return _Expenditures?.Where(e => e.IdBudget == idBudget)?.ToList()
                ?? new List<Expenditure>();
        }
    }
}
