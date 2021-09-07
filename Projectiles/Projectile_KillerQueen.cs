using Ascension.Attributes;
using Ascension.Dusts;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Stand projectile for Killer Queen.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class Projectile_KillerQueen : StandProjectile
    {
        /// <inheritdoc/> //Not sure what to put as desc here lol
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 138;
           
            Projectile.DamageType = Terraria.ModLoader.DamageClass.Melee;
            base.SetDefaults();
        }

        public override void PostDraw(Color lightColor)
        {
            foreach((int, Vector2) pos in pv_DustPositions)
            {
                if(Hooks.Random.Range(0, 3) == 0)
                    Dust.NewDust(Projectile.TopLeft + pos.Item2, 7, 9, 
                        ModContent.DustType<Dust_Stand_KillerQueen_Aura>(), 
                        SpeedX: pos.Item1 * 2, 
                        newColor: Hooks.Random.Range(0, 3) == 0 ? Color.DodgerBlue : default,
                        Alpha: Hooks.Random.Range(pos.Item1 != 0 ? 200 : 125, 230));
            }
        }

        protected override void BonusAI()
        {
            if(Projectile.frame != pv_PrevFrame || pv_PrevSpriteDir != Projectile.spriteDirection)
            {
                UpdateDustPositions();
            }

            pv_PrevFrame = Projectile.frame;
            pv_PrevSpriteDir = Projectile.spriteDirection;
        }

        public override Animator StandAnimator { get; } = new(ASCResources.Animations.Stand_KillerQueen);

        public override bool MinionContactDamage() => false;

        public override bool CloneNewInstances => true;

        protected override void OnBossDefeated(string name)
        {
            base.OnBossDefeated(name);

            UpdateDustPositions();
        }

        private void UpdateDustPositions()
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            Color[,] pixels = Hooks.Colors.GetColorGridFromSprite(texture, 0, Projectile.frame, out int width, out int height,
                maxFramesY: Main.projFrames[Projectile.type] = 6,
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
                        positions.Add((index, new(x + Projectile.gfxOffY - 3, y - 4)));
                    }
                }
            }

            pv_DustPositions = positions.ToArray();
        }

        private (int, Vector2)[] pv_DustPositions;
        private int pv_PrevFrame;
        private int pv_PrevSpriteDir;
    }
}
