using System;
using UnityEngine;

namespace Alarm.Abstraction
{
    /// <summary>
    /// Абстрактный класс будильника.
    /// </summary>
    public abstract class AbstractAlarm : MonoBehaviour
    {
        protected const float HOUR_RATIO = 360f / 12f;
        protected const float MINUTE_OR_SECOND_RATIO = 360f / 60f;

        protected static int AlarmHour { get; set; }
        protected static int AlarmMinute { get; set; }
        protected static int AlarmSecond { get; set; }

        protected static bool IsAlarmWorking { get; set; } = false;
        protected static bool IsAlarmActive { get; set; } = false;

        protected static Action OnStartAlarm = delegate { };
    }
}
