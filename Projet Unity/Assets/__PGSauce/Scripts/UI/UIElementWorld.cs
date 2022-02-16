using System;
using DG.Tweening;
using MonKey.Extensions;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using PGSauce.Games.DropDaBomb;
using TMPro;
using UnityEngine;

namespace PGSauce.UI
{
    public class UIElementWorld : MonoBehaviour, UIElementBase
    {
        [SerializeField] private UIElementData elementData;
        [SerializeField, Tooltip("Should this menu deactivate while its not visible on the screen.")]
        public bool deactivateWhileInvisible = true;
        [SerializeField] private bool visible;

        [SerializeField] private TMP_Text text;
        
        private Vector3AnimationSection MovementSection => elementData.MovementSection;
        private Vector3AnimationSection RotationSection => elementData.RotationSection;
        private Vector3AnimationSection ScaleSection => elementData.ScaleSection;

        private FloatAnimationSection OpacitySection => elementData.OpacitySection;
        
        private float _showingTime;
        private float _hidingTime;
        private Tween _hideTweenDelay;

        private Vector3 _initialPosition;


        public float GetAllHidingTime()
        {
            return GetAllHidingTime(elementData);
        }

        public float GetAllShowingTime()
        {
            return GetAllShowingTime(elementData);
        }
        
        public void SetUIData(UIElementData numberUIElementData)
        {
            elementData = numberUIElementData.Clone();
            InitUIData();
        }

        public float GetAllHidingTime(UIElementData numberUIElementData)
        {
            var hidingTime = 0.0f;
            hidingTime = numberUIElementData.CheckGreatestHidingTime(hidingTime);
            
            return hidingTime;
        }

        public float GetAllShowingTime(UIElementData numberUIElementData)
        {
            var showingTime = 0.0f;
            showingTime = numberUIElementData.CheckGreatestShowingTime(showingTime);
            
            return showingTime;
        }
        
        private void InitUIData()
        {
            elementData.Init();

            _hidingTime = GetAllHidingTime();
            _showingTime = GetAllShowingTime();
        }

        private void OnDestroy()
        {
            elementData.OnUiDestroyed();
            _hideTweenDelay?.Kill();
            _hideTweenDelay = null;
        }


        public void ChangeVisibility(bool v, bool noSounds = false)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            visible = v;

            

            if (MovementSection.UseSection)
            {
                _initialPosition = transform.position;

                ModifyMovementSection(visible, _initialPosition, MovementSection);
                MovementSection.Control(gameObject, ve =>
                {
                    transform.position = ve + _initialPosition;
                }, visible, false);
            }

            if (RotationSection.UseSection)
            {
                RotationSection.Control(gameObject, vector3 => transform.localEulerAngles = vector3,  visible, false);
            }
            
            if (ScaleSection.UseSection)
            {
                ScaleSection.Control(gameObject, vector3 => transform.localScale = vector3,  visible, false);
            }

            if (OpacitySection.UseSection)
            {
                //TODO change this to polymorphism, so that we don't have to use TMP directly
                OpacitySection.Control(gameObject, alpha => text.alpha = alpha, visible, false);
            }
            
            if (deactivateWhileInvisible)
            {
                if (visible)
                {
                    _hideTweenDelay?.Kill();
                }
                else
                {
                    _hideTweenDelay = DOVirtual.DelayedCall(_hidingTime, () =>
                    {
                        gameObject.SetActive(false);
                        _hideTweenDelay = null;
                    });
                }
            }
        }

        private static void ModifyMovementSection(bool vi, Vector3 initialPos, AnimationSection<Vector3> section)
        {
            
        }
    }
}