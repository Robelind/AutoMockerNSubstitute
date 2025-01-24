using System;

namespace NSubstituteAutoMocker;

public class ConstructorParameterNotFoundException() : Exception("The requested parameter type was not used in the original call to the constructor");

