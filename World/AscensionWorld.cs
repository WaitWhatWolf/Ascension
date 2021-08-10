using Ascension.Attributes;
using Ascension.Buffs.StandUnique;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Players;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Ascension.World
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public class AscensionWorld : ModSystem
    {
        /// <summary>
        /// Determines the active state of "ZA WORUDO" a.k.a time stop.
        /// </summary>
        public bool IsInTheWorld { get; private set; }
        public SB_TheWorld TheWorldDebuff { get; private set; }
        public Stand TheWorldPerpetrator { get; private set; }

        public event Action<AscensionWorld> OnTheWorldBegin;
        public event Action<AscensionWorld> OnTheWorldEnd;

        public void SetTheWorld(Stand perpetrator, SB_TheWorld buff)
        {
            IsInTheWorld = true;
            TheWorldDebuff = buff;
            TheWorldPerpetrator = perpetrator;

            OnTheWorldBegin?.Invoke(this);
        }

        public void StopTheWorld()
        {
            IsInTheWorld = false;
            TheWorldDebuff = null;
            TheWorldPerpetrator = null;

            OnTheWorldEnd?.Invoke(this);
        }

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
        }

        private AscendedPlayer pv_AscendedPlayer;
        private Stand pv_Stand;
        private bool pv_HasStand;
    }
}
