using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Budgets.Core.Tests
{
    public class BudgetRootTests
    {

        [Theory]
        [InlineData((uint)500)]
        [InlineData((uint)900000)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        public void CreateBudgetAndCheckExpendituresExist(uint bugetInitial)
        {
            var budget = new BudgetRoot(bugetInitial, "Name");
            Assert.NotNull(budget.Expenditures);
            Assert.Equal(budget.BudgetInitial, budget.BudgetActual);
        }

        [Fact]
        public void AddOneExpenditure()
        {
            var budget = new BudgetRoot(11, "Name");
            var expenditure = new Expenditure("Name", 10.5);
            var checker = budget.AddExpenditure(expenditure);
            Assert.Equal(expenditure, budget.Expenditures.First());
            Assert.True(checker.IsValid);
            Assert.Equal(0.5, budget.BudgetActual);
        }


        [Fact]
        public void AddTwoExpenditure()
        {
            var budget = new BudgetRoot(0, "Name");
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
            var budget = new BudgetRoot(0, "Name");
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
            var budget = new BudgetRoot(0, "Name");

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
            var budget = new BudgetRoot(0, "Name");

            var expenditureOne = new Expenditure(name, 85);
            var checkerError = budget.AddExpenditure(expenditureOne);

            Assert.Empty(budget.Expenditures);
            Assert.False(checkerError.IsValid);
            Assert.Equal(0, budget.BudgetActual);
        }

        public static IEnumerable<object[]> GetNotExistsExpenditure()
        {
            yield return new object[]
            {
                new Expenditure("Name not exist", 10),
            };
            yield return new object[]
            {
                null,
            };
        }
 
        [Theory]
        [MemberData(nameof(GetNotExistsExpenditure))]
        public void RemoveExpenditure_NotExistExpenditure(Expenditure expenditure)
        {
            var budget = new BudgetRoot(0, "Name");
            budget.AddExpenditure(new Expenditure("Name", 20));

            var checkerError = budget.RemoveExpenditure(expenditure);

            Assert.Single(budget.Expenditures);
            Assert.False(checkerError.IsValid);
            Assert.Equal(-20, budget.BudgetActual);
        }

        [Fact]
        public void RemoveExpenditure_ExistExpenditure()
        {
            var budget = new BudgetRoot(100, "Name");

            var expenditureToRemove = new Expenditure("Name", 20);

            budget.AddExpenditure(expenditureToRemove);
            budget.AddExpenditure(new Expenditure("Name", 50));
            budget.AddExpenditure(expenditureToRemove);
            budget.AddExpenditure(new Expenditure("Name", 20));

            var checker = budget.RemoveExpenditure(expenditureToRemove);

            Assert.Equal(3, budget.Expenditures.Count());
            Assert.True(checker.IsValid);
            Assert.Equal(10, budget.BudgetActual);
        }

        [Fact]
        public void RemoveExpenditure_SameTwoExpenditures()
        {
            var budget = new BudgetRoot(100, "Name");

            var expenditureToRemove = new Expenditure("Name", 20);

            budget.AddExpenditure(expenditureToRemove);
            budget.AddExpenditure(new Expenditure("Name", 50));
            budget.AddExpenditure(expenditureToRemove);
            budget.AddExpenditure(new Expenditure("Name", 20));

            var checkerSuppGood = budget.RemoveExpenditure(expenditureToRemove);
            var checkerSuppGood2 = budget.RemoveExpenditure(expenditureToRemove);
            var checkerNotFound = budget.RemoveExpenditure(expenditureToRemove);

            Assert.Equal(2, budget.Expenditures.Count());
            Assert.True(checkerSuppGood.IsValid);
            Assert.True(checkerSuppGood2.IsValid);
            Assert.False(checkerNotFound.IsValid);
            Assert.Equal(30, budget.BudgetActual);
        }

        [Theory]
        [MemberData(nameof(GetNotExistsExpenditure))]
        public void ReplaceExpenditure_NotExistExpenditure(Expenditure expenditureToRemove)
        {
            var budget = new BudgetRoot(100, "Name");

            var expenditureReplace = new Expenditure("Name", 20);

            budget.AddExpenditure(new Expenditure("Name", 50));
            budget.AddExpenditure(expenditureReplace);
            budget.AddExpenditure(expenditureReplace);
            budget.AddExpenditure(new Expenditure("Name", 20));

            var checkerExpenditureReplace = budget.ReplaceExpenditure(expenditureToRemove, expenditureReplace);
            var checkerExpenditureNotIndicated = budget.ReplaceExpenditure(expenditureToRemove, null);


            Assert.Equal(4, budget.Expenditures.Count());
            Assert.False(checkerExpenditureReplace.IsValid);
            Assert.False(checkerExpenditureNotIndicated.IsValid);
            Assert.NotEqual(checkerExpenditureReplace.Result, expenditureReplace);
            Assert.NotEqual(checkerExpenditureReplace.Result, budget.Expenditures.ElementAt(1));
            Assert.Equal(-10, budget.BudgetActual);
        }

        [Fact]
        public void ReplaceExpenditure_Expenditure()
        {
            var budget = new BudgetRoot(100, "Name");

            var expenditureToRemove = new Expenditure("Name", 20);
            var expenditureReplace = new Expenditure("Name Replace", 10, expenditureToRemove.CreationDate);

            budget.AddExpenditure(new Expenditure("Name", 50));
            budget.AddExpenditure(expenditureToRemove);
            budget.AddExpenditure(expenditureToRemove);
            budget.AddExpenditure(new Expenditure("Name", 20));

            var checkerExpenditureReplace = budget.ReplaceExpenditure(expenditureToRemove, expenditureReplace);
            
            
            Assert.Equal(4, budget.Expenditures.Count());
            Assert.True(checkerExpenditureReplace.IsValid);
            Assert.Equal(checkerExpenditureReplace.Result, expenditureReplace);
            Assert.Equal(checkerExpenditureReplace.Result, budget.Expenditures.ElementAt(1));
            Assert.Equal(0, budget.BudgetActual);
        }

        [Fact]
        public void ChangeBudgetInitial_Ok()
        {
            BudgetRoot budget = new BudgetRoot(10, "Name 1");
            var checker = budget.ChangeInitialBudget(50);

            Assert.True(checker.IsValid);
            Assert.Equal((uint)50, budget.BudgetInitial);
        }

        [Fact]
        public void ChangeBudgetInitial_LessThanInitial()
        {
            BudgetRoot budget = new BudgetRoot(10, "Name 1");
            var checker = budget.ChangeInitialBudget(5);

            Assert.True(checker.IsValid);
            Assert.Equal((uint)5, budget.BudgetInitial);
        }

        [Fact]
        public void ChangeBudgetInitial_ButBudgetActualWantNegatif()
        {
            BudgetRoot budget = new BudgetRoot(10, "Name 1");
            budget.AddExpenditure(new Expenditure("name", 10));

            var checker = budget.ChangeInitialBudget(5);

            Assert.False(checker.IsValid);
            Assert.Equal((uint)10, budget.BudgetInitial);
        }
    }
}
