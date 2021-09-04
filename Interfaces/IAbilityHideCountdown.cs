using Ascension.Attributes;
using Ascension.Enums;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Any ability marked by this interface will not draw it's countdown on the stand menu.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 18:08:38")]
    public interface IAbilityHideCountdown
    {
        bool HideCountdown();
    }
}
