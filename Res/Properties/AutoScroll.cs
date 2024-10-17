using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace MFAWPF.Styles.Properties
{

    public static class AutoScroll
    {
        private static bool _autoScroll;

        public static bool GetAutoScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }
        
        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }

        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(AutoScroll), new PropertyMetadata(false, AutoScrollPropertyChanged));

        private static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                bool alwaysScrollToEnd = (e.NewValue != null) && (bool)e.NewValue;
                if (alwaysScrollToEnd)
                {
                    scrollViewer.ScrollToEnd();
                    scrollViewer.ScrollChanged += ScrollChanged;
                }
                else
                {
                    scrollViewer.ScrollChanged -= ScrollChanged;
                }
            }
            else if (d is ListBox listBox)
            {
                INotifyCollectionChanged view = listBox.Items;
                view.CollectionChanged += (_, arg) =>
                {
                    switch (arg.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            listBox.ScrollIntoView(listBox.Items[arg.NewStartingIndex]);
                            break;
                    }
                };
            }
            else
            {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to ScrollViewer instances.");
            }
        }

        private static void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is not ScrollViewer scroll)
            {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to ScrollViewer instances.");
            }

            if (e.ExtentHeightChange == 0)
            {
                _autoScroll = Math.Abs(scroll.VerticalOffset - scroll.ScrollableHeight) < 1e-6;
            }

            if (_autoScroll && e.ExtentHeightChange != 0)
            {
                scroll.ScrollToVerticalOffset(scroll.ExtentHeight);
            }
        }
    }
}
