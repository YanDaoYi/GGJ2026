using Assets.Scripts.MapData;
using Platformer.Core;
using Platformer.Model;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public partial class GameController
    {

        [Button(ButtonSizes.Large)]
        public void LoadLevel()
        {
            mapData.LoadLevel(this);
        }
    }
}