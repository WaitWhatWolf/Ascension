using Ascension.Interfaces;
using Ascension.Players;
using Ascension.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Ascension.Debuffs.StandUnique
{
    /// <summary>
    /// Base class used for custom debuffs in relation to <see cref="StandHandlerNPC"/>.
    /// </summary>
    public abstract class StandDebuff : IStandReferencable
    {
        public Stand Stand { get; }

        public StandHandlerNPC Parent { get; internal set; }

        /// <summary>
        /// If true, any NPC will stop having their AI ran.
        /// </summary>
        /// <returns></returns>
        public abstract bool StopsAI();

        /// <summary>
        /// When this returns true, a <see cref="StandHandlerNPC"/> will remove this debuff.
        /// </summary>
        /// <returns></returns>
        public abstract bool AllowRemove();

        /// <summary>
        /// If <see cref="StopsAI"/> returns true, this AI will be used instead.
        /// </summary>
        public virtual void CustomAI(NPC npc) { }

        /// <summary>
        /// Runs every frame.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Called when the debuff is initiated, but right before it is added to the list of debuffs.
        /// </summary>
        public virtual void Init() { }

        public StandDebuff(Stand stand)
        {
            Stand = stand;
        }
    }
}
