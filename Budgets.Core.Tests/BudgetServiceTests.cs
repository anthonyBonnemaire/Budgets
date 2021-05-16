using Budgets.Ports;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Budgets.Core.Tests
{
    public class BudgetServiceTests
    {
        private readonly BudgetService _BudgetService;
        private readonly Mock<IBudgetFactory> _MockBudgetFactory;
        private readonly Mock<IBudgetRepository> _MockBudgetRepository;

        public BudgetServiceTests()
        {
            _MockBudgetFactory = new Mock<IBudgetFactory>(MockBehavior.Strict);
            _MockBudgetRepository = new Mock<IBudgetRepository>(MockBehavior.Strict);
            _BudgetService = new BudgetService(_MockBudgetRepository.Object, _MockBudgetFactory.Object);
        }

        [Fact]
        public void GetBudgets_ReturnEmpty()
        {
            _MockBudgetRepository.Setup(s => s.GetBudgets()).Returns<IEnumerable<BudgetRoot>>(null);
            _MockBudgetFactory.Setup(s => s.CreateBudget(It.IsAny<Model.Budget>())).Returns<BudgetRoot>(null);

            var budgetEmpty = _BudgetService.GetBudgets();
            Assert.Empty(budgetEmpty);
        }

        [Fact]
        public void GetBudgets_Ok()
        {
            List<Model.Budget> budgets = new List<Model.Budget>
            {
                new Model.Budget(Guid.NewGuid(), 10,"B1"),
                new Model.Budget(Guid.NewGuid(),15,"B3"),
                new Model.Budget(Guid.NewGuid(),500,"B4"),
            };

            _MockBudgetRepository.Setup(s => s.GetBudgets()).Returns(budgets);
            _MockBudgetFactory.Setup(s => s.CreateBudget(It.IsAny<Model.Budget>())).Returns(BudgetRoot.BudgetEmpty);

            var resultBudgets = _BudgetService.GetBudgets();
            Assert.Equal(3, resultBudgets.Count());
        }
    }
}
