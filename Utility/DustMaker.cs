using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Utility
{
    /// <summary>
    /// Makes a dust "explosion".
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/23 20:44:35")]
    public sealed record DustMaker : IDustMaker
    {
        public int[] Types;
        public IntRange Amount;
        public Vector2Range PosVariation;
        public int Width;
        public int Height;
        public Color Color;
        public FloatRange SpeedX;
        public FloatRange SpeedY;
        public IntRange Alpha;
        public FloatRange Scale;
        public Action<Dust> OnCreate;

        public void Create(Vector2 position)
        {
            Hooks.InGame.CreateDust(Amount, position + PosVariation.GetRandom(), Width, Height, Color, SpeedX, SpeedY, Alpha, Scale, OnCreate, Types);
        }

        public DustMaker(IntRange amount, Vector2Range posVariation, int width, int height, Color color, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale, Action<Dust> onCreate = null, params int[] types)
        {
            Types = types;
            Amount = amount;
            PosVariation = posVariation;
            Width = width;
            Height = height;
            Color = color;
            SpeedX = speedX;
            SpeedY = speedY;
            Alpha = alpha;
            Scale = scale;
            OnCreate = onCreate;
        }

        public DustMaker(IntRange amount, Vector2Range posVariation, int width, int height, Color color, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale, params int[] types)
        {
            Types = types;
            Amount = amount;
            PosVariation = posVariation;
            Width = width;
            Height = height;
            Color = color;
            SpeedX = speedX;
            SpeedY = speedY;
            Alpha = alpha;
            Scale = scale;
        }
    }
}
