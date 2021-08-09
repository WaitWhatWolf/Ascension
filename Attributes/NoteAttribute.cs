using Ascension.Enums;
using Ascension.Interfaces;
using System;

namespace Ascension.Attributes
{
    /// <summary>
    /// Used as a "note" in attribute form.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10), AttributeUsage(AttributeTargets.All)]
    public sealed class NoteAttribute : Attribute, IMadeBy
    {
        public Dev Dev { get; }
        public readonly string Note;

        public NoteAttribute(Dev dev, string note)
        {
            Dev = dev;
            Note = note;
        }
    }
}
