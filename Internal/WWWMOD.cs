using System;
using Terraria;
using Terraria.ModLoader;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// WarWolfWorks core mod class.
    /// </summary>
    public sealed class WWWMOD : Mod
    {
        /// <summary>
        /// Determines if the time is currently stopped; Used for <see href="https://jojo.fandom.com/wiki/The_World?file=ZA_WARUDO2.gif">ザ・ワールド</see> of 
        /// <see href="https://jojo.fandom.com/wiki/The_World">Star Platinum</see> and <see href="https://jojo.fandom.com/wiki/The_World">The World</see>.
        /// </summary>
        public bool TimeIsStopped { get; private set; }
        /// <summary>
        /// Invoked when <see cref="StopTime(StandType)"/> is called.
        /// </summary>
        public event Action<WWWMOD, StandType> OnTimeStopped;
        /// <summary>
        /// Invoked when <see cref="ResumeTime(StandType)"/> is called.
        /// </summary>
        public event Action<WWWMOD, StandType> OnTimeResumed;
        
        /// <summary>
        /// Time at which <see cref="StopTime"/> was called.
        /// </summary>
        public double StoppedTime { get; private set; }

        /// <summary>
        /// Stops time.
        /// </summary>
        /// <param name="perpetrator"></param>
        public void StopTime(StandType perpetrator)
        {
            OnTimeStopped?.Invoke(this, perpetrator);
            TimeIsStopped = true;
            StoppedTime = Main.time;
        }

        /// <summary>
        /// Resumes time.
        /// </summary>
        /// <param name="perpetrator"></param>
        public void ResumeTime(StandType perpetrator)
        {
            OnTimeResumed?.Invoke(this, perpetrator);
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
    }
}
