using Ascension.Attributes;
using Ascension.Dusts;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/10/01 11:26:59")]
    public abstract class Projectile_Stand_WAura<T> : StandProjectile where T : AscensionDust
    {
        /// <summary>
        /// Draws the aura; Make sure to include "base.PostDraw(lightColor);" when overriding.
        /// </summary>
        /// <param name="lightColor"></param>
        public override void PostDraw(Color lightColor)
        {
            foreach ((int, Vector2) pos in pr_DustPositions)
            {
                if (Hooks.Random.Range(0, 3) == 0)
                    Dust.NewDust(Projectile.TopLeft + pos.Item2, 7, 9,
                        ModContent.DustType<T>(),
                        SpeedX: pos.Item1 * 2,
                        newColor: Color.White,
                        Alpha: Hooks.Random.Range(pos.Item1 != 0 ? 220 : 150, 230));
            }
        }

        /// <summary>
        /// Handles all effects; Make sure to include "base.BonusAI();" when overriding.
        /// </summary>
        protected override void BonusAI()
        {
            if (Projectile.frame != PrevFrame || PrevSpriteDir != Projectile.spriteDirection)
            {
                UpdateDustPositions();
            }

            PrevFrame = Projectile.frame;
            PrevSpriteDir = Projectile.spriteDirection;

            if(StandUserEmitsAura)
                for (int i = 0; i < 50; i++)
                    Dust.NewDust(pr_Owner.TopLeft + (Vector2.One * -16f),
                        pr_Owner.width + 32,
                        pr_Owner.height + 32, ModContent.DustType<T>());
        }

        /// <summary>
        /// Updates where the aura is displayed; Make sure to include "base.OnBossDefeated(name);" when overrding.
        /// </summary>
        /// <param name="name"></param>
        protected override void OnBossDefeated(string name)
        {
            base.OnBossDefeated(name);

            UpdateDustPositions();
        }

        private void UpdateDustPositions()
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            Color[,] pixels = Hooks.Colors.GetColorGridFromSprite(texture, 0, Projectile.frame, out int width, out int height,
                maxFramesY: Main.projFrames[Projectile.type],
                flipped: Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            List<(int, Vector2)> positions = new();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (pixels[x, y] == Color.Transparent)
                        continue;

                    int top = y - 1;
                    int left = x - 1;
                    int right = x + 1;
                    //int bottom = y + 1;

                    bool pTop = top > -1 && pixels[x, top] != Color.Transparent;
                    bool pLeft = left > -1 && pixels[left, y] != Color.Transparent;
                    bool pRight = right < texture.Width && pixels[right, y] != Color.Transparent;
                    //bool pBot = bottom < frameHeight && pixels[x, bottom] != Color.Transparent;

                    int index = -2;
                    if (!pTop) index = 0;
                    else if (!pLeft) index = -1;
                    else if (!pRight) index = 1;

                    if (index != -2)
                    {
                        positions.Add((index, new(x + Projectile.gfxOffY - 4, y - 4)));
                    }
                }
            }

            pr_DustPositions = positions.ToArray();
        }

        /// <summary>
        /// If true, the player will also emit a light aura similar to the stand.
        /// </summary>
        protected abstract bool StandUserEmitsAura { get; }

        protected int PrevFrame { get; private set; }
        protected int PrevSpriteDir { get; private set; }
        protected (int, Vector2)[] pr_DustPositions;
    }
}
