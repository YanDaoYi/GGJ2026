using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    //[CreateAssetMenu(menuName = "Audio/AudioConfigs")]
    public class AudioConfigs : ScriptableObject
    {
        public InnerAndOuterAudioConfigs InnerAudioConfigs;
        public InnerAndOuterAudioConfigs OuterAudioConfigs;
        [LabelText("穿梭")]
        public AudioClip shuttleAudio;
        [LabelText("复活")]
        public AudioClip reviveAudio;
        [LabelText("门 成功")]
        public AudioClip doorSuccessAudio;
        [LabelText("门 失败")]
        public AudioClip doorFailAudio;
        [LabelText("拾取")]
        public AudioClip pickItemAudio;
    }

    [Serializable]
    public class InnerAndOuterAudioConfigs
    {
        [LabelText("行走")]
        public List<AudioClip> walkAudioList;
        [LabelText("落地")]
        public AudioClip landingAudio;
        [LabelText("死亡")]
        public AudioClip deathAudio;
        [LabelText("跳跃")]
        public AudioClip jumpAudio;
        [LabelText("背景")]
        public AudioClip bgAudio;
        [LabelText("遮罩打开")]
        public AudioClip maskOpenAudio;
        [LabelText("遮罩持续")]
        public AudioClip maskContinueAudio;
        [LabelText("遮罩关闭")]
        public AudioClip maskCloseAudio;
    }
}
