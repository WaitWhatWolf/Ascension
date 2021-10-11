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
    public sealed class Projectile_KillerQueen : Projectile_Stand_WAura<Dust_Stand_KillerQueen_Aura>
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
           
            Projectile.DamageType = pr_Stand?.Class ?? DamageClass.Generic;
            base.SetDefaults();
        }

        public override Animator StandAnimator { get; } = new(ASCResources.Animations.Stand_KillerQueen);

        public override bool MinionContactDamage() => false;

        public override bool CloneNewInstances => true;

        protected override bool StandUserEmitsAura { get; } = true;

        protected override int AfterImageLength { get; } = 6;
    }
}
