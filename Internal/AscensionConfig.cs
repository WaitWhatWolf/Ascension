using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.UI;
using Ascension.World;
using System;
using System.ComponentModel;
using System.Reflection;
using Terraria.ModLoader;
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

        [Header("Stand Menu")]
        [DefaultValue(470), Tooltip("The X position the stand menu will appear in."), Range(0, 1920), Slider, SliderColor(249, 77, 0), BackgroundColor(249, 77, 0)]
        public int StandMenuPosX;
        [DefaultValue(26), Tooltip("The Y position the stand menu will appear in."), Range(0, 1080), Slider, SliderColor(249, 77, 0), BackgroundColor(249, 77, 0)]
        public int StandMenuPosY;

        [DefaultValue(1f), Tooltip("How large the stand menu appears."), Range(0.5f, 2f), Slider, SliderColor(249, 77, 0), BackgroundColor(249, 77, 0), DrawTicks, Increment(0.25f)]
        public float StandMenuScale;

    }
}
