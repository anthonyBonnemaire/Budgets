using System.Linq;
using Xunit;

namespace Budgets.Core.Tests
{
    public class BudgetTests
    {

        [Theory]
        [InlineData((uint)500)]
        [InlineData((uint)900000)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        public void CreateBudgetAndCheckExpendituresExist(uint bugetInitial)
        {
            var budget = new Budget(bugetInitial, "Name");
            Assert.NotNull(budget.Expenditures);
            Assert.Equal(budget.BudgetInitial, budget.BudgetActual);
        }

        [Fact]
        public void AddOneExpenditure()
        {
            var budget = new Budget(11, "Name");
            var expenditure = new Expenditure("Name", 10.5);
            var checker = budget.AddExpenditure(expenditure);
            Assert.Equal(expenditure, budget.Expenditures.First());
            Assert.True(checker.IsValid);
            Assert.Equal(0.5, budget.BudgetActual);
        }


        [Fact]
        public void AddTwoExpenditure()
        {
            var budget = new Budget(0, "Name");
            var expenditureOne = new Expenditure("Name", 1);
            var checkerOne = budget.AddExpenditure(expenditureOne);

            var expenditureTwo = new Expenditure("Name", 1);
            var checkerTwo = budget.AddExpenditure(expenditureTwo);

            Assert.Equal(2, budget.Expenditures.Count());
            Assert.Equal(expenditureOne, budget.Expenditures.ElementAt(0));
            Assert.Equal(expenditureTwo, budget.Expenditures.ElementAt(1));
            Assert.True(checkerOne.IsValid);
            Assert.True(checkerTwo.IsValid);
            Assert.Equal(-2, budget.BudgetActual);
        }

        [Fact]
        public void AddSameExpenditure()
        {
            var budget = new Budget(0, "Name");
            var expenditureOne = new Expenditure("Name", 85);
            var checkerGoodOne = budget.AddExpenditure(expenditureOne);
            var checkerGoodTwo = budget.AddExpenditure(expenditureOne);

            Assert.Equal(2, budget.Expenditures.Count());
            Assert.Equal(expenditureOne, budget.Expenditures.ElementAt(0));
            Assert.Equal(expenditureOne, budget.Expenditures.ElementAt(1));
            Assert.True(checkerGoodOne.IsValid);
            Assert.True(checkerGoodTwo.IsValid);
            Assert.Equal(-170, budget.BudgetActual);
        }

        [Fact]
        public void AddExpenditure_NotExistExpenditure()
        {
            var budget = new Budget(0, "Name");

            var checkerError = budget.AddExpenditure(null);

            Assert.Empty(budget.Expenditures);
            Assert.False(checkerError.IsValid);
            Assert.Equal(0, budget.BudgetActual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddExpenditureWithoutName(string name)
        {
            var budget = new Budget(0, "Name");

            var expenditureOne = new Expenditure(name, 85);
            var checkerError = budget.AddExpenditure(expenditureOne);

            Assert.Empty(budget.Expenditures);
            Assert.False(checkerError.IsValid);
            Assert.Equal(0, budget.BudgetActual);
        }
    }
}
