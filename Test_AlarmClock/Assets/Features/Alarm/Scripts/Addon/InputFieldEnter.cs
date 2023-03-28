using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Alarm.Addon
{
    /// <summary>
    /// Класс для очистки InputField при взаимодействии с ним.
    /// </summary>
    public class InputFieldEnter : MonoBehaviour, IPointerClickHandler
    {
        private InputField inputField;

        private void Start()
        {
            inputField = gameObject.GetComponent<InputField>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            inputField.text = string.Empty;
        }
    }
}
