using System.Linq;
using Xunit;

namespace Budgets.Core.Tests
{
    public class WalletTests
    {
        private readonly Wallet _Wallet;

        public WalletTests()
        {
            _Wallet = new Wallet();
        }

        [Fact]
        public void CreateWalletsAndCheckBudgetsExist()
        {
            Assert.NotNull(_Wallet.Budgets);
        }

        [Fact]
        public void AddOneBudget()
        {
            var budget = new Budget(100, "Name 1"); ;
            var checker = _Wallet.AddBudget(budget);
            Assert.Equal(budget, _Wallet.Budgets.First());
            Assert.True(checker.IsValid);
            Assert.Equal(100, _Wallet.WalletInitial);
            Assert.Equal(100, _Wallet.WalletActual);
        }


        [Fact]
        public void AddTwoBudget()
        {
            var budgetOne = new Budget(0, "Name 1");
            var checkerOne = _Wallet.AddBudget(budgetOne);

            var budgetTwo = new Budget(100, "Name 2");
            var checkerTwo = _Wallet.AddBudget(budgetTwo);

            Assert.Equal(2, _Wallet.Budgets.Count());
            Assert.Equal(budgetOne, _Wallet.Budgets.ElementAt(0));
            Assert.Equal(budgetTwo, _Wallet.Budgets.ElementAt(1));
            Assert.True(checkerOne.IsValid);
            Assert.True(checkerTwo.IsValid);
            Assert.Equal(100, _Wallet.WalletInitial);
            Assert.Equal(100, _Wallet.WalletActual);
        }

        [Fact]
        public void AddSameBudget()
        {
            var budgetOne = new Budget(10, "Name 1"); ;
            var checkerGoodOne = _Wallet.AddBudget(budgetOne);
            var checkerFailBecauseExist = _Wallet.AddBudget(budgetOne);

            Assert.Single(_Wallet.Budgets);
            Assert.Equal(budgetOne, _Wallet.Budgets.ElementAt(0));
            Assert.True(checkerGoodOne.IsValid);
            Assert.False(checkerFailBecauseExist.IsValid);
            Assert.Equal(10, _Wallet.WalletInitial);
            Assert.Equal(10, _Wallet.WalletActual);
        }

        [Fact]
        public void AddBudgetWithSameCategory()
        {
            var budgetOne = new Budget(10, "Name 1");
            var checkerGoodOne = _Wallet.AddBudget(budgetOne);
            var checkerFailBecauseExist = _Wallet.AddBudget(budgetOne);

            Assert.Single(_Wallet.Budgets);
            Assert.Equal(budgetOne, _Wallet.Budgets.ElementAt(0));
            Assert.True(checkerGoodOne.IsValid);
            Assert.False(checkerFailBecauseExist.IsValid);
            Assert.Equal(10, _Wallet.WalletInitial);
            Assert.Equal(10, _Wallet.WalletActual);
        }


        [Fact]
        public void DuplicateBudgetExist()
        {
            var budgetOne = new Budget( 10, "Name 1");
            var budgetTwo = new Budget( 50, "Name 2");
            _Wallet.AddBudget(budgetOne);
            _Wallet.AddBudget(budgetTwo);

            var checkerWithBudgetClone = _Wallet.DuplicateBudget(budgetOne.Name);

            Assert.Equal(3, _Wallet.Budgets.Count());
            Assert.Equal(checkerWithBudgetClone.Result, _Wallet.Budgets.ElementAt(2));
            Assert.Equal(checkerWithBudgetClone.Result.Name, budgetOne.Name);
            Assert.NotSame(checkerWithBudgetClone.Result, budgetOne);
            Assert.Equal(70, _Wallet.WalletInitial);
            Assert.Equal(70, _Wallet.WalletActual);

            Assert.True(checkerWithBudgetClone.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Budget Not Exist")]
        [InlineData(null)]
        public void DuplicateBudgetNotExist(string nameNotExist)
        {
            var budgetOne = new Budget(50, "Name 1");
            var budgetTwo = new Budget(900, "Name 2");
            _Wallet.AddBudget(budgetOne);
            _Wallet.AddBudget(budgetTwo);

            var checkerWithBudgetClone = _Wallet.DuplicateBudget(nameNotExist);

            Assert.Equal(2, _Wallet.Budgets.Count());
            Assert.False(checkerWithBudgetClone.IsValid);
            Assert.Equal(950, _Wallet.WalletInitial);
            Assert.Equal(950, _Wallet.WalletActual);
        }


        [Fact]
        public void CalcultateWalletActual()
        {
            var budgetOne = new Budget(50, "Name 1");
            var budgetTwo = new Budget(900, "Name 2");

            budgetOne.AddExpenditure(new Expenditure("Name", 10));
            budgetOne.AddExpenditure(new Expenditure("Name", 20));

            _Wallet.AddBudget(budgetOne);
            _Wallet.AddBudget(budgetTwo);

            budgetTwo.AddExpenditure(new Expenditure("Name 10", 200));
            budgetTwo.AddExpenditure(new Expenditure("Name 20", 500));
            budgetTwo.AddExpenditure(new Expenditure("Name 20", 210));

            Assert.Equal(950, _Wallet.WalletInitial);
            Assert.Equal(10, _Wallet.WalletActual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        [InlineData("Name 1")]
        [InlineData("Name 2")]
        public void ChangeBudgetName_NotRun(string nameNotRun)
        {
            Budget budget = new Budget(10, "Name 1");
            Budget budget2 = new Budget(10, "Name 2");
            _Wallet.AddBudget(budget);
            _Wallet.AddBudget(budget2);

            var checker = _Wallet.ChangeBudgetName("Name 1", nameNotRun);

            Assert.False(checker.IsValid);
            Assert.Equal("Name 1", budget.Name);
        }

        [Fact]
        public void ChangeBudgetName_Run()
        {
            Budget budget = new Budget(10, "Name 1");
            _Wallet.AddBudget(budget);

            var checker = _Wallet.ChangeBudgetName("Name 1", "Change Name 1");

            Assert.True(checker.IsValid);
            Assert.Equal("Change Name 1", budget.Name);
        }
    }
}
