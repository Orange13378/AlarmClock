using System;
using UnityEngine;

namespace Alarm.Abstraction
{
    /// <summary>
    /// Абстрактый класс таймера, выполняющий логику вычисления и отображения стрелок часов.
    /// </summary>
    public abstract class Timer : AbstractAlarm
    {
        private float _timeHoursClock, _timeMinutesClock, _timeSecondsClock;
        private float _timeHoursText, _timeMinutesText, _timeSecondsText;
        private float _hourTMP, _minuteTMP, _secondTMP;

        protected DateTime CurrentTime { get; set; }

        protected ArrowProvider CurrentArrow { get; set; }

        protected abstract DateTime GetTime();

        /// <summary>
        /// Метод инициализации.
        /// </summary>
        /// <param name="currentArrow">Объект у которого ссылки на все стрелки часов.</param>
        public void Init(ArrowProvider currentArrow)
        {
            CurrentArrow = currentArrow;
            GetCurrentTime();
        }

        /// <summary>
        /// Метод обновления таймера вызываемый в MonoBehaviour Update().
        /// </summary>
        public void UpdateTimer()
        {
            AddTime();

            CheckCorrectText();

            SetArrowsRotation();

            SetText();
        }

        /// <summary>
        /// Метод который запрашивает время с разных сервисов.
        /// </summary>
        public void GetCurrentTime()
        {
            CurrentTime = GetTime();

            _timeHoursText = _hourTMP = CurrentTime.Hour;
            _timeMinutesText = _minuteTMP = CurrentTime.Minute;
            _secondTMP = CurrentTime.Second - Time.time;
        }

        private void AddTime()
        {
            _timeHoursClock = (_hourTMP + (Time.time / 3600)) % 24;
            _timeMinutesClock = (_minuteTMP + (Time.time / 60)) % 60;
            _timeSecondsClock = _secondTMP + Time.time;

            _timeSecondsText = _timeSecondsClock;
        }

        private void CheckCorrectText()
        {
            if (_timeSecondsText >= 60)
            {
                _timeMinutesText++;
                _secondTMP = 0 - Time.time;
            }
            else
                return;

            if (_timeMinutesText >= 60)
            {
                GetCurrentTime(); // Обновление каждый час
                _timeHoursText++;
                _timeMinutesText = 0;
            }
            else
                return;

            if (_timeHoursText >= 24)
            {
                _timeHoursText = 0;
            }
        }

        private void SetArrowsRotation()
        {
            CurrentArrow.hourArrow.localRotation = Quaternion.Euler(0f, 0f, _timeHoursClock * (-HOUR_RATIO));
            CurrentArrow.minuteArrow.localRotation = Quaternion.Euler(0f, 0f, _timeMinutesClock * (-MINUTE_OR_SECOND_RATIO));
            CurrentArrow.secondArrow.localRotation = Quaternion.Euler(0f, 0f, _timeSecondsClock * (-MINUTE_OR_SECOND_RATIO));
        }

        private void SetText()
        {
            CurrentArrow.timeText.text = $"{(int)_timeHoursText:D2}:{(int)_timeMinutesText:D2}:{(int)_timeSecondsText:D2}";
        }

        /// <summary>
        /// Метод проверки наступило ли время указанное на будильнике, в случае если он был включен.
        /// </summary>
        public void CheckAlarmTime()
        {
            if ((int)_timeSecondsText == AlarmSecond &&
                (int)_timeMinutesText == AlarmMinute &&
                (int)_timeHoursText == AlarmHour &&
                IsAlarmWorking)
            {
                IsAlarmWorking = false;
                IsAlarmActive = true;
                OnStartAlarm.Invoke();
            }
        }
    }
}
