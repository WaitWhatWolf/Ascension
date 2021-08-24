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
    public sealed class DustMaker : IDustMaker
    {
        public int[] Types;
        public IntRange Amount;
        public Func<Vector2Range> Position;
        public int Width;
        public int Height;
        public Color Color;
        public FloatRange SpeedX;
        public FloatRange SpeedY;
        public IntRange Alpha;
        public FloatRange Scale;
        public Action<Dust> OnCreate;

        public void CreateDust()
        {
            Hooks.InGame.CreateDust(Amount, Position(), Width, Height, Color, SpeedX, SpeedY, Alpha, Scale, OnCreate, Types);
        }

        public DustMaker(int type, IntRange amount, Func<Vector2Range> position, int width, int height, Color color, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale, Action<Dust> onCreate = null)
        {
            Types = new int[] { type };
            Amount = amount;
            Position = position;
            Width = width;
            Height = height;
            Color = color;
            SpeedX = speedX;
            SpeedY = speedY;
            Alpha = alpha;
            Scale = scale;
            OnCreate = onCreate;
        }

        public DustMaker(int type, IntRange amount, Func<Vector2Range> position, int width, int height, Color color, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale)
        {
            Types = new int[] { type };
            Amount = amount;
            Position = position;
            Width = width;
            Height = height;
            Color = color;
            SpeedX = speedX;
            SpeedY = speedY;
            Alpha = alpha;
            Scale = scale;
        }

        public DustMaker(IntRange amount, Func<Vector2Range> position, int width, int height, Color color, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale, Action<Dust> onCreate = null, params int[] types)
        {
            Types = types;
            Amount = amount;
            Position = position;
            Width = width;
            Height = height;
            Color = color;
            SpeedX = speedX;
            SpeedY = speedY;
            Alpha = alpha;
            Scale = scale;
            OnCreate = onCreate;
        }

        public DustMaker(IntRange amount, Func<Vector2Range> position, int width, int height, Color color, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale, params int[] types)
        {
            Types = types;
            Amount = amount;
            Position = position;
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
