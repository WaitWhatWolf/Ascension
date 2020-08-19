using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;

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
        private Texture2D[] AbilityTextures;

        protected override void OnActiveDrawSelf(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < AbilityImages.Length; i++)
            {
                Rectangle rectangle = new Rectangle((int)AbilityImages[i].Left.Pixels, (int)AbilityImages[i].Top.Pixels,
                    (int)AbilityImages[i].Width.Pixels, (int)AbilityImages[i].Height.Pixels);
                spriteBatch.Draw(AbilityTextures[i], rectangle, Color.White);
                Main.NewText(rectangle.ToString(), 175, 75, 255);
            }
        }

        public override void OnInitialize()
        {
            AT_Init();
            
        }

        private async void AT_Init()
        {
            while(!Perpetrator)
            {
                await Task.Delay(25);
            }

            AbilityImages = new UIImage[Perpetrator.Stand.Abilities.Length];
            AbilityTextures = new Texture2D[Perpetrator.Stand.Abilities.Length];
            for (int i = 0; i < AbilityImages.Length; i++)
            {
                string txtName = Perpetrator.Stand.Abilities[i].Texture;
                if (string.IsNullOrEmpty(txtName))
                    continue;

                AbilityTextures[i] = Perpetrator.mod.GetTexture(txtName);
                AbilityImages[i] = new UIImage(AbilityTextures[i]);
                AbilityImages[i].Width.Set(AbilityTextures[i].Width, 0);
                AbilityImages[i].Height.Set(AbilityTextures[i].Height, 0);
                AbilityImages[i].Top.Set(136, 0);
                AbilityImages[i].Left.Set(544 + (AbilityTextures[i].Width * i), 0);
                
                //Append(AbilityImages[i]);
            }
        }
    }
}
