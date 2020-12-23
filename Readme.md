# Important

* Using application configurations as strongly typed objects and also getting the most updated settings without reloading the application.

```CSharp

// In the Startup class

services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfig"));

// Register the dependency as a `scoped` or a `transient` but use the `IOptionsSnapshot<T>` rather than `IOptions<T>`
services.AddScoped(provider =>
{
    var config = provider.GetRequiredService<IOptionsSnapshot<DatabaseConfig>>().Value;
    return config;
});

```