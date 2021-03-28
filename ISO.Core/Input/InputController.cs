using ISO.Core.Input.VirtualButtons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Input
{
    public class InputController
    {
        public Point MousePosition { get; set; }

        public VirtualButtonConfig VirtualButtonConfig { get; set; }

        private KeyboardState currentKeyboardState { get; set; }
        private KeyboardState lastKeyboardState { get; set; }

        private MouseState currentMouseState { get;set;}
        private MouseState lastMouseState { get;set;}

        public InputController()
        {
            VirtualButtonConfig = new VirtualButtonConfig();
        }

        internal void BeforeUpdate()
        {
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();

            MousePosition = currentMouseState.Position;            
        }

        internal void AfterUpdate()
        {
            lastKeyboardState = currentKeyboardState;
            lastMouseState = currentMouseState;
        }

        /// <summary>
        /// Get whether current key is currently pressed
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool IsVirtualButtonDown(string actionName)
        {
            var pressedKeys = currentKeyboardState.GetPressedKeys();
            
            if (pressedKeys.Length == 0)
                return false;

            if (VirtualButtonConfig.virtualButtonSetHolder.ContainsKey(actionName)) // If virtual button action exists
            {
                for (int holderIndex = 0; holderIndex < VirtualButtonConfig.virtualButtonSetHolder[actionName].Count; holderIndex++)
                {
                    for (int keyIndex = 0; keyIndex < pressedKeys.Length; keyIndex++)
                    {
                        if (VirtualButtonConfig.virtualButtonSetHolder[actionName][holderIndex].Key == pressedKeys[keyIndex])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        private bool IsVirtualButtonDown(string actionName, Keys[] pressedKeys)
        {
            if (pressedKeys.Length == 0)
                return false;

            if (VirtualButtonConfig.virtualButtonSetHolder.ContainsKey(actionName)) // If virtual button action exists
            {
                for (int holderIndex = 0; holderIndex < VirtualButtonConfig.virtualButtonSetHolder[actionName].Count; holderIndex++)
                {
                    for (int keyIndex = 0; keyIndex < pressedKeys.Length; keyIndex++)
                    {
                        if (VirtualButtonConfig.virtualButtonSetHolder[actionName][holderIndex].Key == pressedKeys[keyIndex])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get whether current key is currently not pressed
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool IsVirtualButtonUp(string actionName)
        {
            return !IsVirtualButtonDown(actionName); // :D 
        }
        private bool IsVirtualButtonUp(string actionName, Keys[] pressedKeys)
        {
            return !IsVirtualButtonDown(actionName, pressedKeys); // :D 
        }

        /// <summary>
        /// Is button pressed once
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool IsVirtualButtonPressed(string actionName)
        {
            if (IsVirtualButtonUp(actionName, lastKeyboardState.GetPressedKeys()) && IsVirtualButtonDown(actionName, currentKeyboardState.GetPressedKeys()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Left mouse button click
        /// </summary>
        /// <returns></returns>
        public bool IsLeftMouseButtonPressed() => IsMouseStatePressed(lastMouseState.LeftButton, currentMouseState.LeftButton);

        /// <summary>
        /// Right mouse button click
        /// </summary>
        /// <returns></returns>
        public bool IsRightMouseButtonPressed() => IsMouseStatePressed(lastMouseState.RightButton, currentMouseState.RightButton);


        private bool IsMouseStatePressed(ButtonState lastState, ButtonState currentState)
        {
            if (lastState == ButtonState.Released && currentState == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}