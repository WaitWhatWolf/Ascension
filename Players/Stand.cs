using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Projectiles;
using Ascension.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.UI;

namespace Ascension.Players
{
    /// <summary>
    /// Core class for Stands.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
    public abstract class Stand
    {
        /// <summary>
        /// Standard constructor for the <see cref="Stand"/> class.
        /// </summary>
        /// <param name="player">The player reference to be passed in this stand.</param>
        public Stand(AscendedPlayer player)
        {
            if (player == null)
                return;

            Owner = player;
            Active = false;
            Level = 1;

            if (base_stats_other != null)
                foreach (var pair in base_stats_other)
                {
                    pv_Stats.Add(pair.Key, pair.Value);
                }

            Portrait = Init_Portrait;
            Abilities = Init_Abilities;

            /*foreach (StandAbility ability in Abilities)
            {
                typeof(StandAbility).GetMethod("Event_OnNewBossDefeated", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(ability, new object[] { string.Empty });
            }*/

            if (pv_BaseMovementAI == null)
                throw new Exception("The stand couldn't be created as there was no ability which set the base movement AI.");

            pv_InvokeSound = new(Path.Combine(Path.GetDirectoryName(Terraria.ModLoader.ModLoader.ModPath), @"Mod Sources\Ascension\Assets\Sound\Custom", StandInvokeSoundPath));

            StandMenuUI = new UserInterface();
            StandMenu = new Menu_Stand(this);
            StandMenuUI.SetState(StandMenu);
        }

        /// <summary>
        /// Invoked when <see cref="UnlockAbility(int)"/> is successfully called.
        /// </summary>
        public event Action<int> OnAbilityUnlock;

        /// <summary>
        /// The player who owns this stand.
        /// </summary>
        public AscendedPlayer Owner { get; }
        /// <summary>
        /// Is the stand currently fighting for the player?
        /// </summary>
        public bool Active { get; private set; }
        /// <summary>
        /// The name of the stand.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The tooltip of the stand.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The identity of the stand.
        /// </summary>
        public abstract StandID ID { get; }

        /// <summary>
        /// The current level of the stand.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Menu used by the stand.
        /// </summary>
        public Menu_Stand StandMenu { get; private set; }

        /// <summary>
        /// The user interface used with <see cref="StandMenu"/>.
        /// </summary>
        public UserInterface StandMenuUI { get; private set; }

        /// <summary>
        /// The portrait texture of this stand.
        /// </summary>
        public Asset<Texture2D> Portrait { get; private set; }

        /// <summary>
        /// All abilities of this stand.
        /// </summary>
        public StandAbility[] Abilities { get; private set; }

        /// <summary>
        /// Returns the mod stand projectile of this stand.
        /// </summary>
        /// <returns></returns>
        public StandProjectile GetStandModProjectile() => pv_InstancedStand;

        /// <summary>
        /// Returns the normal stand projectile of this stand.
        /// </summary>
        /// <returns></returns>
        public Projectile GetStandProjectile() => pv_InstancedStand?.Projectile;

        /// <summary>
        /// Returns the current movement AI used by the stand.
        /// </summary>
        /// <returns></returns>
        public Action GetCurrentMovementAI() => pv_MovementAIs.Count > 0 ? pv_MovementAIs[0] : pv_BaseMovementAI;

        /// <summary>
        /// Adds a movement AI; The most recent AI will be used instead of the base movement AI.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool AddMovementAI(Action action)
        {
            if (pv_MovementAIs.Contains(action))
                return false;

            pv_MovementAIs.Insert(0, action);
            return true;
        }

        /// <summary>
        /// Removes a given MovementAI; If no more custom movement AI's are present, the movement AI will return to the base movement AI.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RemoveMovementAI(Action action)
        {
            return pv_MovementAIs.Remove(action);
        }

        /// <summary>
        /// Sets the base movement AI.
        /// </summary>
        /// <param name="AI"></param>
        public void SetBaseMovementAI(Action AI)
        {
            pv_BaseMovementAI = AI;
        }

        /// <summary>
        /// Attempts to upgrade the stand.
        /// </summary>
        /// <returns>True if stand was upgraded.</returns>
        public int UpgradeStand(bool debugUpgrade = true)
        {
            int toReturn = 1;

            if (debugUpgrade)
            {
                Debug.Log($"{Name} has leveled up! (Current Level: {Level})");
            }
            return toReturn;
        }

        /// <summary>
        /// Attempts to unlock all abilities according to their unlock requirements.
        /// </summary>
        /// <param name="debugUnlocks"></param>
        public void TryUnlockAbilities(bool debugUnlocks = true)
        {
            UnlockAbility(0, false);
            if (Owner.ConsumedRedHotChiliPepper)
                UnlockAbility(1, debugUnlocks);

            UnlockAbility(2, debugUnlocks);
            //UnlockAbility(3, debugUnlocks);
        }

        /// <summary>
        /// Unlocks a specific ability.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="debugUnlock"></param>
        public void UnlockAbility(int index, bool debugUnlock = true)
        {
            if (index >= Owner.UnlockedStandAbility.Length)
            {
                Debug.LogWarning($"There is no ability at index {index}.");
                return;
            }

            if (!Owner.UnlockedStandAbility[index])
            {
                Owner.UnlockedStandAbility[index] = true;
                if (debugUnlock)
                    Debug.Log($"{Name} has unlocked {Abilities[index].Name}!");

                OnAbilityUnlock?.Invoke(index);
            }
        }

