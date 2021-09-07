using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using Ascension.Internal;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;
using static Ascension.ASCResources.Textures;

namespace Ascension.UI
{
    /// <summary>
    /// Menu which displays all info of a <see cref="Stand"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class Menu_Stand : Menu
    {
        public override float Scale
        {
            get => pv_Cfg.StandMenuScale;
            set
            {
                pv_Cfg.StandMenuScale = value;
                ScaleMenu();
            }
        }

        public void SetDefaultPosition(int x, int y)
        {
            pv_Cfg.StandMenuPosX = x;
            pv_Cfg.StandMenuPosY = y;

            ResetDimensions();
        }

        protected override float DimensionHeight => 130 * Scale;
        protected override float DimensionWidth
        {
            get
            {
                return 390 * Scale;
                //int abilitiesLength = pv_Stand?.Abilities.Length ?? 3;
                //return (130 + (65 * abilitiesLength)) * Scale;
            }
        }
        protected override float DimensionLeft => pv_Cfg.StandMenuPosX;
        protected override float DimensionTop => pv_Cfg.StandMenuPosY;

        protected override void OnActiveDrawSelf(SpriteBatch spriteBatch)
        {
            pv_BackgroundImage.Color = pv_Stand.ThemeColor;

            for (int i = 0; i < pv_Stand.Abilities.Length; i++)
            {
                if (!pv_Stand.Owner.UnlockedStandAbility[i])
                {
                    pv_AbilityImages[i].Color = Color.Black;
                    continue;
                }

                StandAbility ability = pv_Stand.Abilities[i];

                float abilityCurrentCooldown = ability.GetCurrentCountdown();
                pv_AbilityCooldowns[i].SetText(ability is IAbilityHideCountdown hide && hide.HideCountdown() 
                    ? string.Empty 
                    : abilityCurrentCooldown <= 0 || ability.CountdownReady
                    ? string.Empty 
                    : abilityCurrentCooldown.Truncate(1).ToString());

                pv_AbilityImages[i].Color = ability.CountdownReady ? Color.White : Color.Brown;

                pv_AbilityToggles[i].Rotation += ASCResources.FLOAT_PER_FRAME;
                pv_AbilityToggles[i].Color = ability.Active ? pv_Stand.ThemeColor : Color.Transparent;
            }
        }

        [Obsolete("Not needed for now", true)]
        public void Deinit()
        {
            RemoveAllChildren();
            Remove();
            Recalculate();
            
            if (pv_Stand)
            {
                pv_Stand.OnAbilityUnlock -= Event_OnAbilityUnlock;
                pv_Stand = null;
            }
            pv_AbilityCooldowns = null;
            pv_AbilityImages = null;
            pv_AbilityKeys = null;
            pv_BackgroundImage = null;
            pv_PortraitImage = null;
            pv_TooltipText = null;
        }

        public Menu_Stand(Stand stand) : base(false)
        {
            pv_Cfg = ModContent.GetInstance<AscensionConfig>();

            pv_Stand = stand;
            pv_Stand.OnAbilityUnlock += Event_OnAbilityUnlock;

            pv_AbilityImages = new UIImage[pv_Stand.Abilities.Length];
            pv_AbilityKeys = new UIText[pv_Stand.Abilities.Length];
            pv_AbilityCooldowns = new UIText[pv_Stand.Abilities.Length];
            pv_AbilityToggles = new UIImage[pv_Stand.Abilities.Length];

            pv_BackgroundImage = new UIImage(GetTexture(STAND_MENU_BACKGROUND));
            pv_BackgroundImage.Color = pv_Stand.ThemeColor;
            pv_BackgroundImage.IgnoresMouseInteraction = true;
            pv_BackgroundImage.ScaleToFit = true;
            Append(pv_BackgroundImage);

            pv_PortraitImage = new UIImage(pv_Stand.Portrait);
            pv_PortraitImage.OnMouseOver += Event_PortraitOnMouseOver;
            pv_PortraitImage.OnMouseOut += Event_AbilityOnMouseOut;
            pv_PortraitImage.ScaleToFit = true;
            Append(pv_PortraitImage);

            for (int i = 0; i < pv_Stand.Abilities.Length; i++)
            {
                UIImage image = new(pv_Stand.Abilities[i].Icon);
                image.Color = Color.White;
                image.OnMouseOver += Event_AbilityOnMouseOver;
                image.OnMouseOut += Event_AbilityOnMouseOut;
                image.ScaleToFit = true;
                image.Deactivate();

                //this.Append(image);

                /*if (i != 0)
                {
                    UIText textKey = new(ASCResources.Input.GetStandAbilityKey(i).GetAssignedKeys().First(), 0.75f, true);
                    textKey.Left.Set(0, 0.2f);
                    textKey.Top.Set(0, 0.2f);
                    textKey.TextColor = new(Hooks.Colors.Tangelo.R, Hooks.Colors.Tangelo.G, Hooks.Colors.Tangelo.B, 100);
                    textKey.IgnoresMouseInteraction = true;
                    image.Append(textKey);

                    pv_AbilityKeys[i - 1] = textKey;
                }*/

                UIText textCooldown = new(string.Empty, 0.4f, true);
                textCooldown.Left.Set(0, 0f);
                textCooldown.Top.Set(0, 0f);
                textCooldown.Width.Set(0, 1f);
                textCooldown.Height.Set(0, 1f);
                textCooldown.TextColor = Hooks.Colors.Tooltip_Stand_Ability_Cooldown;
                textCooldown.IgnoresMouseInteraction = true;
                textCooldown.DynamicallyScaleDownToWidth = true;
                image.Append(textCooldown);
                pv_AbilityCooldowns[i] = textCooldown;

                pv_AbilityImages[i] = image;

                UIImage toggleImg = new(GetTexture(STAND_MENU_TOGGLE));
                toggleImg.Left.Set(0, 0.0f);
                toggleImg.Top.Set(0, 0.0f);
                toggleImg.Color = new Color(255, 215, 0, 125);
                toggleImg.IgnoresMouseInteraction = true;
                toggleImg.NormalizedOrigin = Vector2.One * 0.5f;
                image.Append(toggleImg);
                pv_AbilityToggles[i] = toggleImg;
            }

            pv_TooltipText = new(string.Empty, Scale, false);
            pv_TooltipText.IgnoresMouseInteraction = true;
            pv_TooltipText.Deactivate();

            ScaleMenu();
        }

        private void ScaleMenu()
        {
            pv_BackgroundImage.ImageScale = Scale;
            pv_BackgroundImage.Left.Set(0, 0f);
            pv_BackgroundImage.Width.Set(DimensionWidth, 0f);
            pv_BackgroundImage.Top.Set(0, 0f);
            pv_BackgroundImage.Height.Set(DimensionHeight, 0f);

            pv_PortraitImage.Left.Set(1 * Scale, 0f);
            pv_PortraitImage.Width.Set(128f * Scale, 0f);
            pv_PortraitImage.Top.Set(1 * Scale, 0f);
            pv_PortraitImage.Height.Set(128 * Scale, 0f);
            pv_PortraitImage.ImageScale = Scale;

            for (int i = 0; i < pv_AbilityImages.Length; i++)
            {
                pv_AbilityImages[i].Left.Set((130 + (65 * i)) * Scale, 0f);
                pv_AbilityImages[i].Width.Set(64 * Scale, 0f);
                pv_AbilityImages[i].Top.Set(-65 * Scale, 1f);
                pv_AbilityImages[i].Height.Set(64 * Scale, 0f);
                pv_AbilityImages[i].ImageScale = Scale;

                pv_AbilityToggles[i].ImageScale = Scale;
            }

            pv_TooltipText.TextScale = Scale;
        }

        private void Event_AbilityOnMouseOver(UIMouseEvent mEvent, UIElement element)
        {
            pv_MouseOverIndex = Array.IndexOf(pv_AbilityImages, mEvent.Target);

            UpdateTooltip();
        } 
        
        private void Event_PortraitOnMouseOver(UIMouseEvent mEvent, UIElement element)
        {
            pv_MouseOverIndex = -2;

            UpdateTooltip();
        }
        
        private void Event_AbilityOnMouseOut(UIMouseEvent mEvent, UIElement element)
        {
            pv_MouseOverIndex = -1;

            UpdateTooltip();
        }

        private void Event_OnAbilityUnlock(int index)
        {
            if (index < pv_AbilityImages.Length)
            {
                Append(pv_AbilityImages[index]);
                pv_AbilityImages[index].Activate();
            }
        }

        private void UpdateTooltip()
        {

            if (pv_MouseOverIndex == -1)
            {
                pv_TooltipText.Deactivate();
                pv_TooltipText.Remove();

                return;
            }

            if (!HasChild(pv_TooltipText))
                Append(pv_TooltipText);

            pv_TooltipText.Activate();
            

            float left = pv_MouseOverIndex switch
            {
                -2 => 0,
                _ => (130f + (66f * pv_MouseOverIndex)) * Scale,
            };

            float top = pv_MouseOverIndex switch //This is here for later use...probably.
            {
                _ => 130f * Scale,
            };

            string text = pv_MouseOverIndex switch
            {
                -2 => pv_Stand.Tooltip,
                _ => pv_Stand.Abilities[pv_MouseOverIndex].TooltipText
            };

            Vector2 size = FontAssets.MouseText.Value.MeasureString(Hooks.Colors.GetUncoloredTooltipText(text));
            size.Y *= Scale;

            pv_TooltipText.SetText(text);
            pv_TooltipText.Left.Set(left, 0f);
            pv_TooltipText.Top.Set(top, 0f);
            pv_TooltipText.Width.Set(size.X, 0f);
            pv_TooltipText.Height.Set(size.Y + 8f, 0f);
            pv_TooltipText.MaxWidth.Set(size.X, 0f);
            pv_TooltipText.MaxHeight.Set(size.Y + 8f, 0f);

            pv_TooltipText.Recalculate();
        }

        private Stand pv_Stand;
        private readonly AscensionConfig pv_Cfg;

        private UIImage pv_BackgroundImage;
        private UIImage pv_PortraitImage;
        private UIImage[] pv_AbilityImages;
        private UIImage[] pv_AbilityToggles;
        private UIText[] pv_AbilityCooldowns;
        private UIText[] pv_AbilityKeys;
        private UITextPanel<string> pv_TooltipText;

        private int pv_MouseOverIndex;

        private float pv_X;
        private float pv_Y;
    }
}
