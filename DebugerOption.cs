using System;
using UnityEngine;

namespace BordlessFramework
{
    public class DebugerOption
    {
        internal DebugerOptionType type;
        internal Color? color;

        internal DebugerOption(DebugerOptionType type)
        {
            this.type = type;
        }

        internal DebugerOption(DebugerOptionType type, Color color)
        {
            this.type = type;
            this.color = color;
        }

        public static DebugerOption Color(float r, float g, float b)
        {
            return Color(r, g, b, 1f);
        }

        public static DebugerOption Color(float r, float g, float b, float a)
        {
            return Color(new Color(r, g, b, a));
        }

        public static DebugerOption Color(Color color)
        {
            return new DebugerOption(DebugerOptionType.Color, color);
        }

        public static DebugerOption Bold { get; } = new DebugerOption(DebugerOptionType.Bold);

        public static DebugerOption Italic { get; } = new DebugerOption(DebugerOptionType.Italic);

        internal enum DebugerOptionType { Color, Bold, Italic, }
    }
}
