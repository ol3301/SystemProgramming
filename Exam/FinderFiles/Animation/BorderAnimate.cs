using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FinderFiles
{
    public static class BorderAnimate
    {
        public static readonly DependencyProperty IsStartProperty = DependencyProperty.RegisterAttached("IsStart",typeof(bool),
            typeof(BorderAnimate), new PropertyMetadata(false,OnChange,OnCoerce));

        private static Dictionary<WeakReference, bool> firstload = new Dictionary<WeakReference, bool>();

        private static object OnCoerce(DependencyObject d, object baseValue)
        {
            if (!(d is FrameworkElement border))
                return baseValue;

            var obj = firstload.FirstOrDefault(x=>x.Key.Target == d);
            if (obj.Key == null)
            {
                RoutedEventHandler rout = null;

                rout = (s, e) =>
                {
                    firstload[new WeakReference(d)]= true;
                    border.Loaded -= rout;
                    DoAnimation.FromLeftToRight(border, false);
                };

                border.Loaded += rout;
            }
            return baseValue;
        }

        private static void OnChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement border)
                DoAnimation.FromLeftToRight(border, (bool)e.NewValue);
        }

        public static bool GetIsStart(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsStartProperty);
        }

        public static void SetIsStart(DependencyObject obj,bool value)
        {
            obj.SetValue(IsStartProperty,value);
        }
    }
}
