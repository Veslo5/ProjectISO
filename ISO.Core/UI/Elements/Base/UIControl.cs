using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.UI.Elements.Base
{
    public abstract class UIControl
    {
        public string Name { get; set; }


        /// <summary>
        /// Text color
        /// </summary>
        public Color Color { get; set; } = Color.White;


        private Point _position;

        /// <summary>
        /// Control position
        /// </summary>
        public Point Position
        {
            get { return _position; }
            set
            {
                if (Parent != null)
                {
                    _position = Parent.Position + value;
                }
                else
                {
                    _position = value;
                }

            }
        }

        /// <summary>
        /// Text position
        /// </summary>
        public Point Size { get; set; } = new Point(0, 0);

        /// <summary>
        /// Parent
        /// </summary>
        public UIControl Parent { get; set; }
    }
}
