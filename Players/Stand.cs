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
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.UI;
using static Ascension.ASCResources.Players;
using static Ascension.ASCResources.Stats;

namespace Ascension.Players
{
    /// <summary>
    /// Core class for Stands.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
    public sealed class Stand
    {
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
        public string Name { get; }

        /// <summary>
        /// The tooltip of the stand.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The identity of the stand.
        /// </summary>
        public StandID ID { get; }

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
        public Projectile GetStandProjectile() => pv_InstancedStand.Projectile;

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

            if(Level <= 1)
                Level++;

            if(debugUpgrade)
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
            if(Owner.ConsumedRedHotChiliPepper)
                UnlockAbility(1, debugUnlocks);

            //UnlockAbility(2, debugUnlocks);
            //UnlockAbility(3, debugUnlocks);
        }

        /// <summary>
        /// Unlocks a specific ability.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="debugUnlock"></param>
        public void UnlockAbility(int index, bool debugUnlock = true)
        {
            if (!Owner.UnlockedStandAbility[index])
            {
                Owner.UnlockedStandAbility[index] = true;
                if(debugUnlock)
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

            switch (ID)
            {
                case StandID.STAR_PLATINUM:
                    pv_InstancedStandIndex = Projectile.NewProjectile(new ProjectileSource_Stand(Owner, this), Owner.Player.Center, 
                        Vector2.Zero, ModContent.ProjectileType<StarPlatinum>(), GetStat(STAND_STAT_DAMAGE), 
                        GetSingleStat(STAND_STAT_KNOCKBACK));
                    pv_InstancedStand = (StarPlatinum)Main.projectile[pv_InstancedStandIndex].ModProjectile;
                    pv_InstancedStand.SetupStand(Owner.Player, this);
                    
                    break;
                default: Active = false; break;
            }

            SoundEngine.PlaySound(pv_InstancedStandIndex, Owner.Player.Center);
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

            for(int i = 0; i < Abilities.Length; i++) //i < Level <---- to revert to this
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
                pv_StatUpdater(this);
        }

        /// <summary>
        /// Returns a calculated int value using the owner's stats.
        /// </summary>
        /// <param name="statName"></param>
        /// <returns></returns>
        public int GetStat(string statName) => (int)Owner.Stats.CalculatedValue(pv_Stats[statName]);

        /// <summary>
        /// Returns a calculated float value using the owner's stats.
        /// </summary>
        /// <param name="statName"></param>
        /// <returns></returns>
        public float GetSingleStat(string statName) => Owner.Stats.CalculatedValue(pv_Stats[statName]);

        public Stand(AscendedPlayer player, StandID id)
        {
            Owner = player;
            Active = false;
            ID = id;
            Name = GetStandName(ID);
            Description = GetStandDescription(ID);
            pv_StatUpdater = GetStandStatUpdater(ID);
            Level = 1;

            Stat damage = new(0f, STATS_STACKING_BASE, (int)Affection.Damage);
            Stat attackrange = new(0f, STATS_STACKING_BASE, (int)Affection.Range);
            Stat armorpen = new(0f, STATS_STACKING_BASE, (int)Affection.ArmorPen);
            Stat attackspeed = new(0f, STATS_STACKING_BASE, (int)Affection.AttackSpeed);
            Stat knockback = new(0f, STATS_STACKING_BASE, (int)Affection.Knockback);
            Stat AIrange = new(0f, STATS_STACKING_BASE, (int)Affection.MovementRange);
            Stat movespeed = new(0f, STATS_STACKING_BASE, (int)Affection.MovementSpeed);

            switch (ID) //Add stats here
            {
                case StandID.STAR_PLATINUM:
                    damage.Value = 5f;
                    attackrange.Value = 40f;
                    armorpen.Value = 10f;
                    attackspeed.Value = 180f;
                    knockback.Value = 2f;
                    AIrange.Value = 200f;
                    movespeed.Value = 20f;

                    pv_InvokeSoundIndex = ASCResources.Sound.Stand_StarPlatinum_Invoke_Index;
                    Portrait = ASCResources.Textures.Stand_Portrait_StarPlatinum;
                    break;
            }

            pv_Stats.Add(STAND_STAT_DAMAGE, damage);
            pv_Stats.Add(STAND_STAT_ATTACKRANGE, attackrange);
            pv_Stats.Add(STAND_STAT_ARMORPEN, armorpen);
            pv_Stats.Add(STAND_STAT_ATTACKSPEED, attackspeed);
            pv_Stats.Add(STAND_STAT_KNOCKBACK, knockback);
            pv_Stats.Add(STAND_STAT_MOVESPEED, movespeed);
            pv_Stats.Add(STAND_STAT_AIRANGE, AIrange);
            Owner.Stats.AddStat(damage);
            Owner.Stats.AddStat(attackrange);
            Owner.Stats.AddStat(armorpen);
            Owner.Stats.AddStat(attackspeed);
            Owner.Stats.AddStat(knockback);
            Owner.Stats.AddStat(movespeed);
            Owner.Stats.AddStat(AIrange);

            switch (ID) //Add abilities here
            {
                case StandID.STAR_PLATINUM:
                    Abilities = new StandAbility[]
                    {
                        new StandAbility_StarPlatinum_Punch(this),
                        new StandAbility_StarPlatinum_ORA(this),
                        new StandAbility_StarPlatinum_Receipt(this),
                        new StandAbility_StarPlatinum_TheWorld(this),
                    };
                    break;
            }

            if (pv_BaseMovementAI == null)
                throw new Exception("The stand couldn't be created as there was no ability which set the base movement AI.");

            StandMenuUI = new UserInterface();
            StandMenu = new Menu_Stand(this);
            StandMenuUI.SetState(StandMenu);
        }

        private Action pv_BaseMovementAI;
        private Action<Stand> pv_StatUpdater;
        private Dictionary<string, Stat> pv_Stats = new();
        private List<Action> pv_MovementAIs = new();
        private StandProjectile pv_InstancedStand = null;
        private int pv_InstancedStandIndex = -1;
        private int pv_InvokeSoundIndex = -1;

        public static implicit operator bool(Stand stand) => stand != null && stand.ID != StandID.NEWBIE;
    }
}
