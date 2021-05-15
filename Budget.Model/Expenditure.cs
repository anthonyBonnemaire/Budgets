using System;

namespace Budgets.Model
{
    public record Expenditure(string Name, double Value, Guid IdBudget);
}