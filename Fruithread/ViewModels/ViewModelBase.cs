using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;

namespace Fruithread.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public CoreDispatcher CoreDispatcher { get; set; }

        protected static bool IsInDesignMode => DesignMode.DesignModeEnabled;

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected async Task InvokeOnUiAsync(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (DesignMode.DesignModeEnabled || CoreDispatcher == null || CoreDispatcher.HasThreadAccess)
            {
                action();
                return;
            }
            await CoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        protected async Task InvokeOnUiAsync(Func<Task> asyncAction)
        {
            if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));
            if (DesignMode.DesignModeEnabled || CoreDispatcher == null || CoreDispatcher.HasThreadAccess)
            {
                await asyncAction();
                return;
            }
            await CoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await asyncAction());
        }
    }
}