using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.UI
{
    public interface IDialog
    {
        UniTask<IDialog> ShowAsync();
        UniTask<IDialog> HideAsync();

        IDialog ShowInstant();
        IDialog HideInstant();
    }
}