using Ascension.UI;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Ascension.Internal
{
    public sealed class AscensionConfig : ModConfig
    {
        public override ConfigScope Mode { get; } = ConfigScope.ClientSide;

        [DefaultValue(470), Tooltip("The X position the stand menu will appear in.aaaaaaaa"), Range(0, 1920), Slider]
        public int StandMenuPosX;
        [DefaultValue(26), Tooltip("The Y position the stand menu will appear in."), Range(0, 1080), Slider]
        public int StandMenuPosY;

        public override void OnChanged()
        {
            Menu.GetMenu<Menu_Stand>()?.SetDefaultPosition(StandMenuPosX, StandMenuPosY);
        }
    }
}
