using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ProjInv.Application.UseCases.Investidor.Commands;
using ProjInv.Application.UseCases.Investidor;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;
using Serilog;

namespace ProjInv.Tests.Application
{
    public class GetAllInvestidoresHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IValidator<GetAllInvestidorCommand>> _validatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly GetAllInvestidorHandler _handler;

        public GetAllInvestidoresHandlerTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<GetAllInvestidorCommand>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new GetAllInvestidorHandler(_uowMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Deve_Listar_Investidores_Com_Sucesso()
        {
            var command = new GetAllInvestidorCommand { Page = 1, PageSize = 10 };
            var investidores = new List<Investidor> { new Investidor("Lucas"), new Investidor("Teste") };

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _uowMock.Setup(u => u.Investidores.GetAllAsync(command.Page, command.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(investidores);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Investidores.Should().HaveCount(2);
        }
    }
}
