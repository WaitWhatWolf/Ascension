using Ascension;
using Ascension.Attributes;
using Ascension.Dusts;
using Ascension.Enums;
using Ascension.Interfaces;
using Ascension.Internal;
using Ascension.Items;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Ascension.Players
{
    /// <summary>
    /// Core player class of the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
    public sealed class AscendedPlayer : ModPlayer, IAscensionEntity
    {
        public bool ConsumedRedHotChiliPepper;
        
        /// <summary>
        /// The current target of the player.
        /// </summary>
        public NPC Target { get; private set; }
        public bool[] UnlockedStandAbility { get; private set; } = new bool[4];
        public List<string> DefeatedBosses { get; private set; }

        /// <summary>
        /// Stats of the ascended player.
        /// </summary>
        public EntityStats Stats { get; private set; } 

        /// <summary>
        /// The initial position of the player when he first appeared in a world.
        /// </summary>
        public Point SpawnPos { get; private set; }

        /// <summary>
        /// Invoked when a new boss is defeated.
        /// </summary>
        public event Action<string> OnNewBossDefeated;

        /// <summary>
        /// Invoked when the player manifests his stand.
        /// </summary>
        public event Action<Stand, int> OnManifestStand;

        /// <summary>
        /// Invoked when the player has set a new target.
        /// </summary>
        public event Action<NPC> OnSetTarget;
        
        /// <summary>
        /// Invoked when the player has removed it's current target.
        /// </summary>
        public event Action<NPC> OnRemoveTarget;

        /// <summary>
        /// Invoked when the same target is set to <see cref="SetTarget(NPC)"/>.
        /// </summary>
        public event Action<NPC> OnSameTarget;

        /// <summary>
        /// Invoked right before the player dies; Bool value passed is whether or not the death was a custom death.
        /// </summary>
        public event Action<bool> OnBeforeDeath;

        /// <inheritdoc/>
        public override void SaveData(TagCompound tag)
        {
            tag.Set(nameof(StandID), !in_Stand ? -1 : (int)in_Stand.ID, true);
            tag.Set(nameof(DefeatedBosses), DefeatedBosses, true);
            tag.Set(nameof(ConsumedRedHotChiliPepper), ConsumedRedHotChiliPepper, true);
        }

        /// <summary>
        /// Loads the stand.
        /// </summary>
        /// <param name="tag"></param>
        public override void LoadData(TagCompound tag)
        {
            pv_LoadedStandID = (StandID)tag.GetInt(nameof(StandID));
            DefeatedBosses = new(tag.GetList<string>(nameof(DefeatedBosses)));
            ConsumedRedHotChiliPepper = tag.GetBool(nameof(ConsumedRedHotChiliPepper));
        }

        /// <summary>
        /// Sets a new <see cref="Target"/>.
        /// </summary>
        /// <param name="npc"></param>
        public void SetTarget(NPC npc)
        {
            if (npc == Target)
            {
                OnSameTarget?.Invoke(npc);
                return;
            }
            RemoveTarget(npc);
            //Debug.Log($"New Target: {npc.FullName}");
            Target = npc;
            OnSetTarget?.Invoke(npc);
        }

        /// <summary>
        /// Removes the current <see cref="Target"/> if given npc value is the same as the target.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public bool RemoveTarget(NPC npc)
        {
            if (npc == Target)
            {
                //Debug.Log($"Removed Target: {npc.FullName}");
                OnRemoveTarget?.Invoke(npc);
                Target = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Forcefully removes the <see cref="Target"/>.
        /// </summary>
        public void ForceRemoveTarget()
        {
            OnRemoveTarget?.Invoke(Target);
            Target = null;
        }

        public void AddDefeatedBoss(string name)
        {
            if (DefeatedBosses.Contains(name))
                return;

            DefeatedBosses.Add(name);
            OnNewBossDefeated?.Invoke(name);

            Debug.Log("Defeated Bosses: " + DefeatedBosses.Count);
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath)
                return new [] { new Item(ModContent.ItemType<Item_StandArrow>()) };
            
            return null;
        }

        public override void PreUpdate()
        {
            if (!in_IsStandUser || Player.dead)
                return;

            if (ASCResources.Input.Keybind_Stand_Invoke.JustPressed)
            {
                if (!in_Stand.Active)
                    in_Stand.Invoke();
                else in_Stand.Recall();
            }

            if (in_IsStandUser)
            {
                in_Stand.Update();

                if (in_Stand.Active && Player.oldPosition == Player.position && pv_MenacingCountdown)
                {
                    Dust.NewDust(Player.Bottom + new Vector2(12f, -12), 6, 3, ModContent.DustType<Dust_Stand_Menacing>(), SpeedY: -0.5f);
                    Dust.NewDust(Player.Bottom + new Vector2(-32f, -12), 6, 3, ModContent.DustType<Dust_Stand_Menacing>(), SpeedY: -0.5f);
                }
            }

            if (!Hooks.InGame.NPCExists(Target))
                ForceRemoveTarget();
        }

        public override void PostUpdate()
        {
            if (in_IsStandUser)
                in_Stand.UpdateStats();
        }

        public override void OnEnterWorld(Player player)
        {
            Stats = new EntityStats(this);

            if (pv_LoadedStandID != StandID.NEWBIE && ASCResources.Players.ManifestStand(this, pv_LoadedStandID, false) != -1)
            {
                Debug.Log($"Your will is currently manifested as {in_Stand.Name}, Level {in_Stand.Level}");
            }

            SpawnPos = player.position.ToTileCoordinates();

            //Debug.Log("Defeated Bosses: " + DefeatedBosses.Count);
            //Debug.LogEnumerable(DefeatedBosses);
        }

        public override void Initialize()
        {
            in_Stand = null;
            pv_LoadedStandID = StandID.NEWBIE;
            Stats = null;
        }

        /// <summary>
        /// Returns true if the custom death currently set on the player is the same as the custom death given.
        /// </summary>
        /// <param name="customDeath"></param>
        /// <returns></returns>
        public bool HasCustomDeath(Action<AscendedPlayer, Player> customDeath)
        {
            return  pv_CustomDeath.Contains(customDeath);
        }

        /// <summary>
        /// Adds an action which will trigger instead of the player's normal death;
        /// This custom death is removed once triggered.
        /// </summary>
        /// <param name="customDeath"></param>
        public void AddCustomDeath(Action<AscendedPlayer, Player> customDeath)
        {
            pv_CustomDeath.Add(customDeath);
        }

        /// <summary>
        /// Attempts to remove a custom death previously added through <see cref="AddCustomDeath(Action{AscendedPlayer, Player})"/>.
        /// </summary>
        /// <param name="customDeath"></param>
        /// <returns></returns>
        public bool RemoveCustomDeath(Action<AscendedPlayer, Player> customDeath)
        {
            return pv_CustomDeath.Remove(customDeath);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if(pv_CustomDeath.Count > 0)
            {
                OnBeforeDeath?.Invoke(true);

                for (int i = 0; i < pv_CustomDeath.Count; i++)
                {
                    pv_CustomDeath[i](this, Player);
                }

                pv_CustomDeath.Clear();

                return false;
            }

            OnBeforeDeath?.Invoke(false);
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void UpdateDead()
        {
            base.UpdateDead();
            if(in_IsStandUser) in_Stand.Recall();
        }

        public override void OnRespawn(Player player)
        {
            base.OnRespawn(player);
            if(in_IsStandUser) in_Stand.Invoke();
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            SetTarget(target);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            SetTarget(target);
        }

        private void OnManifestCall(int val)
        {
            OnManifestStand?.Invoke(in_Stand, val);
        }

        internal Stand in_Stand = null;
        internal bool in_IsStandUser = false;
        private StandID pv_LoadedStandID = StandID.NEWBIE;
        private ReturnCountdown pv_MenacingCountdown = 0.5f;
        private readonly List<Action<AscendedPlayer, Player>> pv_CustomDeath = new();
    }
}
