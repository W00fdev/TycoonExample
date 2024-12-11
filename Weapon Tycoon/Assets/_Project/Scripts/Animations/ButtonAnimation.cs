using System;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Animations
{
    public class ButtonAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _pushButton;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _easeType;

        [SerializeField] private Vector3 _pushedLocalPosition;
    
        private Vector3 _startLocalPosition;
        private Action _onRelease;

        private void Awake()
        {
            _startLocalPosition = _pushButton.localPosition;
        }

        public void PushButton()
        {
            Tween.LocalPosition(_pushButton, _pushedLocalPosition, _duration, _easeType);
        }

        public void ReleaseButton()
        {
            var tween = Tween.LocalPosition(_pushButton, _startLocalPosition, _duration, _easeType);
            if (_onRelease != null)
                tween.OnComplete(_onRelease);
        }

        [Button("Set current local position")]
        private void SetCurrentLocalPosition()
        {
            if (_pushButton)
                _pushedLocalPosition = _pushButton.localPosition;
        }

        public void SetOnReleaseCallback(Action action)
        {
            _onRelease = action;
        }
    }
}
