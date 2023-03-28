using System;
using System.Threading;
using System.Diagnostics;
using UnityEngine;

namespace Alarm.Model
{
    using Abstraction;

    /// <summary>
    /// Класс таймера реализующий способ получения времени с сервиса по типу google.com через WWW.
    /// </summary>
    public class TimerWWW : Timer
    {
        private readonly string url = "google.com";

        protected override DateTime GetTime()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var www = new WWW(url);
            while (!www.isDone && www.error == null)
            {
                Thread.Sleep(1);
            }

            string timeStr = www.responseHeaders["Date"];

            if (!DateTime.TryParse(timeStr, out DateTime globDateTime))
            {
                return DateTime.MinValue;
            }

            www.Dispose();
            stopwatch.Stop();

            return globDateTime.ToLocalTime().AddMilliseconds(-stopwatch.ElapsedMilliseconds);
        }
    }
}
