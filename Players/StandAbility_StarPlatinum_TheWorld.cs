using Ascension.Attributes;
using Ascension.Buffs.StandUnique;
using Ascension.Enums;
using Ascension.Utility;
using Ascension.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Ascension.Players
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 09)]
    public sealed class StandAbility_StarPlatinum_TheWorld : StandAbility
    {
        public override string Name { get; } = "Star Platinum: The World";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Stops time.", Hooks.Colors.Tooltip_Effect)
            + "\nDuring the time stop, any damage\n"
            + "Taken by enemies will be stored\n"
            + "at 150% it's value.\n"
            + "At the end of the time stop,\n"
            + "all enemies will recieve the\n"
            + "stored damage.\n"
            + Hooks.Colors.GetColoredTooltipText($"Cooldown: {GetCooldown()}", Hooks.Colors.Tooltip_Stand_Ability_Cooldown);

        public override Asset<Texture2D> Icon => ASCResources.Textures.Stand_Ability_StarPlatinum_TheWorld;

        protected override float Cooldown { get; } = 60f;

        protected override bool ActivateCondition() => CooldownReady;

        protected override bool DeactivateCondition() => pv_Duration < 0f;

        protected override void OnActivate()
        {
            pv_CurrentDuration = pv_Duration;

            AscensionWorld world = ModContent.GetInstance<AscensionWorld>();
            world.SetTheWorld(Stand, new SB_TheWorld(world, Stand));
            ResetCooldown();
        }

        protected override void OnDeactivate()
        {
            ModContent.GetInstance<AscensionWorld>().StopTheWorld();
        }

        public override void Update()
        {
            base.Update();

            if(Active)
            {
                pv_Duration -= ASCResources.FLOAT_PER_FRAME;
            }
        }

        public StandAbility_StarPlatinum_TheWorld(Stand stand) : base(stand) { }

        private float pv_Duration = 10f;
        private float pv_CurrentDuration;
    }
}
