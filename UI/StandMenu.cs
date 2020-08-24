using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.UI;
using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.UI
{
    public sealed class StandMenu : Menu
    {
        protected override float DimensionHeight => 64;
        protected override float DimensionWidth => 64 + 32 * (WWWPlayer.Instance?.Stand?.Abilities?.Length ?? 3);
        protected override float DimensionLeft => Main.screenWidth / 3;
        protected override float DimensionTop => Main.screenHeight * 0.9f;

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (WWWMOD.Instance != null)
            {
                base.Update(gameTime);
            }
        }

        protected override void OnActiveDrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dim = GetDimensions();

            spriteBatch.Draw(WWWPlayer.Instance.Stand.Portrait, new Vector2(dim.X, dim.Y), Color.White);

            if (WWWPlayer.Instance.Stand != null)
            {
                for (int i = 0; i < WWWPlayer.Instance.Stand.Abilities.Length; i++)
                {
                    StandAbility ability = WWWPlayer.Instance.Stand.Abilities[i];
                    Rectangle rectangle = new Rectangle(64 + (int)dim.X + (32 * i), (int)dim.Y, 32, 32);
                    spriteBatch.Draw(
                        ability.UITexture,
                        rectangle,
                        Color.White
                        );

                    if (ability.Countdown != TimeSpan.Zero)
                        spriteBatch.DrawString(Main.fontMouseText,
                           ability.Countdown.TotalSeconds.Truncate(1).ToString(), rectangle.Center(), Color.Crimson, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
