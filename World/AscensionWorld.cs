using Ascension.Internal;
using Ascension.Players;
using Ascension.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Ascension.World
{
    public class AscensionWorld : ModSystem
    {
        public override void OnWorldLoad()
        {
            pv_AscendedPlayer = Main.player[Main.myPlayer].GetModPlayer<AscendedPlayer>();
            pv_AscendedPlayer.OnManifestStand += Event_OnManifestStand;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Contains("Resource Bars"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "Ascension:Stand Menu",
                    delegate
                    {
                        //try
                        //{
                            if (pv_HasStand && pv_Stand.StandMenu.Active)
                            {
                                pv_Stand.StandMenu.Draw(Main.spriteBatch);
                                pv_Stand.StandMenuUI.Update(Main._drawInterfaceGameTime);
                            }
                        //}
                        //catch { }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        private void Event_OnManifestStand(Stand stand, int val)
        {
            pv_Stand = stand;
            pv_HasStand = true;
            pv_Stand.StandMenu.SetDefaultPosition(ModContent.GetInstance<AscensionConfig>().StandMenuPosX,
            ModContent.GetInstance<AscensionConfig>().StandMenuPosY);

            Debug.Log(pv_HasStand);
        }

        private AscendedPlayer pv_AscendedPlayer;
        private Stand pv_Stand;
        private bool pv_HasStand;
    }
}
