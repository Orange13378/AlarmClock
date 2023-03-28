using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alarm.Controller
{
    using Addon;
    using Model;

    /// <summary>
    /// Класс для взаимодействия со стрелкой будильника.
    /// </summary>
    public class ArrowMove : Alarm, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ArrowType arrowType;

        private float _angle;
        private Vector3 _endPoint;

        private void Start()
        {
            SetText();
        }

        public void OnDrag(PointerEventData eventData)
        {
            IsDragged.Value = true;

            _angle = (-CalculateAngle());

            SetArrows(_angle);

            CheckArrowType(arrowType);

            SetText();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragged.Value = false;
        }

        private void SetArrows(float angle)
        {
            _endPoint.z = angle;
            transform.eulerAngles = _endPoint;
        }

        private float CalculateAngle()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = (int)Vector2.SignedAngle(mousePosition, transform.position);
            
            angle = CheckCorrectAngle(angle);
            
            return angle;
        }

        private float CheckCorrectAngle(float angle)
        {
            if (angle >= 0)
                return 180 + angle;
            else
                return 180 - (int)Math.Abs(angle);
        }

        private void CheckArrowType(ArrowType arrowType)
        {
            if (arrowType == ArrowType.Hour)
            {
                Hour = (int)((_angle / -HOUR_RATIO));

                // Проверка на то прошла ли стрелка часов один оборот
                {
                    if (Hour % 12 < 3 && Hour % 12 >= 0 && PrevHour % 12 > 9 && PrevHour % 12 < 12)       
                    {
                        hourCoef++;
                    }
                    else if (Hour % 12 > 9 && Hour % 12 < 12 && PrevHour % 12 < 3 && PrevHour % 12 >= 0)
                    {
                        hourCoef--;
                    }
                }

                Hour = (Hour + Math.Abs(hourCoef) * 12) % 24;

                PrevHour = Hour;
            }
            else if (arrowType == ArrowType.Munute)
            {
                Minute = (int)((_angle / -MINUTE_OR_SECOND_RATIO) % 60);
            }
            else if (arrowType == ArrowType.Second)
            {
                Second = (int)((_angle / -MINUTE_OR_SECOND_RATIO) % 60);
            }
        }

        private void SetText()
        {
            arrows.timeText.text = $"{Hour:D2}:{Minute:D2}:{Second:D2}";
        }
    }
}
