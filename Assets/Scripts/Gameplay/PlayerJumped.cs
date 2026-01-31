using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player performs a Jump.
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped>
    {
        public PlayerController player;

        public override void Execute()
        {
            var audioConfig = player.InOuter ? player.AudioConfigs.OuterAudioConfigs : player.AudioConfigs.InnerAudioConfigs;
            if (player.audioSource && audioConfig.jumpAudio)
                player.audioSource.PlayOneShot(audioConfig.jumpAudio);
        }
    }
}