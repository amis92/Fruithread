using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Fruithread.ViewModels;

namespace Fruithread.Helpers
{
    public class FruitTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AppleTemplate { get; set; }
        public DataTemplate OrangeTemplate { get; set; }


        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var fruit = (FruitViewModel) item;
            switch (fruit.Kind)
            {
                case FruitKind.Orange:
                    return OrangeTemplate;
                case FruitKind.Apple:
                    return AppleTemplate;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), "Fruit kind not handled.");
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}