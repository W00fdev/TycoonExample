using UnityEngine;

namespace _Project.Scripts.Utils
{
    public static class Utils
    {
        public static float InverseLerp(this Vector3 value, Vector3 a, Vector3 b)
        {
            Vector3 ab = b - a;
            Vector3 av = value - a;
            return Mathf.Clamp01(Vector3.Dot(av, ab) / Vector3.Dot(ab, ab));
        }

        public static string ToSpeedFormat(this float value)
        {
            return Mathf.RoundToInt(value * 10f).ToString();
        }
        
        public static string ToHeaderMoneyFormat(this int money)
        {
            return $"$ {money}";
        }

    }
}