using Ascension.Attributes;
using Ascension.Utility;
using System;

namespace Ascension.Utility
{
    /// <summary>
    /// Range struct for Clamping/Range utility in <see cref="Int32"/> value.
    /// </summary>
    [Serializable, Note(Enums.Dev.WaitWhatWolf, "Imported from the WarWolfWorks-x-Stride library.")]
    public struct IntRange : IEquatable<IntRange>
    {
        /// <summary>
        /// The minimal value of this <see cref="IntRange"/>.
        /// </summary>
        public int Min;
        /// <summary>
        /// The maximal value of this <see cref="IntRange"/>.
        /// </summary>
        public int Max;

        /// <summary>
        /// Returns the value given clamped between <see cref="Min"/> (inclusive) and <see cref="Max"/> (exclusive).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetClampedValue(int value)
        {
            return Hooks.MathF.Clamp(value, Min, Max - 1);
        }

        /// <summary>
        /// Returns true if the given value is within <see cref="Min"/> (inclusive) and <see cref="Max"/> (exclusive).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsWithinRange(int value)
        {
            return value >= Min && value < Max;
        }

        /// <summary>
        /// Returns a random value between <see cref="Min"/> (inclusive) and <see cref="Max"/> (exclusive).
        /// </summary>
        /// <returns></returns>
        public int GetRandom()
            => Hooks.Random.Range(Min, Max);

        /// <summary>
        /// Returns a <see cref="Tuple{T1, T2}"/> of <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <returns></returns>
        public (int min, int max) GetTupleRange() => (Min, Max);

        /// <summary>
        /// Returns true if both <see cref="FloatRange"/> values have the same <see cref="Min"/> and <see cref="Max"/> values.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IntRange other)
        {
            return other.Min == Min && other.Max == Max;
        }

        /// <summary>
        /// Creates a new <see cref="IntRange"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Implicitly converts this <see cref="IntRange"/> into a <see cref="FloatRange"/>.
        /// </summary>
        /// <param name="range"></param>
        public static implicit operator FloatRange(IntRange range)
            => new FloatRange(range.Min, range.Max);

        public static IntRange operator +(IntRange i, IntRange i2)
            => new IntRange(i.Min + i2.Min, i.Max + i2.Max);
        public static IntRange operator -(IntRange i, IntRange i2)
            => new IntRange(i.Min - i2.Min, i.Max - i2.Max);

        public static bool operator ==(IntRange i, IntRange i2)
            => i.Equals(i2);

        public static bool operator !=(IntRange i, IntRange i2)
            => !i.Equals(i2);

        /// <summary>
        /// Returns true if the given object is a <see cref="IntRange"/> and has the same <see cref="Min"/> and <see cref="Max"/> value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is IntRange range)
                return Equals(range);

            return false;
        }

        /// <summary>
        /// Returns the combine hashcode value of <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }
    }
}
