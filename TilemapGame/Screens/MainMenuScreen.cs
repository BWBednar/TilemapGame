/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using Microsoft.Xna.Framework;
using TilemapGame.StateManagement;

namespace TilemapGame.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        /// <summary>
        /// Constructor for the main menu screen
        /// </summary>
        public MainMenuScreen() : base("Winter Wonderland")
        {
            var playGameMenuEntry = new MenuEntry("Play Game");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        /// <summary>
        /// Event for if the user selects to play the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GamePlayScreen());
        }

        /// <summary>
        /// Event for if the user exits the game, to be implemented later
        /// </summary>
        /// <param name="playerIndex"></param>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Would confirm that the user selection and exit the game, not implemented yet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
