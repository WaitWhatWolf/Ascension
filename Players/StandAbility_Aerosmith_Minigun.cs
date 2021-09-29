using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/29 9:10:52")]
    public class StandAbility_Aerosmith_Minigun : StandAbility_DefaultMovement
    {
        public StandAbility_Aerosmith_Minigun(Stand stand, int index) : base(stand, index)
        {
        }

        public override string Name => "Minigunning";
        public override string Description => "";
        public override Asset<Texture2D> Icon => ASCResources.Textures.Default;

        protected override float StandMoveAttackRange => pv_StandAttackRange;
        protected override float StandMoveNPCDetectionRange => pv_StandNPCDetectionRange;
        protected override float StandMoveSpeed => pv_StandMoveSpeed;

        protected override ReturnCountdown Countdown => pv_Countdown;

        protected override bool ActivateCondition()
        {
            throw new NotImplementedException();
        }

        protected override bool DeactivateCondition()
        {
            throw new NotImplementedException();
        }

        protected override void OnActivate()
        {
            throw new NotImplementedException();
        }

        protected override void OnDeactivate()
        {
            throw new NotImplementedException();
        }

        private ReturnCountdown pv_Countdown;
        private int pv_StandDamage;
        private float pv_StandAttackRange;
        private float pv_StandNPCDetectionRange;
        private float pv_StandMoveSpeed;
    }
}
