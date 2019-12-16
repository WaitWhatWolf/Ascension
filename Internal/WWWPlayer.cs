using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WarWolfWorks_Mod.Items;

namespace WarWolfWorks_Mod.Internal
{
    public sealed class WWWPlayer : ModPlayer
    {
        public Stand Stand { get; set; }

        public static ModHotKey SummonStandKey,
            AbilityKey,
            UltimateKey;

        public override TagCompound Save()
        {
            return new TagCompound()
            {
                ["Stand"] = !Stand ? 0 : (int)Stand.ID
            };
        }

        public override void Load(TagCompound tag)
        {
            Stand = Stand.GetStandByID((StandType)tag.GetInt("Stand"), this);
            if(Stand) Stand.DefineStand(player);
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            if (mediumcoreDeath)
                return;

            Item toAdd = new Item();
            toAdd.SetDefaults(ModContent.ItemType<StandArrow>());
            toAdd.stack = 1;
            items.Add(toAdd);
        }

        /// <summary>
        /// Gives the player a stand. NEWBIE by default, which chooses a random stand.
        /// </summary>
        /// <param name="specific"></param>
        public void ManifestStand(StandType specific = StandType.NEWBIE)
        {
            StandType toUse = specific;
            if(toUse == StandType.NEWBIE)
            {
                Random r = new Random();
                toUse = (StandType)r.Next(1, (int)StandType.THE_WORLD);
            }

            Stand = Stand.GetStandByID(toUse, this);
            Stand.DefineStand(player);
            string standUse = Stand != null ? Stand.Name : "none";
            Main.NewText($"{player.name} has manifested {standUse}!", 175, 75, 255);
        }

        public override void PreUpdate()
        {
            try
            {
                //PlayerStand.Control.OnUpdate();
                //PlayerStand.Ability.OnUpdate();
            }
            catch
            //catch(Exception e)
            {
                //Console.WriteLine(e);
                return;
            }
        }

        public static implicit operator bool(WWWPlayer player)
            => player != null;
    }
}
