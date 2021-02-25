using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Camera
{
    /// <summary>
    /// Camera
    /// </summary>
    public class OrthographicCamera
    {
        public Viewport viewport;
        private Vector2 center;

        private float zoom = 1;
        private float rotation = 0;

        public Vector2 Position { get; set; } = new Vector2(0,0);

        /// <summary>
        /// Camera projection
        /// </summary>
        public Matrix Projection { get; set; }
        
        /// <summary>
        /// Camera zoom
        /// </summary>
        public float Zooom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                {
                    zoom = 0.1f;
                }
            }
        }

        /// <summary>
        /// Rotation of camera view
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">viewport of camera</param>
        public OrthographicCamera(Viewport view)
        {
            viewport = view;
        }

        /// <summary>
        /// Occurs when resolution is changed
        /// </summary>
        /// <param name="view"></param>
        public void OnResolutionChange(Viewport view)
        {
            viewport = view;
        }

        /// <summary>
        /// Recalculation camera view
        /// </summary>
        /// <param name="position"></param>
        public void Update()
        {
            center = new Vector2(Position.X, Position.Y);

            Projection = 
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zooom,Zooom,1)) *
            //Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
            Matrix.CreateTranslation(new Vector3(0, 0, 0));
        }

        public Vector2 ScreenToWorldSpace(in Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(Projection);
            return Vector2.Transform(point, invertedMatrix);
        }


        public Vector2 WorldToScreenSpace(in Vector2 world)
        {
            return Vector2.Transform(world, Projection);
        }

        public void Reset()
        {
            zoom = 1;
            rotation = 0;
        }

    }
}
