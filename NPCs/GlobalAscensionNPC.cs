using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 06)]
    public sealed class GlobalAscensionNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        [Note(Dev.WaitWhatWolf, "For some reason this method doesn't work in the current tmodloader alpha patch...(28/08/21)")]
        public override void OnKill(NPC npc)
        {
            /*if (previouslyHitBy != null)
            {
                previouslyHitBy.GetModPlayer<AscendedPlayer>().RemoveTarget(npc);
                Debug.Log(previouslyHitBy.GetModPlayer<AscendedPlayer>().Target?.FullName);
            }

            if (npc.boss)
            {
                Main.player[Main.myPlayer].GetModPlayer<AscendedPlayer>().AddDefeatedBoss(npc.FullName);
            }*/
        }

        [Note(Dev.WaitWhatWolf, "All code inside this method should be in OnKill but it does not work; See note added in OnKill.")]
        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if(npc.life <= 0)
            {
                if (previouslyHitBy != null)
                {
                    previouslyHitBy.GetModPlayer<AscendedPlayer>().RemoveTarget(npc);
                }

                if (npc.boss)
                {
                    Main.player[Main.myPlayer].GetModPlayer<AscendedPlayer>().AddDefeatedBoss(npc.FullName);
                }
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            previouslyHitBy = player;
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (!projectile.npcProj)
            {
                previouslyHitBy = Main.player[projectile.owner];
            }
        }

        private Player previouslyHitBy;
    }
}
