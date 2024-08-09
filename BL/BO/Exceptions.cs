namespace BO;
[Serializable]

public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
           : base(message, innerException) { }
}

public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
    public BlNullPropertyException(string message, Exception innerException)
          : base(message, innerException) { }
}

public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
              : base(message, innerException) { }
}

public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
    public BlDeletionImpossible(string message, Exception innerException)
              : base(message, innerException) { }
}

public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
}


public class BlFailedToRead : Exception
{
    public BlFailedToRead(string? message) : base(message) { }
    public BlFailedToRead(string message, Exception innerException)
              : base(message, innerException) { }
}
public class BlFailedToUpdate : Exception
{
    public BlFailedToUpdate(string? message) : base(message) { }
    public BlFailedToUpdate(string message, Exception innerException)
              : base(message, innerException) { }

}
public class BlFailedToDelete : Exception
{
    public BlFailedToDelete(string? message) : base(message) { }
    public BlFailedToDelete(string message, Exception innerException)
              : base(message, innerException) { }

}

public class BlFailedToCreate : Exception
{
    public BlFailedToCreate(string? message) : base(message) { }
    public BlFailedToCreate(string message, Exception innerException)
              : base(message, innerException) { }

}

public class BlFailedToReadAll : Exception
{
    public BlFailedToReadAll(string? message) : base(message) { }
    public BlFailedToReadAll(string message, Exception innerException)
              : base(message, innerException) { }

}

public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }
    public BlInvalidDataException(string message, Exception innerException)
              : base(message, innerException) { }
}
