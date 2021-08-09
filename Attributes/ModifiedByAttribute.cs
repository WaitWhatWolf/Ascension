using Ascension.Enums;
using Ascension.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Attributes
{
    /// <summary>
    /// Useful for showing that a dev modified a script (or a part of it) that they did not initially create.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10), AttributeUsage(AttributeTargets.All)]
    public sealed class ModifiedByAttribute : CreatedBy
    {
        public readonly string Comment;

        public ModifiedByAttribute(Dev dev, string comment, int year, int month, int day) : base(dev, year, month, day)
        {
            Comment = comment;
        }

        public ModifiedByAttribute(Dev dev, string comment, string date) : base(dev, date)
        {
            Comment = comment;
        }

        public ModifiedByAttribute(Dev dev, string comment) : base(dev)
        {
            Comment = comment;
        }

        private ModifiedByAttribute() : base() { }
    }
}
