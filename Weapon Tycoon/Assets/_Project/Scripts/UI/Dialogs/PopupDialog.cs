using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.UI
{
    public class PopupDialog : MonoBehaviour, IDialog
    {
        [SerializeField] protected Transform _popupContainer;
        [SerializeField] protected float _popupAnimationDuration;
        
        private bool _isPlaying;
        
        protected readonly Vector3 _originalScale = Vector3.one;
        protected readonly Vector3 _hidedScale = Vector3.one * 0.01f;

        public async UniTask<IDialog> ShowAsync()
        {
            if (_isPlaying)
                return null;

            _isPlaying = true;
            gameObject.SetActive(true);
            
            await _popupContainer
                .DOScale(_originalScale, _popupAnimationDuration)
                .From(_hidedScale)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());

            _isPlaying = false;
            return this;
        }

        public async UniTask<IDialog> HideAsync()
        {
            if (_isPlaying)
                return null;
         
            _isPlaying = true;
            
            await _popupContainer
                .DOScale(_hidedScale, _popupAnimationDuration)
                .From(_originalScale)
                .SetEase(Ease.InBack)
                .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            
            _isPlaying = false;
            gameObject.SetActive(false);
            
            return this;
        }

        public IDialog ShowInstant()
        {
            _popupContainer.localScale = _originalScale;
            gameObject.SetActive(true);

            return this;
        }

        public IDialog HideInstant()
        {
            gameObject.SetActive(false);
            _popupContainer.localScale = _hidedScale;
            
            return this;
        }
    }
}