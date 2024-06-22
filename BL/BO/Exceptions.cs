namespace BO;



public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
               : base(message, innerException) { }

}

public class InvalidInputException : Exception
{
    public InvalidInputException(string? message) : base(message) { }

}
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
    public BlDeletionImpossible(string message, Exception innerException)
              : base(message, innerException) { }
}

public class BlNoDatesForProject:Exception
{
    public BlNoDatesForProject(string? message) : base(message) { }
}
public class BlInvalidDataInTheSchedule : Exception
{
    public BlInvalidDataInTheSchedule(string? message) : base(message) { }
}