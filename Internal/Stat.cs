using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using System;
using System.Collections.Generic;

namespace Ascension.Internal
{
    /// <summary>
    /// Base class used for all <see cref="Nyu"/> statistics.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public sealed class Stat : IStat, IEquatable<Stat>
    {
        /// <summary>
        /// Gets or sets the value of this stat.
        /// </summary>
        public float Value
        {
            get => pv_Value;
            set => pv_Value = value;
        }

        /// <summary>
        /// How this <see cref="Stat"/> is calculated by a <see cref="INyuStacking"/>.
        /// </summary>
        public int Stacking
        {
            get => pv_Stacking;
            set => pv_Stacking = value;
        }

        /// <summary>
        /// Stats with which this <see cref="Stat"/> will interact with.
        /// </summary>
        public int[] Affections
        {
            get => pv_Affections;
            set => pv_Affections = value;
        }


        /// <summary>
        /// Create a Stat.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stacking"></param>
        /// <param name="affections"></param>
        public Stat(float value, int stacking, params int[] affections)
        {
            this.pv_Value = value;
            this.pv_Stacking = stacking;
            this.pv_Affections = affections;
        }

        /// <summary>
        /// Creates a duplicate of the given stat.
        /// </summary>
        /// <param name="stat"></param>
        public Stat(Stat stat)
        {
            pv_Value = stat.pv_Value;
            pv_Stacking = stat.pv_Stacking;
            pv_Affections = stat.pv_Affections;
        }

        /// <summary>
        /// Creates a duplicate of the given stat with a different value.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="value"></param>
        public Stat(Stat stat, float value)
        {
            this.pv_Value = value;
            pv_Stacking = stat.pv_Stacking;
            pv_Affections = stat.pv_Affections;
        }

        /// <summary>
        /// Returns the value in string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string baseText = $"{pv_Value}|{Stacking}|";
            for (int i = 0; i < Affections.Length; i++)
            {
                baseText += $"{Affections[i]}";
                if (i != Affections.Length - 1)
                    baseText += ',';
            }
            return baseText;
        }

        private float pv_Value;
        private int pv_Stacking;
        private int[] pv_Affections;

        /// <summary>
        /// Returns the Stat's value implicitly.
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator float(Stat s) => s.pv_Value;
        /// <summary>
        /// Returns the Stat's value implicitly as int.
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator int(Stat s) => (int)s.pv_Value;

        void IStat.OnAdded(EntityStats to) { }
        void IStat.OnRemoved(EntityStats to) { }

        /// <summary>
        /// Returns true if this <see cref="Stat"/> is equal to another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Stat other)
        {
            return other != null &&
                   Value == other.Value &&
                   Stacking == other.Stacking &&
                   EqualityComparer<int[]>.Default.Equals(Affections, other.Affections);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = -1432848062;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Stacking.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(Affections);
            return hashCode;
        }
    }
}
