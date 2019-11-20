using System;
using System.Threading.Tasks;
using Guide.Core.Boundaries.AcceptTheRules;
using Guide.Core.Exceptions;
using Guide.Core.UseCases;
using Xunit;

namespace Guide.Core.Tests.UseCases
{
    public class AcceptTheRulesTests
    {
        private readonly IAcceptTheRules _useCase;

        public AcceptTheRulesTests()
        {
            _useCase = new AcceptTheRules();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NullOrEmptyName_ShouldThrow(string name)
        {
            Assert.Throws<InvalidInputParameterException>(() => {
                new AcceptTheRulesInput(name, "Bio");
            });
        }

        [Fact]
        public async Task NullInput_ShouldThrow()
        {
            await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(null));
        }
    }
}
