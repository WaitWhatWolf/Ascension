using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Internal
{
    /// <summary>
    /// Used by <see cref="ASCResources"/> to automatically create an asset.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 7:07:52")]
    public sealed class AssetCreator<T> where T : class
    {
        public Asset<T> Asset;
        public string Path;

        public void Init(Mod mod)
        {
            Asset = mod.Assets.Request<T>(Path);
        }

        public AssetCreator(string path)
        {
            Path = path;
        }

        public static implicit operator T(AssetCreator<T> asset)
            => asset.Asset.Value;

        public static implicit operator Asset<T>(AssetCreator<T> asset)
            => asset.Asset;
    }
}
