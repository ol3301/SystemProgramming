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
    public static class DoAnimation
    {
        public static async void FromLeftToRight(FrameworkElement d,bool value,bool isfirst=false)
        {
            var sb = new Storyboard();

            Thickness thickness;

            //В какую сторону двигать?
            if (!value)
                thickness = new Thickness(0, 0, d.ActualWidth, 0);
            else
                thickness = new Thickness();

            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                To = thickness,
                DecelerationRatio = 0.9
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            sb.Children.Add(animation);

            if (isfirst)
                d.Visibility = Visibility.Hidden;

            sb.Begin(d);

            await Task.Delay(200);

            if (isfirst)
                d.Visibility = Visibility.Visible;
        }
    }
}
