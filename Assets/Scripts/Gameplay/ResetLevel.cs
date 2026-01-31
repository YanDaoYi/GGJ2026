using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class ResetLevel : Simulation.Event<ResetLevel>
    {

        public override void Execute()
        {
            GameController.Instance.FailedThisLevel();
        }
    }
}