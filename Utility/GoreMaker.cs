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
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 15:51:48")]
    public class GoreMaker : IDustMaker
    {
        public int[] Types;
        public IntRange Amount;
        public Vector2Range PosVariation;
        public Vector2Range Velocity;
        public FloatRange Scale;
        public Action<Gore> OnCreate;

        public void Create(Vector2 position)
        {
            Hooks.InGame.CreateGore(Amount, new (PosVariation.Min + position, PosVariation.Max + position), Velocity.GetRandom(), Scale.GetRandom(), OnCreate, Types);
        }

        public GoreMaker(int type, IntRange amount, Vector2Range posVariation, Vector2Range velocity, FloatRange scale)
        {
            Types = new int[] { type };
            Amount = amount;
            PosVariation = posVariation;
            Velocity = velocity;
            Scale = scale;
        }

        public GoreMaker(IntRange amount, Vector2Range posVariation, Vector2Range velocity, FloatRange scale, params int[] types)
        {
            Types = types;
            Amount = amount;
            PosVariation = posVariation;
            Velocity = velocity;
            Scale = scale;
        }

        public GoreMaker(IntRange amount, Vector2Range posVariation, Vector2Range velocity, FloatRange scale, Action<Gore> onCreate, params int[] types)
        {
            Types = types;
            Amount = amount;
            PosVariation = posVariation;
            Velocity = velocity;
            Scale = scale;
            OnCreate = onCreate;
        }
    }
}
