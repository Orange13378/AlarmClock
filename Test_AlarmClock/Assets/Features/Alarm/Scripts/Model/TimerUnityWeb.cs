using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine.Networking;

namespace Alarm.Model
{
    using Abstraction;

    /// <summary>
    /// Класс таймера реализующий способ получения времени с сервиса по типу microsoft.com через UnityWebRequest.
    /// </summary>
    public class TimerUnityWeb : Timer
    {
        private readonly string url = "microsoft.com";

        protected override DateTime GetTime()
        {
            StartCoroutine(nameof(GetUnityTime));
            return CurrentTime;
        }

        IEnumerator GetUnityTime()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                request.SendWebRequest();

                while (!request.isDone)
                {
                    Thread.Sleep(1);
                }

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    print(request.error);
                    stopwatch.Stop();
                }
                else
                {
                    string timeStr = request.GetResponseHeader("Date");

                    if (!DateTime.TryParse(timeStr, out DateTime globDateTime))
                    {
                        print("MinValue: " + DateTime.MinValue);
                        stopwatch.Stop();
                    }

                    CurrentTime = globDateTime.ToLocalTime().AddMilliseconds(-stopwatch.ElapsedMilliseconds);
                    stopwatch.Stop();
                    request.Dispose();
                }
            }
            yield return CurrentTime;
        }
    }
}
