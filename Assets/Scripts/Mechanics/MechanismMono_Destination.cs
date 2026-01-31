using Platformer.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class MechanismMono_Destination : MechanismMono_Base
    {
        public override EMechanismType MechanismType => EMechanismType.Destination;

        protected override void TriggerEnterHandle(Collider2D other)
        {
            if (GameController.Instance.PickedUpTokenCount >= GameController.Instance.TargetTokenCount)
            {
                GameController.Instance.PassThisLevel();
            }
        }
    }
}
