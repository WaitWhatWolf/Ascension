using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
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
        protected override float DimensionHeight => 68;
        protected override float DimensionWidth
        {
            get
            {
                int abilitiesLength = pv_Stand?.Abilities.Length ?? 3;
                return 66 + (34 * abilitiesLength);
            }
        }
        protected override float DimensionLeft => pv_X;
        protected override float DimensionTop => pv_Y;

        public void SetDefaultPosition(int x, int y)
        {
            pv_X = x;
            pv_Y = y;

            ResetDimensions();
        }

        protected override void OnActiveDrawSelf(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < pv_Stand.Abilities.Length; i++)
            {
                if (!pv_Stand.Owner.UnlockedStandAbility[i])
                    continue;

                StandAbility ability = pv_Stand.Abilities[i];

                float abilityCurrentCooldown = ability.GetCurrentCountdown();
                pv_AbilityCooldowns[i].SetText(ability is IAbilityHideCountdown hide && hide.HideCountdown() 
                    ? string.Empty 
                    : abilityCurrentCooldown <= 0 || ability.CountdownReady
                    ? string.Empty 
                    : abilityCurrentCooldown.Truncate(1).ToString());
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

        public Menu_Stand(Stand stand) : base()
        {
            pv_Stand = stand;
            pv_Stand.OnAbilityUnlock += Event_OnAbilityUnlock;

            pv_AbilityImages = new UIImage[pv_Stand.Abilities.Length];
            pv_AbilityKeys = new UIText[pv_Stand.Abilities.Length];
            pv_AbilityCooldowns = new UIText[pv_Stand.Abilities.Length];

            pv_BackgroundImage = new UIImage(GetTexture(STAND_MENU_BACKGROUND));
            pv_BackgroundImage.Left.Set(0, 0f);
            pv_BackgroundImage.Width.Set(DimensionWidth, 0f);
            pv_BackgroundImage.Top.Set(0, 0f);
            pv_BackgroundImage.Height.Set(DimensionHeight, 0f);
            pv_BackgroundImage.Color = Hooks.Colors.Tangelo;
            pv_BackgroundImage.IgnoresMouseInteraction = true;
            Append(pv_BackgroundImage);

            pv_PortraitImage = new UIImage(pv_Stand.Portrait);
            pv_PortraitImage.Left.Set(2, 0f);
            pv_PortraitImage.Width.Set(64, 0f);
            pv_PortraitImage.Top.Set(2, 0f);
            pv_PortraitImage.Height.Set(64, 0f);
            pv_PortraitImage.OnMouseOver += Event_PortraitOnMouseOver;
            pv_PortraitImage.OnMouseOut += Event_AbilityOnMouseOut;
            Append(pv_PortraitImage);

            for (int i = 0; i < pv_Stand.Abilities.Length; i++)
            {
                UIImage image = new(pv_Stand.Abilities[i].Icon);
                image.Left.Set(67 + (34 * i), 0f);
                image.Width.Set(32, 0f);
                image.Top.Set(34, 0f);
                image.Height.Set(32, 0f);
                image.Color = Color.White;
                image.OnMouseOver += Event_AbilityOnMouseOver;
                image.OnMouseOut += Event_AbilityOnMouseOut;
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
                textCooldown.Left.Set(0, 0.5f);
                textCooldown.Top.Set(0, 0.25f);
                textCooldown.TextColor = Color.WhiteSmoke;
                textCooldown.IgnoresMouseInteraction = true;
                image.Append(textCooldown);
                pv_AbilityCooldowns[i] = textCooldown;

                pv_AbilityImages[i] = image;
            }

            pv_TooltipText = new(string.Empty, 1f, false);
            pv_TooltipText.IgnoresMouseInteraction = true;
            pv_TooltipText.Deactivate();
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
            

            int left = pv_MouseOverIndex switch
            {
                0 => 99,
                1 => 122,
                2 => 155,
                3 => 188,
                -2 => 66,
                _ => 0,
            };

            int top = pv_MouseOverIndex switch //This is here for later use...probably.
            {
                _ => 64,
            };

            string text = pv_MouseOverIndex switch
            {
                0 => pv_Stand.Abilities[0].TooltipText,
                1 => pv_Stand.Abilities[1].TooltipText,
                2 => pv_Stand.Abilities[2].TooltipText,
                3 => pv_Stand.Abilities[3].TooltipText,
                -2 => pv_Stand.Description,
                _ => string.Empty
            };

            Vector2 size = FontAssets.MouseText.Value.MeasureString(Hooks.Colors.GetUncoloredTooltipText(text));

            pv_TooltipText.SetText(text);
            pv_TooltipText.Left.Set(left, 0f);
            pv_TooltipText.Top.Set(top, 0f);
            pv_TooltipText.MaxWidth.Set(size.X, 0f);
            pv_TooltipText.Width.Set(size.X, 0f);
            pv_TooltipText.MaxHeight.Set(size.Y + 8f, 0f);
            pv_TooltipText.Height.Set(size.Y + 8f, 0f);

            pv_TooltipText.Recalculate();
        }

        private Stand pv_Stand;
        private UIImage pv_BackgroundImage;
        private UIImage pv_PortraitImage;
        private UIImage[] pv_AbilityImages;
        private UIText[] pv_AbilityCooldowns;
        private UIText[] pv_AbilityKeys;
        private UITextPanel<string> pv_TooltipText;

        private int pv_MouseOverIndex;

        private float pv_X;
        private float pv_Y;
    }
}
