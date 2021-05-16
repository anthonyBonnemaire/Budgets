using System;

namespace Budgets.Core
{
    public record Expenditure(string Name, double Value)
    {
        public Expenditure(string name, double value, DateTime creationDate) : this(name,value)
        {
            _CreationDate = creationDate;
        }

        private DateTime _CreationDate = DateTime.Now;
        public DateTime CreationDate { get => _CreationDate; }
    }
}