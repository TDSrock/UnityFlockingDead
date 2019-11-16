using UnityEngine;

namespace SjorsGielen.CustomVariables.RangeVariables
{
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public MinMaxRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
        public float Min { get; private set; }
        public float Max { get; private set; }

        public MinMaxRangeAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}