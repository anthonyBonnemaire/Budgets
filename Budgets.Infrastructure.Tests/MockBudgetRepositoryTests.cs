using Budgets.Infrastucture;
using Xunit;

namespace Budgets.Infrastructure.Tests
{
    public class MockBudgetRepositoryTests
    {

        [Fact]
        public void GetBudgets_ReturnNotEmpty()
        {
            var mockBudgetRepository = new MockBudgetRepository();
            Assert.NotEmpty(mockBudgetRepository.GetBudgets());
        }

        [Fact]
        public void GetBudgets_ReturnEmpty()
        {
            var mockBudgetRepository = new MockBudgetRepository(null);
            Assert.Empty(mockBudgetRepository.GetBudgets());
        }
    }
}
