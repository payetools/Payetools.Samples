// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Microsoft.Extensions.Logging;

namespace Payetools.Samples.Common;

public static class Logging
{
    public static ILogger<T> MakeLogger<T>() where T : class =>
        LoggerFactory.Create(builder => builder.AddConsole())
        .CreateLogger<T>();
}