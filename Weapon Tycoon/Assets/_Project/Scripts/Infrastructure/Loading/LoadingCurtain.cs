using _Project.Scripts.Infrastructure.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace _Project.Scripts.Infrastructure.Loading
{
    public class LoadingCurtain : MonoBehaviour, IDialog
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] protected float _popupAnimationDuration;

        public async UniTask<IDialog> ShowAsync()
        {
            await _canvasGroup
                .DOFade(1f, _popupAnimationDuration)
                .From(0f)
                .SetEase(Ease.OutQuad)
                .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            
            return this;
        }

        public async UniTask<IDialog> HideAsync()
        {
            await _canvasGroup
                .DOFade(0f, _popupAnimationDuration)
                .From(1f)
                .SetEase(Ease.InQuad)
                .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            
            return this;
        }

        public IDialog ShowInstant()
        {
            _canvasGroup.alpha = 1f;

            return this;
        }

        public IDialog HideInstant()
        {
            _canvasGroup.alpha = 0f;
            
            return this;
        }
    }
}