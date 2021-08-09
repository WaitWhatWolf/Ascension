using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework.Audio;
using System;
using Terraria.ModLoader;

namespace Ascension.Sound
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08), Obsolete("Sound does not work in tmodloader 1.4 alpha")]
    public class AscensionSound : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            if (!pv_OverlapSound && soundInstance.State == SoundState.Playing)
                soundInstance.Stop();

            soundInstance.Play();
            soundInstance.Volume = volume;
            soundInstance.Pan = pan;
            
            return soundInstance;
        }

        public AscensionSound(bool overlapSound)
        {
            pv_OverlapSound = overlapSound;
        }

        private readonly bool pv_OverlapSound;
    }
}
