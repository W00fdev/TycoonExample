using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
            if (money < 10000)
                return $"$ {money}";
            else if (money < 100000)
                return $"$ {money / 1000}k";
            else if (money < 100000000)
                return $"$ {money / 1000000}m";
            
            return $"$ {money}";
        }

        [Conditional("DEBUG_MODE")]
        public static void Print(string message, PrintStatus status = PrintStatus.SUCCESS)
        {
            Color color;
            
            switch (status)
            {
                case PrintStatus.SUCCESS:
                    color = Color.green;
                    break;
                case PrintStatus.FAILURE:
                    color = Color.red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
            
            var hexcolor = $"#{color.r:X2}{color.g:X2}{color.b:X2}";
            var coloredString = $"<color={hexcolor}>{message}</color>";
            Debug.Log(coloredString);
        }

        public enum PrintStatus { SUCCESS, FAILURE }
    }
}