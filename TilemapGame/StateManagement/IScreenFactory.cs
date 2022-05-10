/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace TilemapGame.StateManagement
{
    /// <summary>
    /// Defines an object that can create a screen when given its type.
    /// </summary>
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
