using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using WarWolfWorks_Mod.Interfaces;
using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.UI
{
    /// <summary>
    /// Core class for all UI of the WarWolfWorks mod.
    /// </summary>
    public abstract class Menu : UIState, IPostWorldLoadable
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
        public static void ActivateMenu<T>(WWWPlayer perpetrator) where T : Menu
        {
            T toUse = GetMenu<T>();
            toUse.ActivateMenu(perpetrator);
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
        /// Modplayer which activated this menu.
        /// </summary>
        public WWWPlayer Perpetrator { get; private set; }

        /// <summary>
        /// Returns true if this menu is currently dragged with the mouse.
        /// </summary>
        protected bool Dragged { get; private set; }

        private Vector2 Offset;

        /// <summary>
        /// The width of this menu.
        /// </summary>
        protected virtual StyleDimension DimensionWidth => StyleDimension.Fill;
        /// <summary>
        /// The height of this menu.
        /// </summary>
        protected virtual StyleDimension DimensionHeight => StyleDimension.Fill;

        private void SetPerpetrator(WWWPlayer perpetrator)
        {
            Perpetrator = perpetrator;
        }

        private void RemovePerpetrator() => Perpetrator = null;

        /// <summary>
        /// Called right after the world is loaded through <see cref="WarWolfWorks_Mod.Internal.WWWPlayer"/>.
        /// </summary>
        public virtual void OnWorldLoaded(WWWPlayer from)
        {
            if (!Main.dedServ)
            {
                MenuInterface = new UserInterface();
                Activate();
                ActivateMenu(from);
            }
        }

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
        }

        /// <summary>
        /// Handles position change of the menu based on mousedrag.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                Vector2 mousePos = Vector2.Zero;//Utilities.MousePos;
                Perpetrator.player.mouseInterface = ContainsPoint(mousePos);
                if (Dragged)
                {
                    Left.Set(mousePos.X - Offset.X, 0f);
                    Top.Set(mousePos.Y - Offset.Y, 0f);

                    Recalculate();
                    RecalculateChildren();
                }
            }
        }

        /// <summary>
        /// Activates this menu, making it draw on the screen.
        /// </summary>
        public void ActivateMenu(WWWPlayer perpetrator)
        {
            if (Active)
                return;

            SetPerpetrator(perpetrator);
            MenuInterface.SetState(this);
            Active = true;
        }

        /// <summary>
        /// Deactivates this menu, making it stop drawing on the screen.
        /// </summary>
        public void DeactivateMenu()
        {
            if (!Active)
                return;

            RemovePerpetrator();
            MenuInterface.SetState(null);
            Active = false;
        }

        /// <summary>
        /// <see cref="UserInterface"/> of this menu.
        /// </summary>
        public UserInterface MenuInterface { get; private set; }

        /// <summary>
        /// Constructs this menu, Adding it to the <see cref="AllMenus"/> list.
        /// </summary>
        public Menu() : base()
        {
            AllMenus.Add(this);
            WWWPlayer.PostWorldLoadables.Add(this);

            Width = DimensionWidth;
            Height = DimensionHeight;
        }

        /// <summary>
        /// Deconstructs this menu, removing it from <see cref="AllMenus"/> list.
        /// </summary>
        ~Menu()
        {
            AllMenus.Remove(this);
        }
    }
}
