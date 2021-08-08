using Ascension;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Ascension.UI
{
    public abstract class Menu : UIState
    {
        #region Static
        /// <summary>
        /// All menus currently initiated.
        /// </summary>
        internal static List<Menu> AllMenus { get; } = new List<Menu>();

        /// <summary>
        /// Returns the first menu found of the given generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetMenu<T>() where T : Menu
        {
            return AllMenus.Find(m => m is T) as T;
        }

        /// <summary>
        /// Activates the first menu found of the given generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ActivateMenu<T>() where T : Menu
        {
            T toUse = GetMenu<T>();
            toUse.ActivateMenu();
        }

        /// <summary>
        /// Deactivates the first menu found of the given generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DeactivateMenu<T>() where T : Menu
        {
            T toUse = GetMenu<T>();
            toUse.DeactivateMenu();
        }
        #endregion

        /// <summary>
        /// Returns the active state of this menu.
        /// </summary>
        public bool Active { get; private set; }
        
        /// <summary>
        /// Returns true if this menu is currently dragged with the mouse.
        /// </summary>
        protected bool Dragged { get; private set; }

        private Vector2 Offset;

        protected virtual float DimensionWidth => 1920;
        protected virtual float DimensionHeight => 1080;
        protected virtual float DimensionTop => 0;
        protected virtual float DimensionLeft => 0;

        /// <summary>
        /// Adds an automatic handling of menu dragging.
        /// </summary>
        public override void OnInitialize()
        {
            OnMouseDown += new MouseEvent(DragStart);
            OnMouseUp += new MouseEvent(DragEnd);
        }

        private void DragStart(UIMouseEvent @event, UIElement element)
        {
            Offset = new Vector2(@event.MousePosition.X - Left.Pixels, @event.MousePosition.Y - Top.Pixels);
            Dragged = true;
        }

        private void DragEnd(UIMouseEvent @event, UIElement element)
        {
            Vector2 end = @event.MousePosition;
            Left.Set(end.X - Offset.X, 0f);
            Top.Set(end.Y - Offset.Y, 0f);
            Offset = Vector2.Zero;
            Dragged = false;

            Recalculate();
            RecalculateChildren();

            //Debug.Log($"{Left.Pixels} : {Top.Pixels}");
        }

        /// <summary>
        /// Handles position change of the menu based on mousedrag.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected sealed override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                base.DrawSelf(spriteBatch);
                Vector2 mousePos = ASCResources.MousePos;
                if (Dragged)
                {
                    Left.Set(mousePos.X - Offset.X, 0f);
                    Top.Set(mousePos.Y - Offset.Y, 0f);

                    Recalculate();
                }

                OnActiveDrawSelf(spriteBatch);
            }
        }

        public void ResetDimensions()
        {
            Width.Set(DimensionWidth, 0);
            Height.Set(DimensionHeight, 0);
            Top.Set(DimensionTop, 0);
            Left.Set(DimensionLeft, 0);

            Recalculate();
        }

        protected virtual void OnActiveDrawSelf(SpriteBatch spriteBatch) { }

        /// <summary>
        /// Activates this menu, making it draw on the screen.
        /// </summary>
        public void ActivateMenu()
        {
            if (Active)
                return;

            Activate();
            Active = true;
        }

        /// <summary>
        /// Deactivates this menu, making it stop drawing on the screen.
        /// </summary>
        public void DeactivateMenu()
        {
            if (!Active)
                return;

            Deactivate();
            Active = false;
        }

        /// <summary>
        /// Saves the position(s) of this menu.
        /// </summary>
        [Obsolete]
        public virtual void SaveCoordinates(out TagCompound compound)
        {
            compound = new TagCompound()
            {
                [nameof(Left)] = GetTagCompoundSringFromDimensions(Left),
                [nameof(Top)] = GetTagCompoundSringFromDimensions(Top),
                [nameof(Width)] = GetTagCompoundSringFromDimensions(Width),
                [nameof(Height)] = GetTagCompoundSringFromDimensions(Height),
            };
        }

        /// <summary>
        /// Loads the position(s) of this menu.
        /// </summary>
        /// <param name="compound"></param>
        [Obsolete]
        public virtual void LoadCoordinates(TagCompound compound)
        {
            pr_LoadedLeft = GetDimensionsFromTagCompound(compound, nameof(Left));
            pr_LoadedTop = GetDimensionsFromTagCompound(compound, nameof(Top));
            pr_LoadedWidth = GetDimensionsFromTagCompound(compound, nameof(Width));
            pr_LoadedHeight = GetDimensionsFromTagCompound(compound, nameof(Height));

            pr_LoadedDimensions = true;
        }

        /// <summary>
        /// Constructs this menu, Adding it to the <see cref="AllMenus"/> list.
        /// </summary>
        public Menu() : base()
        {
            AllMenus.Add(this);
            ResetDimensions();
        }

        [Obsolete]
        protected string GetTagCompoundSringFromDimensions(StyleDimension dimension) => $"{GetType().Name}:{dimension.Pixels}:{dimension.Precent}";

        [Obsolete]
        protected StyleDimension GetDimensionsFromTagCompound(TagCompound compound, string name)
        {
            string[] args = compound.GetString(name).Split(':');
            return new StyleDimension(Convert.ToSingle(args[1]), Convert.ToSingle(args[2]));
        }

        [Obsolete]
        protected bool pr_LoadedDimensions;
        [Obsolete]
        protected StyleDimension pr_LoadedLeft;
        [Obsolete]
        protected StyleDimension pr_LoadedTop;
        [Obsolete]
        protected StyleDimension pr_LoadedWidth;
        [Obsolete]
        protected StyleDimension pr_LoadedHeight;
    }
}
