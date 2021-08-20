using Ascension.Attributes;
using Ascension.Utility;
using System;

namespace Ascension.Utility
{
    /// <summary>
    /// Range struct for Clamping/Range utility in <see cref="Single"/> value.
    /// </summary>
    [Serializable, Note(Enums.Dev.WaitWhatWolf, "Imported from the WarWolfWorks-x-Stride library.")]
    public struct FloatRange : IEquatable<FloatRange>
    {
        /// <summary>
        /// The minimal value of this <see cref="FloatRange"/>.
        /// </summary>
        public float Min;
        /// <summary>
        /// The maximal value of this <see cref="FloatRange"/>.
        /// </summary>
        public float Max;

        /// <summary>
        /// Returns the value given clamped between MinRange and MaxRange.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float GetClampedValue(float value)
        {
            return Hooks.MathF.Clamp(value, Min, Max);
        }

        /// <summary>
        /// Returns true if the given value is within Min (inclusive) and Max (inclusive).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsWithinRange(float value)
        {
            return value >= Min && value <= Max;
        }

        /// <summary>
        /// Returns a random value between <see cref="Min"/> (inclusive) and <see cref="Max"/> (inclusive).
        /// </summary>
        /// <returns></returns>
        public float GetRandom()
            => Hooks.Random.Range(Min, Max);
        
        /// <summary>
        /// Returns a <see cref="Tuple{T1, T2}"/> of <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <returns></returns>
        public (float min, float max) GetTupleRange() => (Min, Max);

        /// <summary>
        /// Returns true if both <see cref="FloatRange"/> values have the same <see cref="Min"/> and <see cref="Max"/> values.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(FloatRange other)
            => other.Min == Min && other.Max == Max;

        /// <summary>
        /// Creates a new <see cref="FloatRange"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public static implicit operator IntRange(FloatRange range)
            => new IntRange(Convert.ToInt32(range.Min), Convert.ToInt32(range.Max));

        public static FloatRange operator +(FloatRange f, FloatRange f2)
            => new FloatRange(f.Min + f2.Min, f.Max + f2.Max);
        public static FloatRange operator -(FloatRange f, FloatRange f2)
            => new FloatRange(f.Min - f2.Min, f.Max - f2.Max);

        public static bool operator ==(FloatRange f, FloatRange f2)
            => f.Equals(f2);

        public static bool operator !=(FloatRange f, FloatRange f2)
            => !f.Equals(f2);

        /// <summary>
        /// Returns true if the given object is a <see cref="FloatRange"/> and has the same <see cref="Min"/> and <see cref="Max"/> value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is FloatRange range)
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
