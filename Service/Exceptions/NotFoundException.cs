namespace Service.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string type) : base($"The {type} could not be found")
    {
    }
}