        /// <summary>
        /// Calls the stand to fight for the player.
        /// </summary>
        public void Invoke()
        {
            Active = true;

            pv_InstancedStandIndex = Projectile.NewProjectile(new ProjectileSource_Stand(Owner, this), Owner.Player.Center,
                Vector2.Zero, StandProjectileType, GetDamage(), GetKnockback());
            pv_InstancedStand = (StandProjectile)Main.projectile[pv_InstancedStandIndex].ModProjectile;

            pv_InstancedStand.SetupStand(Owner.Player, this);
            
            pv_InvokeSound.Play();
            StandMenu.ActivateMenu();
        }

        /// <summary>
        /// Opposite of <see cref="Invoke"/>.
        /// </summary>
        public void Recall()
        {
            pv_InstancedStand?.Projectile.Kill();
            Active = false;
            pv_InstancedStand = null;
            StandMenu.DeactivateMenu();
        }

        /// <summary>
        /// This should be called by the player every frame.
        /// </summary>
        public void Update()
        {
            if (Active)
                pv_InstancedStand.Projectile.timeLeft = int.MaxValue;

            for (int i = 0; i < Abilities.Length; i++)
            {
                if (!Owner.UnlockedStandAbility[i])
                    continue;
                Abilities[i].Update();
                //This makes it so even if the stand is recalled, it will still cool down all abilities.
                if (Active)
                {

                    //Tries to activate this ability if it's either the basic attack or the correct key was pressed.
                    if (i == 0 || ASCResources.Input.GetStandAbilityKey(i).JustPressed)
                        Abilities[i].TryActivate();

                    Abilities[i].TryDeactivate();
                }
            }
        }

        /// <summary>
        /// This should be called by the player every frame.
        /// </summary>
        public void UpdateStats()
        {
            if (Active)
                StatUpdater();
        }

        /// <summary>
        /// Returns a calculated int value using the owner's stats.
        /// </summary>
        /// <param name="statName"></param>
        /// <returns></returns>
        public int GetOtherStat(string statName) => (int)Owner.Stats.CalculatedValue(pv_Stats[statName]);

        /// <summary>
        /// Returns a calculated float value using the owner's stats.
        /// </summary>
        /// <param name="statName"></param>
        /// <returns></returns>
        public float GetOtherSingleStat(string statName) => Owner.Stats.CalculatedValue(pv_Stats[statName]);

        /// <summary>Returns the calculated damage of the stand.</summary><returns></returns>
        public int GetDamage() => (int)Owner.Stats.CalculatedValue(base_stat_damage);
        /// <summary>Returns the calculated armor penetration of the stand.</summary><returns></returns>
        public int GetArmorPen() => (int)Owner.Stats.CalculatedValue(base_stat_armorpen);
        /// <summary>Returns the calculated attack speed (in RPM) of the stand.</summary><returns></returns>
        public float GetAttackSpeed() => (int)Owner.Stats.CalculatedValue(base_stat_attackspeed);
        /// <summary>Returns the calculated knockback of the stand.</summary><returns></returns>
        public float GetKnockback() => (int)Owner.Stats.CalculatedValue(base_stat_knockback);
        /// <summary>Returns the attack range of the stand.</summary><returns></returns>
        public float GetRange() => (int)Owner.Stats.CalculatedValue(base_stat_range);
        /// <summary>Returns the calculated AI-based range of the stand.</summary><returns></returns>
        public float GetAIRange() => (int)Owner.Stats.CalculatedValue(base_stat_airange);
        /// <summary>Returns the calculated movement speed of the stand..</summary><returns></returns>
        public float GetSpeed() => (int)Owner.Stats.CalculatedValue(base_stat_movespeed);

        /// <summary>
        /// Use this method to update the stats which this stand affects on the player.
        /// </summary>
        protected abstract void StatUpdater();

        /// <summary>
        /// The identity of the projectile to spawn on <see cref="Invoke"/>.
        /// </summary>
        protected abstract int StandProjectileType { get; }
        protected abstract string StandInvokeSoundPath { get; }

        protected abstract StandAbility[] Init_Abilities { get; }
#pragma warning disable IDE1006
        protected abstract Stat base_stat_damage { get; }
        protected abstract Stat base_stat_armorpen { get; }
        protected abstract Stat base_stat_attackspeed { get; }
        protected abstract Stat base_stat_knockback { get; }
        protected abstract Stat base_stat_range { get; }
        protected abstract Stat base_stat_airange { get; }
        protected abstract Stat base_stat_movespeed { get; }
        /// <summary>
        /// You can add additional base stats in here.
        /// </summary>
        /// <remarks>
        /// These will be added to an internal list which is then used by <see cref="GetOtherStat(string)"/> and <see cref="GetOtherSingleStat(string)"/>.
        /// </remarks>
        protected virtual KeyValuePair<string, Stat>[] base_stats_other { get; }
#pragma warning restore IDE1006
        protected abstract Asset<Texture2D> Init_Portrait { get; }

        private Action pv_BaseMovementAI;
        private Dictionary<string, Stat> pv_Stats = new();
        private List<Action> pv_MovementAIs = new();
        private StandProjectile pv_InstancedStand = null;
        private int pv_InstancedStandIndex = -1;
        private int pv_InvokeSoundIndex = -1;
        [Note(Dev.WaitWhatWolf, "This is only temporary while tModLoader hasn't merged the soundfix branch.")]
        private System.Media.SoundPlayer pv_InvokeSound;

        public static implicit operator bool(Stand stand) => stand != null && stand.ID != StandID.NEWBIE;
    }
}
