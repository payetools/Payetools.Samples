// This example code may be freely used without restriction - it may be freely copied, adapted and
// used without attribution.
//
// Note however that the libraries it relies upon are copyright (c) 2023-2024, Payetools Foundation,
// licensed under the MIT License or commercial licence terms as set out in the documentation.

using Payetools.Hmrc.Common.Dps;
using Payetools.Hmrc.Dps;
using Payetools.Hmrc.Dps.Model.Messages;

namespace DpsExample;

internal class DpsMessageProcessor : IHmrcDpsMessageProcessor
{
    public bool NotifyWhenNoMessagesAvailable => true;

    public Task ProcessCodingNoticeP9sAsync(IEnumerable<CodingNoticeP9> codingNoticeEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in codingNoticeEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessCodingNoticeP6P6BsAsync(IEnumerable<CodingNoticesP6P6B> codingNoticeEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in codingNoticeEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessIncentiveLettersAsync(IEnumerable<IncentiveLetter> incentiveEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in incentiveEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessP11DbNotifsAsync(IEnumerable<P11DbNotif> p11DbNotifEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in p11DbNotifEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    [Obsolete]
    public Task ProcessP35NotifsAsync(IEnumerable<P35Notif> p35NotifEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in p35NotifEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessPostgraduateLoanStartsAsync(IEnumerable<PostgraduateLoanStart> postGraduateLoanStartEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in postGraduateLoanStartEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessPostgraduateLoanStopsAsync(IEnumerable<PostgraduateLoanStop> postGraduateLoanEndEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in postGraduateLoanEndEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessReminderARsAsync(IEnumerable<ReminderAR> reminderAREvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in reminderAREvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessReminderARmnsAsync(IEnumerable<ReminderARmn> reminderARmnEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in reminderARmnEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessRTINotsAsync(IEnumerable<RTINot> rtiNotifEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var codingNoticeEvent in rtiNotifEvents)
            Console.WriteLine($"{dpsMessageType}/{codingNoticeEvent.GetType().Name}: {codingNoticeEvent.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessStudentLoanStopsAsync(IEnumerable<StudentLoanEnd> studentLoanEndEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in studentLoanEndEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessStudentLoanStartsAsync(IEnumerable<StudentLoanStart> studentLoanStarts, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var codingNoticeEvent in studentLoanStarts)
            Console.WriteLine($"{dpsMessageType}/{codingNoticeEvent.GetType().Name}: {codingNoticeEvent.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task ProcessGenericNotificationsAsync(IEnumerable<GenericNotification> genericNotificationEvents, DpsMessageType dpsMessageType, uint highWaterMark)
    {
        foreach (var evt in genericNotificationEvents)
            Console.WriteLine($"{dpsMessageType}/{evt.GetType().Name}: {evt.SequenceNumber}");
        return Task.CompletedTask;
    }

    public Task NoMessagesForMessageType(DpsMessageType dpsMessageType, uint highWaterMark)
    {
        Console.WriteLine($"No messages for {dpsMessageType}: {highWaterMark}");
        return Task.CompletedTask;
    }
}
