#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Raytracer
{
    /// <summary>
    /// This screen calls the actual raytracer logic to fill a texture
    /// </summary>
    class RaytracerScreen : BackgroundScreen
    {
        private RayTracerFramework.RayTracer.Raytracer raytracer;

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public RaytracerScreen(RayTracerFramework.RayTracer.Raytracer raytracer)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.raytracer = raytracer;
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                if (content == null) content = new ContentManager(ScreenManager.Game.Services);

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
               
                backgroundTexture = new Texture2D(ScreenManager.GraphicsDevice,
                                              viewport.Width,
                                              viewport.Height,
                                              1, ResourceUsage.None, SurfaceFormat.Color);

                Color[] data = new Color[viewport.Width * viewport.Height];
                raytracer.GenerateImage(ref data, viewport.Width, viewport.Height);
                backgroundTexture.SetData<Color>(data);
            }
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
                content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // nothing todo yet
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.PauseGame)
            {
                const string message = "Are you sure you want to exit the raytracer?";

                MessageBoxScreen messageBox = new MessageBoxScreen(message);

                messageBox.Accepted += ExitMessageBoxAccepted;

                ScreenManager.AddScreen(messageBox);
            }
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
