using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Drawer
{
    public class VM : BindableBase
    {
        public SeriesCollection SeriesCollection { get; set; }
        //форматирует отображение значений по y, а также при наведении миши на точки
        public Func<double, string> YFormatter => val => 
        {
            return val.ToString()+ '°';
        };

        public ObservableCollection<string> Labels { get; set; }

        public VM()
        {
            Labels = new ObservableCollection<string>();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Movement weather",
                    Values = new ChartValues<double> ()
                }
            };
        }

        public void AddData(double[] weathers,string[] days)
        {
            Application.Current.Dispatcher.Invoke(new Action(()=>
            {
                foreach (var i in weathers)
                    SeriesCollection[0].Values.Add(i);

                foreach (var i in days)
                    Labels.Add(i);
            }));
        }
    }
}
