using AutoMockerNSubstitute.UnitTests.SamplesToTest;
using NSubstituteAutoMocker;

namespace AutoMockerNSubstitute.UnitTests;

public class NSubstituteAutoMockerTests
{
    public class DefaultConstructor
    {
        [Fact]
        public void UsesDefaultConstructorIfAvailable()
        {
            NSubstituteAutoMocker<ClassWithJustDefaultConstructor> autoMocker = 
                new NSubstituteAutoMocker<ClassWithJustDefaultConstructor>();
            Assert.NotNull(autoMocker.ClassUnderTest);
        }

        [Fact]
        public void ThrowsExceptionIfPrivate()
        {
            Assert.Throws<ConstructorMatchException>(() => new NSubstituteAutoMocker<ClassWithPrivateDefaultConstructor>());
        }

        [Fact]
        public void ClassUnderTestCanBeSealed()
        {
            NSubstituteAutoMocker<SealedClass> autoMocker = new NSubstituteAutoMocker<SealedClass>();
            
            Assert.NotNull(autoMocker.ClassUnderTest);
        }

        [Fact]
        public void PrimitiveParameterTypesGetSetToDefaultValue()
        {
            NSubstituteAutoMocker<ClassWithPrimitiveConstructors> autoMocker = new NSubstituteAutoMocker<ClassWithPrimitiveConstructors>();
            
            Assert.Null(autoMocker.ClassUnderTest.IntValue);
            Assert.Null(autoMocker.ClassUnderTest.StringValue);
        }
    }

    public class ConstructorWithArgs
    {
        [Fact]
        public void UsesEquivalentConstructorIfAvailable()
        {
            NSubstituteAutoMocker<ClassWithAllConstructors> autoMocker = 
                new NSubstituteAutoMocker<ClassWithAllConstructors>([typeof(IDependency1), typeof(IDependency2)]);
            Assert.NotNull(autoMocker.ClassUnderTest);
            Assert.NotNull(autoMocker.ClassUnderTest.Dependency1);
            Assert.NotNull(autoMocker.ClassUnderTest.Dependency2);
        }

        [Fact]
        public void NullParameterUsesDefaultConstructor()
        {
            NSubstituteAutoMocker<ClassWithAllConstructors> autoMocker =
                new NSubstituteAutoMocker<ClassWithAllConstructors>(null, null);
            Assert.NotNull(autoMocker.ClassUnderTest);
        }

        [Fact]
        public void EmptyParameterUsesDefaultConstructor()
        {
            NSubstituteAutoMocker<ClassWithAllConstructors> autoMocker = new NSubstituteAutoMocker<ClassWithAllConstructors>([]);
            
            Assert.NotNull(autoMocker.ClassUnderTest); 
        }

        [Fact]
        public void ThrowsExceptionIfNoMatchAvailable()
        {
            Assert.Throws<ConstructorMatchException>(() => new NSubstituteAutoMocker<ClassWithAllConstructors>([typeof(IDependency2), typeof(IDependency1)]));
        }

        [Fact]
        public void CanOverrideParameterViaEvent()
        {
            NSubstituteAutoMocker<ClassWithAllConstructors> autoMocker =
                new NSubstituteAutoMocker<ClassWithAllConstructors>([typeof(IDependency1), typeof(IDependency2)],
                    (paramInfo, obj) =>
                    {
                        if (paramInfo.ParameterType == typeof(IDependency1))
                            return new Dependency1VitoImplementation();
                        return obj;
                    });

            Assert.IsAssignableFrom<Dependency1VitoImplementation>(autoMocker.ClassUnderTest.Dependency1);
        }

        [Fact]
        public void CanOverrideParameterViaEventAlt()
        {
            NSubstituteAutoMocker<ClassWithAllConstructors> autoMocker =
                new NSubstituteAutoMocker<ClassWithAllConstructors>((paramInfo, obj) =>
                    {
                        if (paramInfo.ParameterType == typeof(IDependency1))
                            return new Dependency1VitoImplementation();
                        return obj;
                    });

            Assert.IsAssignableFrom<Dependency1VitoImplementation>(autoMocker.ClassUnderTest.Dependency1);
        }
    }

    public class Get
    {
        [Fact]
        public void AvailableTypeReturned()
        {
            NSubstituteAutoMocker<ClassWithAllConstructors> autoMocker = new NSubstituteAutoMocker<ClassWithAllConstructors>();
            
            Assert.NotNull(autoMocker.Get<IDependency1>());
            Assert.NotNull(autoMocker.Get<IDependency2>());
        }

        [Fact]
        public void UnavailableTypeThrowsException()
        {
            NSubstituteAutoMocker<ClassWithAllConstructors> autoMocker = new NSubstituteAutoMocker<ClassWithAllConstructors>();

            Assert.Throws<ConstructorParameterNotFoundException>(() => autoMocker.Get<IOverdraft>());
        }

        [Fact]
        public void MultipleAvailableTypesThrowsExceptionIfNameNotSpecified()
        {
            NSubstituteAutoMocker<ClassWithDuplicateConstructorTypes> autoMocker = new NSubstituteAutoMocker<ClassWithDuplicateConstructorTypes>();

            Assert.Throws<ConstructorParameterNotFoundException>(() => autoMocker.Get<IDependency1>());
        }

        [Fact]
        public void MultipleAvailableTypesWithNameReturned()
        {
            NSubstituteAutoMocker<ClassWithDuplicateConstructorTypes> autoMocker = new NSubstituteAutoMocker<ClassWithDuplicateConstructorTypes>();
            
            Assert.NotNull(autoMocker.Get<IDependency1>("dependencyTwo"));
        }
    }

    // TODO Do I need to test the use of generics or collections?
    // TODO Do I need to test the visibility of classes/interfaces?
}
