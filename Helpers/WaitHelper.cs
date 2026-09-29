using System.Diagnostics;
using Microsoft.Playwright;
using PlaywrightAutomationDemo.Config;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightAutomationDemo.Helpers;

public static class WaitHelper
{
   public static async Task ForReadyAsync(ILocator locator, int? timeoutMs = null)
   {
       await Expect(locator).ToBeVisibleAsync(new() { Timeout = timeoutMs });
       await Expect(locator).ToBeEnabledAsync(new() { Timeout = timeoutMs });
   }
   
    // Polls a non-UI condition (DB record, API status) until it's met or times out
    public static async Task<T> UntilAsync<T>(
        Func<Task<T>> action, Func<T, bool> condition,
        int timeoutMs = TestConfig.DefaultWaitMs, int pollMs = 500)
    {
        var timer = Stopwatch.StartNew();
        while (true)
        {
            var result = await action();
            if (condition(result)) return result;
            if (timer.ElapsedMilliseconds > timeoutMs)
                throw new TimeoutException($"Condition not met within {timeoutMs} ms");
            await Task.Delay(pollMs);
        }
    }
}