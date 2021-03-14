using ISO.Core.UI.Elements;
using ISO.Core.UI.Elements.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO.Core.UI.Helpers
{
    internal class UIHelper
    {
        public UIHelper(List<IUI> uIHolder)
        {
            uiControlHolder = uIHolder.Cast<UIControl>().ToList();
        }

        //TODO: clear controlsOnPositon before returning GetUIOnPosition()
        private List<UIControl> controlsOnPositon = new List<UIControl>();
        private List<UIControl> uiControlHolder { get; set; }
        private UIControl uiUnderPosition { get; set; }
        private ISOPanel panelUnderPosition { get; set; }


        /// <summary>
        /// Get UI on positon
        /// </summary>
        public UIControl GetUIOnPosition(Point position)
        {
            panelUnderPosition = null;
            uiUnderPosition = null;
            return getUIOnPositionRecursive(position, uiControlHolder);
        }



        /// <summary>
        /// Recursively loops through UIControls and returns the one which meets this requirements:
        /// Is under position passed by parameter
        /// Has the highest Z index
        /// Is most nested
        /// Have no child that are under position passed by parameter
        /// </summary>
        /// <returns>Returns UIControl on position or null</returns>
        UIControl getUIOnPositionRecursive(Point position, List<UIControl> Childs)
        {
            getControlsOnPositon(Childs, position);

            if (controlsOnPositon.Count > 0)
            {
                //Select UIControl with highest Z index
                uiUnderPosition = controlsOnPositon.OrderByDescending(x => x.ZIndex).FirstOrDefault();

                if (uiUnderPosition is ISOPanel)
                {
                    panelUnderPosition = uiUnderPosition as ISOPanel;
                    //Are there any childs?
                    if (panelUnderPosition.Childs.Count > 0)//If so, call itself recursively
                        return getUIOnPositionRecursive(position, panelUnderPosition.Childs);
                }

                //Returns last found UIControl that can't have childs
                return uiUnderPosition;
            }

            //Returns last found UIControl or null
            return uiUnderPosition;
        }


        /// <summary>
        /// Loops through UIControls and collect those which are under position to <see cref="controlsOnPositon"/>
        /// </summary>
        void getControlsOnPositon(List<UIControl> Childs, Point position)
        {
            controlsOnPositon.Clear();

            foreach (UIControl control in Childs)
                if (control.DimensionsRectangle.Contains(position))
                    controlsOnPositon.Add(control);
        }
    }
}
