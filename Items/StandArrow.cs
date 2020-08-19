using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.Items
{
    public sealed class StandArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stand Arrow");
            Tooltip.SetDefault("<right> to manifest your will into a Stand! (WARNING: PERMANENT EFFECT)");
        }

        public override void SetDefaults()
        {
            item.value = 0;
            item.useStyle = 1;
            item.width = item.height = 20;
            item.rare = 10;
        }

        //public override string Texture => "WarWolfWorks_Mod\\Items\\StandArrow.png";

        public override bool CanRightClick() => true;

        private (string name, StandType stand)[] StandIndexing = new (string name, StandType index)[]
        {
            ("JOTARO", StandType.STAR_PLATINUM),
            ("DIO", StandType.THE_WORLD),
            ("AVDOL", StandType.MAGICIANS_RED),
            ("KIRA", StandType.KILLER_QUEEN),
            ("KAKYOIN", StandType.HIEROPHANT_GREEN)
        };

        private StandType GetStandTypeByName(Player player)
        {
            for(int i = 0; i < StandIndexing.Length; i++)
            {
                if (player.name.ToUpper().Contains(StandIndexing[i].name))
                    return StandIndexing[i].stand;
            }

            return StandType.NEWBIE;
        }

        public override void RightClick(Player player)
        {
            WWWPlayer wwwplayer = player.GetModPlayer<WWWPlayer>();
            if (wwwplayer.Stand != null)
                return;
            player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name}'s will was not strong enough to manifest a Stand while surviving."), player.statLife - 1, player.direction);

            wwwplayer.ManifestStand(GetStandTypeByName(player));
        }
    }
}
