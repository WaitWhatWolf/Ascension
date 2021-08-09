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
    /// Determines which thing was initially created by who.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10), AttributeUsage(AttributeTargets.All)]
    public class CreatedBy : Attribute, IMadeBy
    {
        public Dev Dev { get; }
        public readonly DateTime CreationTime;
        public readonly bool DateUnknown;

        public CreatedBy(Dev dev, int year, int month, int day)
        {
            Dev = dev;
            CreationTime = new DateTime(year, month, day);
        } 
        
        public CreatedBy(Dev dev, string date)
        {
            Dev = dev;
            if (DateTime.TryParse(date, out DateTime result))
                CreationTime = result;
            else DateUnknown = true;
        }

        public CreatedBy(Dev dev)
        {
            Dev = dev;
            DateUnknown = true;
        }

        protected CreatedBy() { }
    }
}
