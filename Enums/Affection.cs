using Ascension.Attributes;
using Ascension.Internal;

namespace Ascension.Enums
{
    /// <summary>
    /// Affections indexed for <see cref="EntityStats"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public enum Affection
    {
        Undefined = 0,

        Damage = 101,
        AttackSpeed = 102,
        Range = 103,
        ArmorPen = 104,
        Knockback = 105,

        MovementSpeed = 201,
        MovementRange = 202,
    }
}
