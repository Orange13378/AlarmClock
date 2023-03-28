using UnityEngine;

namespace Alarm.Controller
{
    using Abstraction;
    using Model;

    /// <summary>
    /// Класс точка входа для обработки логики часов.
    /// </summary>
    public class TimeController : AbstractAlarm
    {
        [SerializeField] private ArrowProvider ArrowsNTP, ArrowsWWW, ArrowsUnityWeb;
        [SerializeField] private GameObject alarmNotification;
        [SerializeField] private AudioClip alarmSound;

        private AudioSource audioSource;

        private Timer _timerNTP, _timerWWW, _timerUnityWeb;
        private bool _isActivated = false, _isVibrate = false;

        private void Start()
        {
            _timerNTP = gameObject.AddComponent<TimerNTP>();
            _timerWWW = gameObject.AddComponent<TimerWWW>();
            _timerUnityWeb = gameObject.AddComponent<TimerUnityWeb>();

            audioSource = Camera.main.GetComponent<AudioSource>();

            _timerNTP.Init(ArrowsNTP);
            _timerWWW.Init(ArrowsWWW);
            _timerUnityWeb.Init(ArrowsUnityWeb);

            ActivatedTimer();
        }

        private void Update()
        {
            if (!_isActivated)
                return;

            _timerNTP.UpdateTimer();
            _timerWWW.UpdateTimer();
            _timerUnityWeb.UpdateTimer();

            if (IsAlarmWorking)
            {
                _timerNTP.CheckAlarmTime();
                _timerWWW.CheckAlarmTime();
                _timerUnityWeb.CheckAlarmTime();
            }

            if (IsAlarmActive)
            {
                IsAlarmActive = false;
                _isVibrate = true;

                alarmNotification.SetActive(true);

                audioSource.clip = alarmSound;
                audioSource.Play();
            }

            if (_isVibrate)
            {
                Handheld.Vibrate();
                _isVibrate = false;
            }

        }

        /// <summary>
        /// Метод для обновления текущего времени.
        /// </summary>
        public void GetCurrentTime()
        {
            _timerNTP.GetCurrentTime();
            _timerWWW.GetCurrentTime();
            _timerUnityWeb.GetCurrentTime();
        }

        /// <summary>
        /// Публичный метод для включения таймера.
        /// </summary>
        public void ActivatedTimer()
        {
            _isActivated = true;
        }

        /// <summary>
        /// Публичный метод для выключения таймера.
        /// </summary>
        public void DeactivatedTimer()
        {
            _isActivated = false;
        }

        public void AlarmNotificationOFF()
        {
            alarmNotification.SetActive(false);
            audioSource.Stop();
            _isVibrate = false;
        }
    }
}
