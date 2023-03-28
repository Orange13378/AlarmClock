using UnityEngine;
using UnityEngine.UI;

namespace Alarm.Buttons
{
    using Model;

    /// <summary>
    /// Класс для смены состояния кнопок.
    /// </summary>
    public class ButtonControl : Alarm
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        private Button _button;

        private void Start()
        {
            _button = gameObject.GetComponent<Button>();
            
            _button.onClick.AddListener(EnableButton);
            _button.onClick.AddListener(DisableButton);

            OnStartAlarm += DisableButton;
            OnStartAlarm += EnableButton;
            OnStartAlarm += DisableImage;
        }

        private void OnDestroy()
        {
            OnStartAlarm -= DisableButton;
            OnStartAlarm -= EnableButton;
            OnStartAlarm -= DisableImage;
        }

        private void DisableButton()
        {
            gameObject.SetActive(false);
        }

        private void EnableButton()
        {
            button.gameObject.SetActive(true);
        }

        private void DisableImage()
        {
            image.gameObject.SetActive(false);
        }
    }
}
