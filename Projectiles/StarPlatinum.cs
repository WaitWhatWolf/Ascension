using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Ascension.ASCResources.Stats;

namespace Ascension.Projectiles
{
    public sealed class StarPlatinum : StandProjectile
    {
        /// <inheritdoc/> //Not sure what to put as desc here lol
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.DamageType = ASCResources.Stats.DamageClass_Umbral;
            base.SetDefaults();
        }

        public override bool CloneNewInstances => true;

        protected override void OnBossDefeated(string name)
        {
            base.OnBossDefeated(name);
        }
    }
}
