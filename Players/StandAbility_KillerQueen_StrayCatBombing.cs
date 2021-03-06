using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Projectiles;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/29 6:32:10")]
    public class StandAbility_KillerQueen_StrayCatBombing : StandAbility
    {
        public StandAbility_KillerQueen_StrayCatBombing(Stand stand, int index) : base(stand, index)
        {

        }

        public override string Name { get; } = "Stray Cat Bombing";

        public override string Description => $"{{{{c:Stand_Title={Stand.Name}}}}}"
            + " fires a barrage of bubbles\nwhich {{c:Effect=seek the most recently attacked enemy}} when close." +
            "\nWhen the bubbles hit terrain or a {{c:Effect=seeked enemy}}," +
            "\nthey {{c:Effect=explode}} dealing damage to anything within"
            + $"\na {{{{c:Stat={((int)(Stand.GetRange() * 2f)) / 16}-block distance}}}}.";

        public override string Quote { get; } = "Once narrow like a cat's; Now broad.";

        public override Asset<Texture2D> Icon => ASCResources.Textures.GetTexture(ASCResources.Textures.STAND_ABILITY_KILLERQUEEN_ABILITY1);

        protected override ReturnCountdown Countdown => pv_Countdown;

        protected override bool ActivateCondition() => this.CountdownReady;

        protected override bool DeactivateCondition() => Active && pv_AttackCounter >= pv_AttackCount;

        protected override void OnActivate()
        {
            ResetCountdown();

            Stand.GetStandModProjectile().StandAnimator.Play(ASCResources.Animations.NAME_STAND_KILLERQUEEN_STRAYCATBOMBING);
        }

        public override void Update()
        {
            base.Update();

            if(Active)
            {
                if(pv_AttackDelayCountdown)
                {
                    var vel = new Vector2Range(0f, -0.3f, 0f, 0.3f);
                    var standProj = Stand.GetStandModProjectile();
                    int projID = Projectile.NewProjectile(new Projectiles.ProjectileSource_Stand(Stand.Owner, Stand), standProj.Front, 
                        new Vector2(standProj.Direction.X, vel.GetRandom().Y) * pv_ProjectileVelocity, pv_ProjectileType, 
                        pv_ProjectileDamage, pv_ProjectileKnockback);
                    
                    ((Projectile_KillerQueen_StrayCatBomb)Main.projectile[projID].ModProjectile).Init(Stand, Stand.Owner);
                    pv_AttackCounter++;
                }
            }
        }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_ProjectileDamage = Stand.GetDamage();
            pv_ProjectileKnockback = Stand.GetKnockback();
            pv_ProjectileVelocity = Stand.GetOtherSingleStat(ASCResources.Stats.STAND_STAT_PROJECTILE_VELOCITY);
            float rpm = Stand.GetAttackSpeed();
            pv_AttackCount = (int)(rpm / 5f);
            pv_AttackDelayCountdown = 60f / (rpm * 12f);
            pv_ProjectileType = ModContent.ProjectileType<Projectile_KillerQueen_StrayCatBomb>();
        }

        protected override void OnDeactivate()
        {
            pv_AttackCounter = 0;

            Stand.StandAnimator.Play();
        }

        private ReturnCountdown pv_Countdown = new(11f, true);
        private ReturnCountdown pv_AttackDelayCountdown;
        private int pv_ProjectileType;
        private int pv_ProjectileDamage;
        private int pv_AttackCounter;
        private int pv_AttackCount;
        private float pv_ProjectileVelocity;
        private float pv_ProjectileKnockback;
    }
}
