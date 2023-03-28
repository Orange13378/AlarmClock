using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Alarm.Controller
{
    using Model;

    /// <summary>
    /// Класс контроллера будильника.
    /// </summary>
    public class AlarmController : Alarm
    {
        [SerializeField] protected InputField hourField, minuteField, secondField;

        private void Start()
        {
            SetTimeInField();

            Observable.EveryFixedUpdate().Select(x => IsDragged.Value).
                Subscribe(SetInputTime).AddTo(this);

            hourField.onEndEdit.AddListener(text =>
            {
                Hour = SetTime(text, hourField, 24);
                
                if (Hour >= 12)
                {
                    PrevHour = Hour;
                    hourCoef = 1;
                }
                else
                {
                    PrevHour = Hour;
                    hourCoef = 0;
                }

                SetArrows(arrows.hourArrow, Hour, true);
            });

            minuteField.onEndEdit.AddListener(text =>
            {
                Minute = SetTime(text, minuteField, 60);
                SetArrows(arrows.minuteArrow, Minute);
            });

            secondField.onEndEdit.AddListener(text =>
            {
                Second = SetTime(text, secondField, 60);
                SetArrows(arrows.secondArrow, Second);
            });
        }

        private void OnDestroy()
        {
            hourField.onEndEdit.RemoveAllListeners();
            minuteField.onEndEdit.RemoveAllListeners();
            secondField.onEndEdit.RemoveAllListeners();
        }

        private void SetInputTime(bool isDragged)
        {
            if (!isDragged)
                return;

            SetTimeInField();
        }

        public void SetTimeInField()
        {
            hourField.text = Hour.ToString("D2");
            minuteField.text = Minute.ToString("D2");
            secondField.text = Second.ToString("D2");
        }

        private void SetArrows(Transform arrow, int time, bool isHour = false)
        {
            if (isHour)
                arrow.localRotation = Quaternion.Euler(0f, 0f, time * (-HOUR_RATIO));
            else
                arrow.localRotation = Quaternion.Euler(0f, 0f, time * (-MINUTE_OR_SECOND_RATIO));

            SetText();
        }

        private void SetText()
        {
            arrows.timeText.text = $"{Hour:D2}:{Minute:D2}:{Second:D2}";
        }

        private int SetTime(string text, InputField inputField, int max = 60, int min = 0)
        {
            int result = 0;
            try
            {
                result = CheckCorrectInput(text, inputField, max, min);
                return result;
            }
            catch (Exception)
            {
                inputField.text = 0.ToString("D2");
                PrevHour = result;
                hourCoef = 0;
                return result;
            }
        }

        private int CheckCorrectInput(string text, InputField inputField, int max, int min)
        {
            int number = int.Parse(text);
            inputField.text = number.ToString("D2");

            if (number >= max || number < min)
            {
                number = min;
                inputField.text = number.ToString("D2");
            }

            return number;
        }

        /// <summary>
        /// Метод для кнопки ON.
        /// </summary>
        public void ButtonON()
        {
            AlarmHour = Hour;
            AlarmMinute = Minute;
            AlarmSecond = Second;

            IsAlarmWorking = true;
        }

        /// <summary>
        /// Метод для кнопки OFF.
        /// </summary>
        public void ButtonOFF()
        {
            IsAlarmWorking = false;
        }

    }
}

