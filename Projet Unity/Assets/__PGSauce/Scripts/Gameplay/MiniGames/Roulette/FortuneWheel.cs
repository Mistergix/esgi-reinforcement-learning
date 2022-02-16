using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using PGSauce.Unity;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace PGSauce.Gameplay.MiniGames.Roulette
{
    public class FortuneWheel : MiniGame<RouletteReward>
    {
        #region Public And Serialized Fields

        [SerializeField, FoldoutGroup(SelectorGroup)]
        private RectTransform selector;
        [SerializeField, FoldoutGroup(SelectorGroup)]
        private SelectorDirection selectorDirection;
        [SerializeField, FoldoutGroup(SelectorGroup)]
        private bool anchorSelectorToCenter;
        [SerializeField, FoldoutGroup(SelectorGroup)]
        private float selectorElasticDuration = 1f;
        [SerializeField, FoldoutGroup(SelectorGroup)]
        private float selectorElasticAmplitude;
        [SerializeField, FoldoutGroup(SelectorGroup)]
        private float selectorElasticPeriod;

        [SerializeField, FoldoutGroup(WheelGroup)]
        private Transform wheel;
        [SerializeField, FoldoutGroup(WheelGroup), Tooltip("Empty space between elements")]
        private float elementSpread;
        [SerializeField, FoldoutGroup(WheelGroup), Tooltip("Space between element and center")]
        private float elementOffset;
        [SerializeField, FoldoutGroup(WheelGroup)]
        private bool equalDistribution;
        
        [SerializeField, FoldoutGroup(KnotsGroup)]
        private Transform knotsParent;
        [SerializeField, FoldoutGroup(KnotsGroup)]
        private float knotsOffset;
        [SerializeField, FoldoutGroup(KnotsGroup)]
        private AudioClip[] soundsKnotsCollision;
        [SerializeField, FoldoutGroup(KnotsGroup)]
        private MinMax<Float01> volumeRange = new MinMax<Float01>() {min = 0.2f, max = 1f};
        [SerializeField, FoldoutGroup(KnotsGroup)]
        private UnityEventAudioClip playKnotSound;
        
        [SerializeField, FoldoutGroup(SimulationGroup)]
        private bool centerOnElement;
        [SerializeField, FoldoutGroup(SimulationGroup), Tooltip("If true, rotates selector instead of wheel (if selector is inside)")]
        private bool rotateSelector;
        [SerializeField, FoldoutGroup(SimulationGroup), Tooltip("How many full laps it does every time it plays")]
        private MinMax<float> rotationCyclesRange = new MinMax<float>() {min = 2, max = 3};
        [SerializeField, FoldoutGroup(SimulationGroup), Tooltip("How long it stays rotating")]
        private MinMax<float> rotationTimeRange = new MinMax<float>() {min = 3, max = 4};
        [SerializeField, FoldoutGroup(SimulationGroup)]
        private float speedUpTime = 0.2f;
        [SerializeField, FoldoutGroup(SimulationGroup)]
        private float friction = 5;
        [SerializeField, FoldoutGroup(SimulationGroup)]
        private UnityEvent onStartSpinning;
        [SerializeField, FoldoutGroup(SimulationGroup)]
        private UnityEvent onFinishSpinning;
        [SerializeField, FoldoutGroup(SimulationGroup)]
        private float timeCollectReward;
        [SerializeField, FoldoutGroup(SimulationGroup)]
        private float timeNewRound;

        #endregion
        #region Private Fields
        private const string SelectorGroup = "Selector";
        private const string WheelGroup = "Wheel";
        private const string KnotsGroup = WheelGroup + "/Knots";
        private const string SimulationGroup = "Simulation";

        private List<RectTransform> _knots;
        private bool _isPlaying;
        private float _selectorMaxAngle;
        private float _wheelMaxAngleForKnot;
        private float _selectorElasticStartTime;
        private float _selectorElasticStartAngle;
        private int _forceReward = -1;
        #endregion
        #region Properties
        public float OffsetAngle => 90f * (int) selectorDirection;

        public float ElementOffset => elementOffset;

        #endregion
        #region Unity Functions
        public void Start()
        {
        }
        
        public void Update()
        {
        }
        
        public void OnEnable()
        {
        }
        
        public void OnDisable()
        {
        }
        
        public void OnDestroy()
        {
        }
        
        #endregion
        #region Public Methods

        public void StartSpinning()
        {
            StartSpinning(-1);
        }

        private void StartSpinning(int forceReward)
        {
            _forceReward = forceReward;
            StartRound();
        }

        protected override void FilterRewards()
        {
            base.FilterRewards();
            Rewards.RemoveAll(r => !r.gameObject.activeInHierarchy);
        }

        protected override void ApplyLayout()
        {
            _forceReward = -1;
            wheel.localRotation = Quaternion.identity;

            var wheelStartAngle = InitRewards();

            InitSelector();
            ApplyLayoutKnots(wheelStartAngle);
            GetSelectorAngles();
        }

        #endregion
        #region Private Methods
        private void StartRound()
        {
            onStartSpinning.Invoke();
            var totalAngle = GetRandomElement();
            StartCoroutine(DoPlay(totalAngle));
            _forceReward = -1;
        }

        private IEnumerator DoPlay(float totalAngle)
        {
            yield return null;
        }

        private float GetRandomElement()
        {
            var forceElement = _forceReward;
            _forceReward = 0;
            var angle = 0f;
            var totalPriority = Rewards.Sum(r => r.Probability);
            var randomPos = forceElement >= 0 ? totalPriority + 1 : Random.value * totalPriority;
            if (rotateSelector)
            {
                angle = OffsetAngle + (anchorSelectorToCenter ? 180f : 0f);
            }
            else
            {
                angle = 0f;
            }

            var first = true;
            var totalVisualPriority = equalDistribution ? Rewards.Count : totalPriority;
            for (var i = 0; i < Rewards.Count; ++i)
            {
                var element = Rewards[i];
                var priority = equalDistribution ? 1 : element.Probability;
                var sliceSize = 360f * priority / totalVisualPriority;
                if (first)
                {
                    angle -= sliceSize / 2f;
                    first = false;
                }

                randomPos -= element.Probability;
                if (randomPos < 0 || (forceElement >= 0 && i == forceElement))
                {
                    angle += _wheelMaxAngleForKnot;
                    if (centerOnElement)
                    {
                        angle += 0.5f * sliceSize;
                    }
                    else
                    {
                        angle += Mathf.Lerp(0.2f, 0.8f, Random.value) *
                                 (sliceSize - elementSpread - _wheelMaxAngleForKnot);
                    }
                    _forceReward = i;
                    return angle;
                }
                else
                {
                    angle += sliceSize;
                }
            }

            return angle;
        }

        /// <summary>
        ///  Highest angle the selector reaches because of the knots
        /// </summary>
        private void GetSelectorAngles()
        {
            var wheelToSelector = selector ? selector.localPosition.magnitude : 0;
            if (_knots.Count <= 0 || anchorSelectorToCenter || wheelToSelector <= 0)
            {
                _selectorMaxAngle = 0;
                _wheelMaxAngleForKnot = 0;
                return;
            }

            var selectorPivot = Vector3.up * wheelToSelector;
            var knot = _knots[0];
            var vKnot = knot.localPosition;
            var wheelToKnotSq = vKnot.sqrMagnitude;
            var wheelToSelectorSq = wheelToSelector * wheelToSelector;
            var selectorLength = selector.rect.height * selector.pivot.y;
            var selectorLengthSq = selectorLength * selectorLength;
            var y = (wheelToKnotSq + wheelToSelectorSq - selectorLengthSq) / (2 * selectorPivot.y);
            var x = Mathf.Sqrt(Mathf.Abs(wheelToKnotSq - y * y));
            var farthestPoint = new Vector3(x, y, selectorPivot.z);

            _selectorMaxAngle = Vector3.Angle(-selectorPivot, farthestPoint - selectorPivot);

            var rect = knot.rect;
            var knotHalfWidth = rect.width / 2f;
            var knotHalfHeight = rect.height / 2f;
            var knotRadius = Mathf.Sqrt(knotHalfWidth * knotHalfWidth + knotHalfHeight * knotHalfHeight);
            var selectorWidth = selector.rect.width * (1 - selector.pivot.x);
            _selectorMaxAngle += Mathf.Atan((selectorWidth + knotRadius) / selectorLength) * Mathf.Rad2Deg;
            _wheelMaxAngleForKnot = Mathf.Atan((farthestPoint.x + selectorWidth + knotRadius) / vKnot.magnitude) *
                                    Mathf.Rad2Deg;
        }
        private void ApplyLayoutKnots(float wheelStartAngle)
        {
            _knots.Clear();
            if (!knotsParent)
            {
                return;
            }

            foreach (RectTransform knot in knotsParent)
            {
                if (knot.gameObject.activeInHierarchy)
                {
                    _knots.Add(knot);
                }
            }

            for (var i = 0; i < _knots.Count; i++)
            {
                var knot = _knots[i];
                var angle = wheelStartAngle + knotsOffset - i * 360f / _knots.Count;
                knot.localRotation = Quaternion.Euler(0,0,angle);
                knot.localPosition = Quaternion.Euler(0, 0, angle) * (Vector3.up * knot.localPosition.magnitude);
            }
        }
        private float InitRewards()
        {
            var rewardsCount = Rewards.Count;
            var totalPriority = equalDistribution ? rewardsCount : Rewards.Sum(r => r.Probability);
            var angle = 0f;
            var first = true;
            var wheelStartAngle = 0f;
            foreach (var reward in Rewards)
            {
                if (reward.Separator)
                {
                    reward.Separator.gameObject.SetActive(true);
                }

                var priority = equalDistribution ? 1 : reward.Probability;
                var sliceSize = 360f * priority / totalPriority;

                if (first)
                {
                    angle -= sliceSize / 2f;
                    wheelStartAngle = angle;
                    first = false;
                }

                reward.ApplyLayout(angle, sliceSize - elementSpread, this);
                angle += sliceSize;
            }

            return wheelStartAngle;
        }

        private void InitSelector()
        {
            if (selector)
            {
                var offsetAngle = OffsetAngle;
                if (anchorSelectorToCenter)
                {
                    selector.localRotation = Quaternion.Euler(0, 0, 180 - offsetAngle);
                    selector.localPosition = Vector3.zero;
                }
                else
                {
                    selector.localRotation = Quaternion.Euler(0, 0, -offsetAngle);
                    selector.localPosition = Quaternion.Euler(0, 0, -offsetAngle) *
                                             (Vector3.up * selector.localPosition.magnitude);
                }

                _selectorElasticStartTime = -1000;
                _selectorElasticStartAngle = selector.rotation.eulerAngles.z;
            }
        }
        #endregion

        private enum SelectorDirection
        {
            Top,
            Right,
            Bottom,
            Left
        }
    }
}
