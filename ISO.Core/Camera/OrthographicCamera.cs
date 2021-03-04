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

        public bool UseVirtualResolution { get; set; }
        public bool updateMetrix { get; set; } = true;


        private Vector2 _position = new Vector2(0, 0);

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                updateMetrix = true;
            }
        }


        /// <summary>
        /// Camera projection
        /// </summary>
        public Matrix Projection { get; set; }

        /// <summary>
        /// Camera zoom
        /// </summary>
        private float zoom = 1;

        public float Zooom
        {
            get { return zoom; }
            set
            {
                zoom = value;

                if (zoom < 0.5f)
                {
                    zoom = 0.5f;
                }

                if (zoom > 1.5f)
                {
                    zoom = 1.5f;
                }

                updateMetrix = true;
            }
        }

        /// <summary>
        /// Rotation of camera view
        /// </summary>
        private float rotation = 0;

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                updateMetrix = true;
            }
        }

        public Vector2 VirtualResolution { get; set; } = new Vector2(1920, 1080);


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">viewport of camera</param>
        public OrthographicCamera(Viewport view, bool useVirtualResolution = false)
        {
            viewport = view;
            UseVirtualResolution = useVirtualResolution;
        }

        /// <summary>
        /// Occurs when resolution is changed
        /// </summary>
        /// <param name="view"></param>
        public void OnResolutionChange(Viewport view)
        {
            viewport = view;            
            updateMetrix = true;
        }

        /// <summary>
        /// Recalculation camera view
        /// </summary>
        /// <param name="position"></param>
        public void Update()
        {
            if (updateMetrix == true)
            {

                float widthRatio = 1;
                float heighRatio = 1;

                if (UseVirtualResolution == true)
                {
                    widthRatio = viewport.Width / VirtualResolution.X;
                    heighRatio = viewport.Height / VirtualResolution.Y;
                }


                center = new Vector2(Position.X, Position.Y);

                Projection =
                    Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(widthRatio, heighRatio, 1) *
                    Matrix.CreateScale(new Vector3(Zooom, Zooom, 1)) *
                //Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
                Matrix.CreateTranslation(new Vector3(0, 0, 0));

                updateMetrix = false;
            }
        }

        public void ReCenter()
        {
            Position -= new Vector2(viewport.X / 2, viewport.Y / 2);
            updateMetrix = true;
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
            updateMetrix = true;
        }

    }
}
