using Platformer.Mechanics;
using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class MechanismMono_Token : MechanismMono_Base
    {
        public override EMechanismType MechanismType => EMechanismType.Token;
        bool isPickedUp = false;

        protected override void TriggerEnterHandle(Collider2D other)
        {
            if (!isPickedUp)
            {
                isPickedUp = true;
                GameController.Instance.PickedUpTokenCount++;

                viewTf.gameObject.SetActive(false);
                logicTf.gameObject.SetActive(false);
            }
        }
    }
}
