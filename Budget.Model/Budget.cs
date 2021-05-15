using System;

namespace Budgets.Model
{
    public record Budget(Guid Id, uint Initial, string Name);
}