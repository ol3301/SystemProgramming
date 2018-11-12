using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initiator
{
    class VM : BindableBase
    {
        private BindProject bindProject;

        private DataTable _table;
        private DataSet set;
        private SqlDataAdapter adapter;
        private SqlConnection conn;


        private DelegateCommand _loadData;
        private DelegateCommand _paintData;

        public bool isPaintEnable { get; set; }

        
        public DataTable Table
        {
            get => _table;
            set
            {
                _table = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand PaintData
        {
            get => _paintData ?? (_paintData = new DelegateCommand(()=>
            {
                double[] weathers = new double[Table.Rows.Count];
                string[] days = new string[Table.Rows.Count];

                for(int i = 0; i < Table.Rows.Count; ++i) {
                    days[i] = Table.Rows[i][0].ToString();
                    weathers[i] = double.Parse(Table.Rows[i][1].ToString());
                }

                bindProject.SendToPaint(weathers,days);
            }));
        }
        public DelegateCommand LoadData
        {
            get => _loadData ?? (_loadData = new DelegateCommand(()=>
            {
                set = new DataSet();
                conn = new SqlConnection(@"Data source=(localdb)\MSSQLLocalDB;Initial catalog=ForecastWeather;Integrated security=SSPI;");
                adapter = new SqlDataAdapter("SELECT Day,Weather FROM Weather",conn);

                adapter.Fill(set);

                Table = set.Tables[0];
            }));
        }

        public VM(bool isshow=false,BindProject bindProject=null)
        {
            Table = new DataTable();
            isPaintEnable = isshow;
            this.bindProject = bindProject;
        }
    }
}
