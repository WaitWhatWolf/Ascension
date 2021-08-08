using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Ascension.UI
{
    public sealed class Menu_Stand : Menu
    {
        protected override float DimensionHeight => 64;
        protected override float DimensionWidth => 64 + 32 * (pv_Stand?.Abilities.Length ?? 3);
        protected override float DimensionLeft => Main.screenWidth / 3;
        protected override float DimensionTop => Main.screenHeight * 0.9f;

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActiveDrawSelf(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < pv_Stand.Abilities.Length; i++)
            {
                StandAbility ability = pv_Stand.Abilities[i];
                Rectangle rectangle = pv_AbilityImages[i].GetClippingRectangle(spriteBatch);
                
                if (i != 0)
                {
                    spriteBatch.DrawString(FontAssets.MouseText.Value, ASCResources.Input.GetStandAbilityKey(i).GetAssignedKeys().First(), rectangle.Center(), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                    if (ability.GetCurrentCooldown() > 0f)
                        spriteBatch.DrawString(FontAssets.MouseText.Value,
                           ability.GetCurrentCooldown().Truncate(1).ToString(), rectangle.Bottom(), Color.Crimson, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
        }

        public Menu_Stand(Stand stand)
        {
            pv_Stand = stand;

            pv_AbilityImages = new UIImage[pv_Stand.Abilities.Length];

            //Background image to add
            /*pv_BackgroundImage = new UIImage(TextureAssets.Logo4);
            pv_BackgroundImage.Left.Set(0, 0f);
            pv_BackgroundImage.Width.Set(DimensionWidth, 0f);
            pv_BackgroundImage.Top.Set(0, 0f);
            pv_BackgroundImage.Height.Set(DimensionHeight, 0f);
            Append(pv_BackgroundImage);*/

            pv_PortraitImage = new UIImage(stand.Portrait);
            pv_PortraitImage.Left.Set(0, 0f);
            pv_PortraitImage.Width.Set(64, 0f);
            pv_PortraitImage.Top.Set(0, 0f);
            pv_PortraitImage.Height.Set(64, 0f);
            Append(pv_PortraitImage);

            for (int i = 0; i < pv_Stand.Abilities.Length; i++)
            {
                UIImage image = new(pv_Stand.Abilities[i].Icon);
                image.Left.Set(64 + (32 * i), 0f);
                image.Width.Set(32, 0f);
                image.Top.Set(32, 0f);
                image.Height.Set(32, 0f);
                image.Color = Color.White;
                this.Append(image);

                pv_AbilityImages[i] = image;
            }
        }

        private Stand pv_Stand;
        private UIImage pv_BackgroundImage;
        private UIImage pv_PortraitImage;
        private UIImage[] pv_AbilityImages;
    }
}
