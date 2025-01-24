using NSubstitute;
using NSubstituteAutoMocker;

namespace AutoMockerNSubstitute.UnitTests.DocumentationSnippets;

public class SavingsAccount(IInterestCalculator interestCalculator)
{
    public decimal Balance { get; private set; }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public void WithDraw(decimal amount)
    {
        Balance -= amount;
    }

    public void ApplyInterest()
    {
        Balance += interestCalculator.Calculate();
    }
}

public interface IInterestCalculator
{
    decimal Calculate();
}

public class SavingsAccountTests
{
    [Fact]
    public void ApplyInterestUpdatesTheBalance()
    {
        // Arrange
        IInterestCalculator interestCalculator = Substitute.For<IInterestCalculator>();
        interestCalculator.Calculate().Returns(123);
        SavingsAccount savingsAccount = new SavingsAccount(interestCalculator);

        // Act
        savingsAccount.ApplyInterest();

        // Assert
        Assert.Equal(123, savingsAccount.Balance);
    }
}

public class SavingsAccountTestsWithNSubstituteAutoMocker
{
    [Fact]
    public void ApplyInterestUpdatesTheBalance()
    {
        // Arrange
        var automocker = new NSubstituteAutoMocker<SavingsAccount>();
        automocker.Get<IInterestCalculator>().Calculate().Returns(123);

        // Act
        automocker.ClassUnderTest.ApplyInterest();

        // Assert
        Assert.Equal(123, automocker.ClassUnderTest.Balance);
    }
}
