using Budgets.Ports;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Budgets.Core.Tests
{
    public class BudgetFactoryTests
    {
        private readonly BudgetFactory _BudgetFactory;
        private readonly Mock<IExpenditureRepository> _MockExpenditureRepository;

        public BudgetFactoryTests()
        {
            _MockExpenditureRepository = new Mock<IExpenditureRepository>(MockBehavior.Strict);
            _BudgetFactory = new BudgetFactory(_MockExpenditureRepository.Object);
        }

        [Fact]
        public void CreateBudget_NotSetBudgetAndReturnEmpty()
        {
            var budgetEmpty = _BudgetFactory.CreateBudget(null);
            Assert.Equal(Budget.BudgetEmpty, budgetEmpty);
        }

        [Fact]
        public void CreateBudget_AndReturnTheBudgetWithEmptyExpenditures()
        {
            var expectedBudget = new Budget(10, "Name 1");

            _MockExpenditureRepository.Setup(s => s.GetExpendituresByBudget(It.IsAny<Guid>()))
                .Returns<IEnumerable<Model.Expenditure>>(null);
            
            var resultBudget = _BudgetFactory.CreateBudget(new Budgets.Model.Budget(Guid.NewGuid(), 10, "Name 1"));

            Assert.Equal(expectedBudget.BudgetActual, resultBudget.BudgetActual);
            Assert.Equal(expectedBudget.BudgetInitial, resultBudget.BudgetInitial);
            Assert.Equal(expectedBudget.Expenditures, resultBudget.Expenditures);
            Assert.Empty(resultBudget.Expenditures);
        }


        [Fact]
        public void CreateBudget_AndReturnTheBudgetWithExpenditures()
        {
            var idBudget = Guid.NewGuid();

            var expectedExpenditures = new List<Model.Expenditure>
            {
                new Model.Expenditure("Name 1", 10,idBudget),
                new Model.Expenditure("Name 2", 3, idBudget),
            };

            var expectedBudget = new Budget(10, "Name 1");
            foreach (var expenditure in expectedExpenditures)
                expectedBudget.AddExpenditure(new Expenditure(expenditure.Name, expenditure.Value));

            _MockExpenditureRepository.Setup(s => s.GetExpendituresByBudget(It.IsAny<Guid>()))
                .Returns(expectedExpenditures);

            var resultBudget = _BudgetFactory.CreateBudget(new Budgets.Model.Budget(idBudget, 10, "Name 1"));

            Assert.Equal(expectedBudget.BudgetActual, resultBudget.BudgetActual);
            Assert.Equal(expectedBudget.BudgetInitial, resultBudget.BudgetInitial);
            Assert.Equal(expectedBudget.Expenditures, resultBudget.Expenditures);
        }
    }
}
