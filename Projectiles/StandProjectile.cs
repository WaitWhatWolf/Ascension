using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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

			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CountsAsHoming[Projectile.type] = false; //Some damage reduction shit, nothing to do with AI
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
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
			Projectile.alpha = 30;
		}

		public override bool? CanCutTiles() => true;
		public override bool MinionContactDamage() => true;

		/// <summary>
		/// The animator of this stand projectile.
		/// </summary>
		public abstract Animator StandAnimator { get; }

		/// <summary>
		/// Returns the position in front of the stand.
		/// </summary>
		public virtual Vector2 Front
		{
			get { return Projectile.spriteDirection == -1 ? Projectile.Right : Projectile.Left; }
			set
            {
				if (Projectile.spriteDirection == -1)
					Projectile.Right = value;
				else Projectile.Left = value;
            }
		}
		/// <summary>
		/// Returns the normalized direction the stand is facing.
		/// </summary>
		public virtual Vector2 Direction =>  new(Projectile.spriteDirection == -1 ? 1f : -1f, 0f);

		/// <summary>
		/// Whether this stand emits an after-image effect.
		/// </summary>
		public virtual bool EmitsAfterImages { get; } = true;

		internal void SetupStand(Player owner, Stand stand)
        {
			pr_Owner = owner;
			pr_Stand = stand;
			pr_Stand.Owner.OnNewBossDefeated += OnBossDefeated;
			OnBossDefeated(string.Empty);
		}

		/// <summary>
		/// Emits light as well as after-images if <see cref="EmitsAfterImages"/> is true.
		/// </summary>
		/// <param name="lightColor"></param>
		/// <returns></returns>
		public override bool PreDraw(ref Color lightColor)
        {
			Lighting.AddLight(Projectile.Center, pr_Stand.ThemeColor.R / 100f, pr_Stand.ThemeColor.G / 100f, pr_Stand.ThemeColor.B / 100f);

			if (EmitsAfterImages)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (Projectile.spriteDirection == -1)
					spriteEffects = SpriteEffects.FlipHorizontally;

				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

				int frameHeight = texture.Height / Main.projFrames[Projectile.type];
				int startY = frameHeight * Projectile.frame;

				Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
				Vector2 origin = sourceRectangle.Size() / 2f;

				for (int i = 0; i < Projectile.oldPos.Length; i++)
				{
					Color drawColor = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * (i < 3 ? 1f : .5f);
					Main.EntitySpriteDraw(texture,
						(Projectile.oldPos[i] + (Projectile.Size / 2f)) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
						sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
				}
			}

			return true;
		}

        public sealed override void AI()
		{
			if (!CheckActive(pr_Owner))
				return;

			pr_Stand.GetCurrentMovementAI()();
			Projectile.timeLeft = 18000;
			StandAnimator.Update();
			Projectile.frame = StandAnimator;

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
			Projectile.damage = pr_Stand.GetDamage();
			Projectile.knockBack = pr_Stand.GetKnockback();
		}

		protected Player pr_Owner;
		protected Stand pr_Stand;

		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
				return false;

			return true;
		}
	}
}
