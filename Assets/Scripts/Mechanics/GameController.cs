using Assets.Scripts.Mechanics;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public partial class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        [SerializeField]
        SwitchCtrl switchCtrl;
        public SwitchCtrl SwitchCtrl => switchCtrl;
        [SerializeField]
        Transform masksRoot;
        public Transform MasksRoot => masksRoot;
        [SerializeField]
        Transform innerItemsRoot;
        public Transform InnerItemsRoot => innerItemsRoot;
        [SerializeField]
        Transform outerItemsRoot;
        public Transform OuterItemsRoot => outerItemsRoot;


        #region Prefabs
        [SerializeField]
        GameObject maskPrefab;
        public GameObject MaskPrefab => maskPrefab;
        [SerializeField]
        GameObject itemPrefab_token;
        public GameObject ItemPrefab_Token => itemPrefab_token;
        [SerializeField]
        GameObject itemPrefab_destination;
        public GameObject ItemPrefab_Destination => itemPrefab_destination;
        #endregion

        public event System.Action OnLevelPassed = delegate { };
        public event System.Action OnLevelFailed = delegate { };
        public int TargetTokenCount { get; set; } = 0;
        public int PickedUpTokenCount { get; set; } = 0;

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }

        //顺利通过本关
        public void PassThisLevel()
        {
            OnLevelPassed?.Invoke();
        }

        public void FailedThisLevel()
        {
            OnLevelFailed?.Invoke();
        }
    }
}