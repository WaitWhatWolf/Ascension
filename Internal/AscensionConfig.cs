using Ascension.Attributes;
using Ascension.Enums;
using Ascension.UI;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Ascension.Internal
{
    /// <summary>
    /// Config manager of the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    internal sealed class AscensionConfig : ModConfig
    {
        public override ConfigScope Mode { get; } = ConfigScope.ClientSide;

        [DefaultValue(470), Tooltip("The X position the stand menu will appear in.aaaaaaaa"), Range(0, 1920), Slider]
        public int StandMenuPosX;
        [DefaultValue(26), Tooltip("The Y position the stand menu will appear in."), Range(0, 1080), Slider]
        public int StandMenuPosY;

    }
}
