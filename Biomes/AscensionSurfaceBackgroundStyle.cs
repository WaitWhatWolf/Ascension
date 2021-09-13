using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Biomes
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/12 18:55:35")]
    public abstract class AscensionSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
    {
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
			return BackgroundTextureLoader.GetBackgroundSlot(ASCResources.ASSETS_PATH_BIOMES 
				+ ASCResources.ASSETS_SUBPATH_BACKGROUNDS + "SurfaceClose_" + Hooks.Text.GetFormatName(this, string.Empty));
        }

        public override int ChooseMiddleTexture()
        {
			return BackgroundTextureLoader.GetBackgroundSlot(ASCResources.ASSETS_PATH_BIOMES 
				+ ASCResources.ASSETS_SUBPATH_BACKGROUNDS + "SurfaceMid_" + Hooks.Text.GetFormatName(this, string.Empty));
        }

        public override int ChooseFarTexture()
        {
			return BackgroundTextureLoader.GetBackgroundSlot(ASCResources.ASSETS_PATH_BIOMES 
				+ ASCResources.ASSETS_SUBPATH_BACKGROUNDS + "SurfaceFar_" + Hooks.Text.GetFormatName(this, string.Empty));
        }

        public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			transitionSpeed *= 0.5f;

			for (int i = 0; i < Slot; i++)
				ModifyFarFadeNonSlot(ref fades[i], transitionSpeed);

			ModifyFarFadeSlot(ref fades[Slot], transitionSpeed);

			for (int i = Slot + 1; i < fades.Length; i++)
				ModifyFarFadeNonSlot(ref fades[i], transitionSpeed);
		}

		private void ModifyFarFadeSlot(ref float fade, float transitionSpeed)
		{
			fade += transitionSpeed;
			if (fade > 1f)
				fade = 1f;
		}

		private void ModifyFarFadeNonSlot(ref float fade, float transitionSpeed)
		{
			fade -= transitionSpeed;
			if (fade < 0f)
				fade = 0f;
		}
	}
}
