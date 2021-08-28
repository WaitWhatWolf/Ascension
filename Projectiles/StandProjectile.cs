using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Terraria;
using Terraria.ID;
using static Ascension.ASCResources.Stats;

namespace Ascension.Projectiles
{
    /// <summary>
    /// The base class for all in-game stand instances.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
	public abstract class StandProjectile : AscensionProjectile
    {
		/// <summary>
		/// Sets necessary variables to make the stand work; Make sure to include "base.SetStaticDefaults();".
		/// </summary>
        public override void SetStaticDefaults()
        {
			base.SetStaticDefaults();
			//The line below is just so I don't forget, ignore
			//Main.projFrames[Projectile.type] = 4;

			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CountsAsHoming[Projectile.type] = false; //Some damage retuction shit, nothing to do with AI
		}

		/// <summary>
		/// Sets necessary variables to make the stand work; Make sure to include "base.SetDefaults();".
		/// </summary>
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;

			Projectile.friendly = true;
			Projectile.minion = false; // Only determines the damage type								  
			Projectile.penetrate = -1; // Needed so stand doesn't despawn
		}

		public override bool? CanCutTiles() => true;
		public override bool MinionContactDamage() => true;

		internal void SetupStand(Player owner, Stand stand)
        {
			pv_Owner = owner;
			pv_Stand = stand;
			pv_Stand.Owner.OnNewBossDefeated += OnBossDefeated;
			OnBossDefeated(string.Empty);
		}

		public sealed override void AI()
		{
			if (!CheckActive(pv_Owner))
				return;

			pv_Stand.GetCurrentMovementAI()();

			BonusAI();
		}

		/// <summary>
		/// Called at the end of <see cref="AI"/>.
		/// </summary>
		protected virtual void BonusAI() { }

        /// <summary>
        /// Called when a new boss has been defeated;
        /// Also called when <see cref="SetupStand(Player, Stand)"/> is successfully setup.
        /// Make sure to include "base.OnBossDefeated();".
        /// </summary>
        protected virtual void OnBossDefeated(string name)
        {
			Projectile.damage = pv_Stand.GetDamage();
			Projectile.knockBack = pv_Stand.GetKnockback();
		}

		protected Player pv_Owner;
		protected Stand pv_Stand;

		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
				return false;

			return true;
		}
	}
}
