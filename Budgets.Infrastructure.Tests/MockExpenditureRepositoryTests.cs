using Budgets.Infrastucture;
using Budgets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Budgets.Infrastructure.Tests
{
    public class MockExpenditureRepositoryTests
    {

        public MockExpenditureRepositoryTests()
        {
           
        }

        [Fact]
        public void GetExpendituresByBudget_WhenNotExistBudget()
        {
            var mockExpenditureRepository = new MockExpenditureRepository();
            Assert.Empty(mockExpenditureRepository.GetExpendituresByBudget(Guid.NewGuid()));
        }


        [Fact]
        public void GetExpendituresByBudget_ReturnEmpty()
        {
            var mockExpenditureRepository = new MockExpenditureRepository(null);
            Assert.Empty(mockExpenditureRepository.GetExpendituresByBudget(Guid.NewGuid()));
        }

        [Fact]
        public void GetExpendituresByBudget()
        {
            var idBudget = Guid.NewGuid();
            var expenditures = new List<Expenditure>
            {
                new Expenditure("name 1", 10, idBudget),
                new Expenditure("name 2", 50, idBudget),
                new Expenditure("name 3", 10, Guid.NewGuid()),
            };

            var mockExpenditureRepository = new MockExpenditureRepository(expenditures);

            var expendituresExpected = expenditures.Where(e => e.IdBudget == idBudget).ToList();

            var resultExpenditures = mockExpenditureRepository.GetExpendituresByBudget(idBudget);

            Assert.Equal(expendituresExpected, resultExpenditures);
        }
    }
}
