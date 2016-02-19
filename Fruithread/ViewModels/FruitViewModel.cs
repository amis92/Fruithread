using System.Collections.Generic;

namespace Fruithread.ViewModels
{
    public class FruitViewModel : ViewModelBase
    {
        private int _counter;

        private FruitViewModel(FruitKind kind)
        {
            Kind = kind;
        }

        public int Counter
        {
            get { return _counter; }
            set { Set(ref _counter, value); }
        }

        public FruitKind Kind { get; }

        public static FruitViewModel CreateOrange()
        {
            return new FruitViewModel(FruitKind.Orange);
        }

        public static FruitViewModel CreateApple()
        {
            return new FruitViewModel(FruitKind.Apple);
        }

        public static IEnumerable<FruitViewModel> CreateForDesignTime()
        {
            return new[]
            {
                new FruitViewModel(FruitKind.Apple) {Counter = 3},
                new FruitViewModel(FruitKind.Orange) {Counter = 4},
                new FruitViewModel(FruitKind.Apple) {Counter = 1},
                new FruitViewModel(FruitKind.Apple) {Counter = 7},
                new FruitViewModel(FruitKind.Orange) {Counter = 5}
            };
        }
    }

    public enum FruitKind
    {
        Orange,
        Apple
    }
}