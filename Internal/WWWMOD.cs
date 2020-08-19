using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using WarWolfWorks_Mod.UI;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// WarWolfWorks core mod class.
    /// </summary>
    public sealed class WWWMOD : Mod
    {
        public WWWMOD Instance { get; set; }

        /// <summary>
        /// Determines if the time is currently stopped; Used for <see href="https://jojo.fandom.com/wiki/The_World?file=ZA_WARUDO2.gif">ザ・ワールド</see> of 
        /// <see href="https://jojo.fandom.com/wiki/The_World">Star Platinum</see> and <see href="https://jojo.fandom.com/wiki/The_World">The World</see>.
        /// </summary>
        public bool TimeIsStopped { get; private set; }
        /// <summary>
        /// Invoked when <see cref="StopTime(StandType)"/> is called.
        /// </summary>
        public event Action<StandType> OnTimeStopped;
        /// <summary>
        /// Invoked when <see cref="ResumeTime(StandType)"/> is called.
        /// </summary>
        public event Action<StandType> OnTimeResumed;
        
        /// <summary>
        /// Time at which <see cref="StopTime"/> was called.
        /// </summary>
        public double StoppedTime { get; private set; }

        public StandMenu StandMenu { get; private set; }
        public UserInterface StandUI { get; private set; }

        public WWWMOD()
        {
            Instance = this;
        }

        public override void LoadResources()
        {
            if (!Main.dedServ)
            {
                R_Load_StandMenu();
            }
        }

        public void R_Load_StandMenu()
        {
            StandMenu = new StandMenu();
            StandMenu.ActivateMenu();
            StandUI = new UserInterface();
            StandUI.SetState(StandMenu);
        }

        public void StopTime(StandType perpetrator)
        {
            OnTimeStopped?.Invoke(perpetrator);
            TimeIsStopped = true;
            StoppedTime = Main.time;
        }

        public void ResumeTime(StandType perpetrator)
        {
            OnTimeResumed?.Invoke(perpetrator);
            TimeIsStopped = false;
            StoppedTime = 0;
        }

        /// <summary>
        /// Freezes the time.
        /// </summary>
        public override void MidUpdateInvasionNet()
        {
            if (TimeIsStopped)
                Main.time = StoppedTime;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (StandUI != null)
                StandUI.Update(gameTime);
        }
    }
}
