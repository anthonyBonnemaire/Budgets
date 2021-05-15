using Budgets.Model;
using System;
using System.Collections.Generic;

namespace Budgets.Ports
{
    public interface IExpenditureRepository
    {
        IEnumerable<Expenditure> GetExpendituresByBudget(Guid idBudget);
    }
}
