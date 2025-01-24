using System;

namespace NSubstituteAutoMocker;

public class ConstructorMatchException() : Exception(
    $"Constructor with supplied types cannot be found.{Environment.NewLine}This might be a result of a missing (or private) default constructor, or a miss match in the parameter list given.");
