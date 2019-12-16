using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WarWolfWorks_Mod.UI
{
    /// <summary>
    /// UI Displaying all relevant information about the player's stand.
    /// </summary>
    public sealed class StandUI : Menu
    {
        /// <summary>
        /// Height of the Stand UI.
        /// </summary>
        protected override StyleDimension DimensionHeight => new StyleDimension(136, 0f); //136 = 128 + (4 * 2)
        /// <summary>
        /// Width of the Stand UI.
        /// </summary>
        protected override StyleDimension DimensionWidth => new StyleDimension(544, 0f); //544 = 136 * 4

        private UIImage[] AbilityImages;

        public override void OnInitialize()
        {
            AbilityImages = new UIImage[Perpetrator.Stand.Abilities.Length];
            for(int i = 0; i < AbilityImages.Length; i++)
            {
                string txtName = Perpetrator.Stand.Abilities[i].Texture;
                if (string.IsNullOrEmpty(txtName))
                    continue;
                AbilityImages[i] = new UIImage(Perpetrator.mod.GetTexture(txtName));
            }
        }
    }
}
