// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Microsoft.Extensions.Logging;

namespace PayrollExample;

public static class Logging
{
    public static ILogger<T> MakeLogger<T>() where T : class
    {
        var factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        return factory.CreateLogger<T>();
    }
}