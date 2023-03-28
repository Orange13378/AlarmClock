using UnityEngine;
using UnityEngine.UI;

namespace Alarm
{
    /// <summary>
    /// Класс посредник для передачи ссылок на стрелки часов.
    /// </summary>
    public class ArrowProvider : MonoBehaviour
    {
        public Transform hourArrow, minuteArrow, secondArrow;
        public Text timeText;
    }
}
