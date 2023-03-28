using UniRx;

namespace Alarm.Model
{
    using Abstraction;
    using UnityEngine;

    /// <summary>
    /// ����� ������ ����������.
    /// ����� ����� ��� InputField � ����� �� ���������.
    /// </summary>
    public class Alarm : AbstractAlarm
    {
        [SerializeField] protected ArrowProvider arrows;

        protected static int Hour { get; set; } = 6;
        protected static int Minute { get; set; } = 0;
        protected static int Second { get; set; } = 10;

        protected static BoolReactiveProperty IsDragged = new BoolReactiveProperty(false);

        protected static int PrevHour { get; set; }
        protected static int hourCoef = 0;
    }
}
