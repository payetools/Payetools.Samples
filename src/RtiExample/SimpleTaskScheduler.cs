// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Hmrc.Rti.Model;

namespace RtiExample;

internal class SimpleTaskScheduler : ITaskScheduler
{
    public async Task ScheduleTask(Action task, TimeSpan periodFromNow)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(periodFromNow);
            task();
        });
    }
}