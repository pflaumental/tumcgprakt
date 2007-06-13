#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
#endregion

namespace Raytracer
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        RayTracerFramework.RayTracer.Raytracer rayTracer;

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen(RayTracerFramework.RayTracer.Raytracer raytracer)
        {
            this.rayTracer = raytracer;
            MenuEntries.Add("Trace Scene");            

            // tum.3D: you may want do add additional menu items here, e.g. settings for your raytracer

            MenuEntries.Add("Exit");
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user menu selections.
        /// </summary>
        protected override void OnSelectEntry(int entryIndex)
        {
            switch (entryIndex)
            {
                case 0:
                    // Play the game.
                    LoadingScreen.Load(ScreenManager, LoadTracerScreen, true);
                    break;

                // tum.3D: if you added items in the list above you need to add the logic here

                case 1:
                    // Exit the sample.
                    OnCancel();
                    break;
            }
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit the raytracer?";

            MessageBoxScreen messageBox = new MessageBoxScreen(message);

            messageBox.Accepted += ExitMessageBoxAccepted;

            ScreenManager.AddScreen(messageBox);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        /// <summary>
        /// Loading screen callback for activating the raytracer.
        /// </summary>
        void LoadTracerScreen(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new RaytracerScreen(rayTracer));
        }


        #endregion
    }
}
