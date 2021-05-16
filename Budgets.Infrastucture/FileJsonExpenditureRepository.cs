using Budgets.Model;
using Budgets.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Budgets.Infrastucture
{
    public class FileJsonExpenditureRepository : IExpenditureRepository
    {
        private readonly string _FilePath;

        public FileJsonExpenditureRepository(string filePath)
        {
            _FilePath = filePath;
        }

        private IEnumerable<Expenditure> GetAllExpenditures()
        {
            var jsonBudgets = File.ReadAllText(_FilePath);
            return JsonSerializer.Deserialize<IEnumerable<Expenditure>>(jsonBudgets);
        }

        public IEnumerable<Expenditure> GetExpendituresByBudget(Guid idBudget)
        {
            var allExpenditures = GetAllExpenditures();
            return allExpenditures.Where(b => b.IdBudget == idBudget).ToList();
        }

        public void Save(IEnumerable<Expenditure> expenditures)
        {
            IEnumerable<Guid> idBudgets = expenditures.Select(e => e.IdBudget).Distinct();

            var allExpenditures = GetAllExpenditures();
            var expendituresRemove = allExpenditures.Where(e => idBudgets.Contains(e.IdBudget)).ToList();
            var expendituresSave = new List<Expenditure>(allExpenditures.Except(expendituresRemove));
            expendituresSave.AddRange(expenditures);

            var jsonExpenditures = JsonSerializer.Serialize(expendituresSave);
            File.WriteAllText(_FilePath, jsonExpenditures);
        }
    }
}
