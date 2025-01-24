This repo was forked from [NSubstituteAutoMocker](https://github.com/kevin-mcmanus/NSubstituteAutoMocker), which seems quite dead, and is updated to be able to target relevant frameworks.

NSubstituteAutoMocker
=====================

Put very simply NSubstitueAutoMocker is a very lightweight extension to the NSubstitute mocking framework.  Whilst there may be other uses, its primary function is to further simplify unit testing and create a class under test, whose own constructor may contain dependencies that you wish to mock but don't want the hasstle to set up and maintain over time.

Quick start
-----------

Imagine the following class needed some testing (yes, arguably the tests should come first if your following true TDD):


    public class SavingsAccount
    {
        private readonly IInterestCalculator _interestCalculator;

        public SavingsAccount(IInterestCalculator interestCalculator)
        {
            _interestCalculator = interestCalculator;
        }

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
            Balance += _interestCalculator.Calculate();
        }
    }

    public interface IInterestCalculator
    {
        decimal Calculate();
    }


A typical unit test using the NSubstitute mocking framework might look something similar to that below:

    [TestClass]
    public class SavingsAccountTests
    {
        [TestMethod]
        public void ApplyInterestUpdatesTheBalance()
        {
            // Arange
            IInterestCalculator interestCalculator = Substitute.For<IInterestCalculator>();
            interestCalculator.Calculate().Returns(123);
            SavingsAccount savingsAccount = new SavingsAccount(interestCalculator);

            // Act
            savingsAccount.ApplyInterest();

            // Assert
            Assert.AreEqual(123, savingsAccount.Balance);
        }
    }

Using NSubstituteAutoMocker the test can be simplifed to the following:

    [TestClass]
    public class SavingsAccountTestsWithNSubstituteAutoMocker
    {
        [TestMethod]
        public void ApplyInterestUpdatesTheBalance()
        {
            // Arange
            var automocker = new NSubstituteAutoMocker<SavingsAccount>();
            automocker.Get<IInterestCalculator>().Calculate().Returns(123);

            // Act
            automocker.ClassUnderTest.ApplyInterest();

            // Assert
            Assert.AreEqual(123, automocker.ClassUnderTest.Balance);
        }
    }

The key difference is that constructor dependency has not been explicitly defined.  Whilst this might not seem significant (we've only reduced our line count be one), imagine the scenario of a class with a large constructor parameter count.  Not only would this result in additional code in the first test above, it will also create a maintainance nightmare when a developer changes the signature (e.g. the addition of a new argument).  Such a change would result in the update of all tests on the class, for which there may be many.  This would be true even for tests that don't need the dependency.  As a direct consequence of using the AutoMocker, the test code becomes significantly easier to write and also maintain, focusing the developer on the functionality being testest and not of the wiring of test objects.

As the parameters were not explicitly defined in the test, their access mechanism is through the Get<T>() method, where T is the type of the parameter you are requesting.  In the case of multiple parameters of the same type, an optional string parameter can be used to specify the parameter name.

Under the hood the automocker simply makes a call out to NSubstitute for each of the parameters it finds on the constructor.  In the event that the class under test contains multiple constructors, the constructor to the automocker allows you to supply a type list of arguments.  When using the default constructor, the automockers behavoir is to locate the class with the highest number of parameters (or the first items it finds in the result of a conflict).

There may be scenarios where you want to override the autogenerated parameters.  This might be to inject something new, or to supply additional properties that are not initialised by default.  This can be achieved through the Func<> parameter on the automockers constructor.  Please be mindful that this only allows you to override the constructor parameters to the class under test, there is no mechanism to override dependency graphs (if you find yourself needing to do this, it could be a testability/design code smell too).


Final note
----------

This extension has been built with unit testing in mind.  Paying tribute to NSubstitute it has been designed to keep things simple.  There are much more powerful automockers that make use of Dependency Injection containers, this is not one of them.
