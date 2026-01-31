using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using System.Threading.Tasks;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override async void Execute()
        {
            var player = model.player;
            player.collider2d.enabled = true;
            player.controlEnabled = false;
            if (player.audioSource && player.AudioConfigs.reviveAudio)
                player.audioSource.PlayOneShot(player.AudioConfigs.reviveAudio);
            player.health.Increment();
            player.Teleport(model.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.Grounded;
            player.animator.SetBool("dead", false);
            Simulation.Schedule<EnablePlayerInput>(2f);
            await Task.Delay(2000);
            GameController.Instance.FailedThisLevel();
        }
    }
}