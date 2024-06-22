namespace BlApi;

// Factory class for creating an instance of the business logic layer.
public static class Factory
{
    // Gets an instance of the business logic layer.
    // Returns: An instance of the business logic layer.
    public static IBl Get() => new BlImplementation.Bl();
}
