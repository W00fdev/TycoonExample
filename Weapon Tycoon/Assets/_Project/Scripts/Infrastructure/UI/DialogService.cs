using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.UI
{
    public class DialogService
    {
        private readonly Dictionary<Type, IDialog> _dialogs;

        public DialogService()
        {
            _dialogs = new()
            {

            };
        }

        public void Register<TDialog>(TDialog dialog)
            where TDialog : class, IDialog
            => _dialogs.TryAdd(typeof(TDialog), dialog);
        
        public UniTask<IDialog> ShowAsync<TDialog>()
            where TDialog : class, IDialog
            => _dialogs[typeof(TDialog)].ShowAsync();
        
        public UniTask<IDialog> HideAsync<TDialog>()
            where TDialog : class, IDialog
            => _dialogs[typeof(TDialog)].HideAsync();
        
        public IDialog ShowInstant<TDialog>()
            where TDialog : class, IDialog
            => _dialogs[typeof(TDialog)].ShowInstant();
        
        public IDialog HideInstant<TDialog>()
            where TDialog : class, IDialog
            => _dialogs[typeof(TDialog)].HideInstant();
    }
}