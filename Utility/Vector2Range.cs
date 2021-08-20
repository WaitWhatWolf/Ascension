using Ascension.Attributes;
using Microsoft.Xna.Framework;
using System;

namespace Ascension.Utility
{
    /// <summary>
    /// Range struct for Clamping/Range utility in <see cref="Single"/> value.
    /// </summary>
    [Serializable, Note(Enums.Dev.WaitWhatWolf, "Imported from the WarWolfWorks-x-Stride library.")]
    public struct Vector2Range : IEquatable<Vector2Range>
    {
        /// <summary>
        /// The minimal value of this <see cref="FloatRange"/>.
        /// </summary>
        public Vector2 Min;
        /// <summary>
        /// The maximal value of this <see cref="FloatRange"/>.
        /// </summary>
        public Vector2 Max;

        /// <summary>
        /// Returns the value given clamped between MinRange and MaxRange.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Vector2 GetClampedValue(Vector2 value)
        {
            return Hooks.MathF.Clamp(value, Min, Max);
        }

        /// <summary>
        /// Returns true if the given value is within Min (inclusive) and Max (inclusive).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsWithinRange(Vector2 value)
        {
            bool xWithin = value.X >= Min.X && value.X <= Max.X;
            return xWithin && value.Y >= Min.Y && value.Y <= Max.Y;
        }

        /// <summary>
        /// Returns a random value between <see cref="Min"/> (inclusive) and <see cref="Max"/> (inclusive).
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRandom()
            => Hooks.Random.Range(Min, Max);
        
        /// <summary>
        /// Returns a <see cref="Tuple{T1, T2}"/> of <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <returns></returns>
        public (Vector2 min, Vector2 max) GetTupleRange() => (Min, Max);

        /// <summary>
        /// Returns true if both <see cref="FloatRange"/> values have the same <see cref="Min"/> and <see cref="Max"/> values.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Vector2Range other)
            => other.Min == Min && other.Max == Max;

        /// <summary>
        /// Creates a new <see cref="FloatRange"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Vector2Range(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Creates a new <see cref="FloatRange"/>.
        /// </summary>
        public Vector2Range(float minX, float minY, float maxX, float maxY)
        {
            Min = new Vector2(minX, minY);
            Max = new Vector2(maxX, maxY);
        }

        //TODO: Convert to Vector3Range
        //public static implicit operator IntRange(FloatRange range)
        //    => new IntRange(Convert.ToInt32(range.Min), Convert.ToInt32(range.Max));

        public static Vector2Range operator +(Vector2Range f, Vector2Range f2)
            => new(f.Min + f2.Min, f.Max + f2.Max);
        public static Vector2Range operator -(Vector2Range f, Vector2Range f2)
            => new(f.Min - f2.Min, f.Max - f2.Max);

        public static bool operator ==(Vector2Range f, Vector2Range f2)
            => f.Equals(f2);

        public static bool operator !=(Vector2Range f, Vector2Range f2)
            => !f.Equals(f2);

        /// <summary>
        /// Returns true if the given object is a <see cref="FloatRange"/> and has the same <see cref="Min"/> and <see cref="Max"/> value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector2Range range)
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
