using Budgets.Model;
using Budgets.Ports;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Budgets.Infrastucture
{
    public class FileJsonBudgetRepository : IBudgetRepository
    {
        private readonly string _FilePath;

        public FileJsonBudgetRepository(string filePath)
        {
            _FilePath = filePath;
        }

        public IEnumerable<Budget> GetBudgets()
        {
            var jsonBudgets = File.ReadAllText(_FilePath);
            return JsonSerializer.Deserialize<IEnumerable<Budget>>(jsonBudgets);
        }

        public void Save(IEnumerable<Budget> budgets)
        {
            var jsonBudgets = JsonSerializer.Serialize(budgets);
            File.WriteAllText(_FilePath, jsonBudgets);
        }


        public void Save(Budget budget)
        {
            List<Budget> budgets = new List<Budget>(GetBudgets());

            var changeBudget = budgets.FirstOrDefault(b => b.Id == budget.Id);
            if (changeBudget != null)
            {
                var indexChange = budgets.IndexOf(changeBudget);
                budgets.Remove(changeBudget);
                budgets.Insert(indexChange, budget);
            }

            var jsonBudgets = JsonSerializer.Serialize(budgets);
            File.WriteAllText(_FilePath, jsonBudgets);
        }
    }
}
