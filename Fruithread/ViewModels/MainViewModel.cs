using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Fruithread.Commands;

namespace Fruithread.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int RemoveRandomMax = 10;
        private const int RemoveRandomChance = 1;

        private const int MaxFruits = 10;
        private const int TaskDelaySeconds = 1;
        private bool _isTaskRunning;

        private RelayCommand _startCommand;
        private RelayCommand _stopCommand;

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                foreach (var fruit in FruitViewModel.CreateForDesignTime())
                {
                    Fruits.Add(fruit);
                }
            }
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsTaskRunning))
                {
                    StartCommand.RaiseCanExecuteChanged();
                }
            };
        }

        public ObservableCollection<FruitViewModel> Fruits { get; } = new ObservableCollection<FruitViewModel>();

        public RelayCommand StartCommand
            => _startCommand ?? (_startCommand = new RelayCommand(async () => await RunTaskAsync(), CanStart));

        public RelayCommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand(Stop));

        private CancellationTokenSource CancellationTokenSource { get; set; }

        private bool IsTaskRunning
        {
            get { return _isTaskRunning; }
            set { Set(ref _isTaskRunning, value); }
        }

        private Random Random { get; } = new Random();

        private void Stop()
        {
            if (IsTaskRunning)
            {
                CancellationTokenSource.Cancel();
                return;
            }
            Fruits.Clear();
        }

        private async Task RunTaskAsync()
        {
            if (IsTaskRunning)
            {
                return;
            }
            using (CancellationTokenSource = new CancellationTokenSource())
            {
                IsTaskRunning = true;
                var cancellationToken = CancellationTokenSource.Token;
                cancellationToken.Register(() => IsTaskRunning = false);
                try
                {
                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }
                            await ChangeFruitsAsync();
                            await Task.Delay(TimeSpan.FromSeconds(TaskDelaySeconds), cancellationToken);
                        }
                    }, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    // it's all good
                    IsTaskRunning = false;
                }
            }
        }

        private async Task ChangeFruitsAsync()
        {
            if (Fruits.Count < MaxFruits)
            {
                var newFruit = Random.Next(2) < 1 ? FruitViewModel.CreateOrange() : FruitViewModel.CreateApple();
                await InvokeOnUiAsync(() => Fruits.Add(newFruit));
                return;
            }
            if (Random.Next(RemoveRandomMax) < RemoveRandomChance)
            {
                await InvokeOnUiAsync(() => Fruits.RemoveAt(Random.Next(MaxFruits)));
            }
            else
            {
                await InvokeOnUiAsync(() => Fruits[Random.Next(MaxFruits)].Counter += 1);
            }
        }

        private bool CanStart() => !IsTaskRunning;
    }
}