﻿namespace AutoMockerNSubstitute.UnitTests.SamplesToTest;

public class ClassWithPrimitiveConstructors
{
    public ClassWithPrimitiveConstructors()
    {
    }

    public ClassWithPrimitiveConstructors(IDependency1 dependency1, IDependency2 dependency2, string stringValue, int intValue)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }

    public IDependency1? Dependency1 { get; set; }

    public IDependency2? Dependency2 { get; set; }

    public string? StringValue { get; set; }

    public int? IntValue { get; set; }
}