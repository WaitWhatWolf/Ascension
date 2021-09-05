using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Dusts;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 22:13:41")]
    public class StandAbility_KillerQueen_BitesTheDust : StandAbility
    {
        public StandAbility_KillerQueen_BitesTheDust(Stand stand, int index) : base(stand, index) { }

        public override string Name { get; } = "Bites The Dust";

        public override string Description => "When used, all enemies on screen are remembered;"
            + "\nIf all remembered enemies die, this ability is "
            + Hooks.Colors.GetColoredTooltipText("set on a reduced cooldown", Hooks.Colors.Tooltip_Effect) + ".\n"
            + Hooks.Colors.GetColoredTooltipText(Name, Hooks.Colors.Tooltip_Stand_Ability)
            + " " + Hooks.Colors.GetColoredTooltipText("marks", Hooks.Colors.Tooltip_Effect) + " the "
            + Hooks.Colors.GetColoredTooltipText("strongest remembered enemy", Hooks.Colors.Tooltip_Stat) + ";"
            + "\nIf you are killed in the presence of the " 
            + Hooks.Colors.GetColoredTooltipText("marked", Hooks.Colors.Tooltip_Effect) + " enemy,"
            + "\nthe enemy will " + Hooks.Colors.GetColoredTooltipText("explode", Hooks.Colors.Tooltip_Effect) + "; If it was a " 
            + "boss, the explosion deals"
            + "\n"
            + Hooks.Colors.GetColoredTooltipText("10% of it's max health as damage", Hooks.Colors.Tooltip_Stat)
            + ", otherwise, the enemy"
            + "\nis "+ Hooks.Colors.GetColoredTooltipText("instantly killed", Hooks.Colors.Tooltip_Effect)
            + ".\n\nWhen the explosion successfully triggers, you will\n"
            + Hooks.Colors.GetColoredTooltipText("teleport back to spawn", Hooks.Colors.Tooltip_Effect) 
            + " instead of " + Hooks.Colors.GetColoredTooltipText("dying", Hooks.Colors.Tooltip_Debuff) + '.'
            + "\n\n"
            + Hooks.Colors.GetColoredTooltipMultilineText("Bites the Dust emerged from my single-minded desire not to encounter you..." 
                + "\nBring it on! Let's see you stop time! How long can you keep it up?", Hooks.Colors.Tooltip_Quote);

        public override Asset<Texture2D> Icon => ASCResources.Textures.GetTexture(ASCResources.Textures.STAND_ABILITY_KILLERQUEEN_ULTIMATE);

        public override void Update()
        {
            base.Update();

            if(Active)
            {
                Countdown.Reset();

                foreach (NPC npc in pv_NPCs)
                    (npc == pv_Target
                        ? ASCResources.Dusts.Dust_Stand_KillerQueen_BitesTheDust_Mark
                        : ASCResources.Dusts.Dust_Stand_KillerQueen_BitesTheDust_Remember)
                        .Create(npc.Top + ASCResources.Players.Stand_KillerQueen_BitesTheDust_MarkOffset);

                if (!Hooks.InGame.NPCExists(pv_Target))
                    SeekTarget();

                if(BuffAvailable)
                {
                    Stand.Owner.Player.AddBuff(ModContent.BuffType<Buff_BitesTheDust>(), 60);
                }
            }
        }

        public bool BuffAvailable => Active && pv_Target != null && pv_Target.Center.Distance(Stand.Owner.Player.Center) < 1000f;

        protected override ReturnCountdown Countdown { get; } = new(60f, true);

        protected override bool ActivateCondition() 
        {
            if (CountdownReady && !Active)
            {
                PopulateNPCList();
                return pv_NPCs.Count > 0;
            }

            return false;
        }

        protected override bool DeactivateCondition() => pv_Death;

        protected override void OnActivate()
        {
            ResetCountdown();
            SeekTarget();

            Stand.Owner.OnBeforeDeath += Event_OnBeforeDeath;

            ASCResources.Sound.biteZaDasuto.Play();
        }

        private void Event_OnBeforeDeath(bool obj)
        {
            pv_Death = obj;
        }

        protected override void OnDeactivate()
        {
            Stand.Owner.OnBeforeDeath -= Event_OnBeforeDeath;

            pv_Target?.StrikeNPC(pv_Target.boss ? (pv_Target.lifeMax / 10) : (pv_Target.life + pv_Target.defense), 0f, 0);

            pv_Target = null;
            pv_NPCs = null;

            pv_Death = false;

            Countdown.Reset();
        }

        private void SeekTarget()
        {
            NPC target = null;
            float prev = float.NegativeInfinity;
            if (pv_NPCs.Count > 0)
                pv_NPCs.RemoveAll(npc => !Hooks.InGame.NPCExists(npc));
            else
            {
                ForceDeactivate();
                Countdown.SetCurrentCountdown(10f);
                return;
            }

            foreach (NPC npc in pv_NPCs)
            {
                int comparer = npc.friendly ? 1 : npc.life * (npc.damage / 2);
                if (prev < comparer)
                {
                    prev = comparer;
                    target = npc;
                }
            }

            pv_Target = target;
        }

        private void PopulateNPCList()
        {
            pv_NPCs = Hooks.InGame.GetAllWithin(Stand.Owner.Player, Stand.Owner.Player.Center, 1000f);
        }

        private List<NPC> pv_NPCs;
        private NPC pv_Target;
        private bool pv_Death;
    }
}
