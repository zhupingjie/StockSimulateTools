using ServiceStack;
using ServiceStack.Text;
using StockSimulateCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockPriceTools
{
    public partial class VoluatieForm : Form
    {
        public VoluatieForm()
        {
            InitializeComponent();
        }

        private DataTable DataSource = null;

        private void btnValuatie_Click(object sender, EventArgs e)
        {
            var code = this.txtCode.Text.Trim();
            var data = GetMainTargets(code);
            if (data != null)
            {
                MockData(data);

                this.dataGridView1.DataSource = this.DataSource.DefaultView;
                for (var i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    if (i == 0)
                        this.dataGridView1.Columns[i].Width = 120;
                    else if(i>=1 && i<=9)
                        this.dataGridView1.Columns[i].Width = 60;
                    else
                        this.dataGridView1.Columns[i].Width = 80;
                }
            }
        }

        MainTargetEntity[] GetMainTargets(string code)
        {
            var josnParam = ServiceStack.Text.JsonSerializer.SerializeToString(new
            {
                type = 1,
                code = code
            });
            var retStr = "http://f10.eastmoney.com/NewFinanceAnalysis/MainTargetAjax".PostJsonToUrl(josnParam, requestFilter => {
                requestFilter.Timeout = 5 * 60 * 1000;
            });
            retStr = retStr.Replace("亿", "");
            var result = ServiceStack.Text.JsonSerializer.DeserializeFromString<MainTargetEntity[]>(retStr);
            return result;
        }

        void MockData(MainTargetEntity[] data)
        {
            var dates = data.GroupBy(c => c.date).Select(c=>c.Key).OrderByDescending(c => c).ToArray();
            this.DataSource = new DataTable("stockvoluation");
            this.DataSource.Columns.Add("指标");
            foreach(var date in dates)
            {
                this.DataSource.Columns.Add(date.ToString("yyyy"));
            }
            this.DataSource.Columns.Add("平均增长");
            this.DataSource.Columns.Add("去年增长");

            //基本每股收益
            BuildTargetRow("jbmgsy", dates, data);
            //每股净资产(元)
            BuildTargetRow("mgjzc", dates, data);
            //每股公积金(元)
            BuildTargetRow("mggjj", dates, data);
            //每股未分配利润(元)
            BuildTargetRow("mgwfply", dates, data);
            //每股经营现金流(元)
            BuildTargetRow("mgjyxjl", dates, data);
            //营业总收入(元)
            BuildTargetRow("yyzsr", dates, data);
            //归属净利润(元)
            BuildTargetRow("gsjlr", dates, data);
            //扣非净利润(元)
            BuildTargetRow("kfjlr", dates, data);
            //营业总收入同比增长(%)
            BuildTargetRow("yyzsrtbzz", dates, data);
            //归属净利润同比增长(%)
            BuildTargetRow("gsjlrtbzz", dates, data);
            //毛利率(%)
            BuildTargetRow("mll", dates, data);
            //资产负债率(%)
            BuildTargetRow("zcfzl", dates, data);
        }

        DataRow BuildTargetRow(string field, DateTime[] dates, MainTargetEntity[] data)
        { 
            //基本每股收益
            var dr = this.DataSource.NewRow();
            dr["指标"] = GetPropertyDesc(typeof(MainTargetEntity), field);//"基本每股收益(元)";

            var vals = new List<decimal>();
            var percent = new List<decimal>();
            foreach (var date in dates)
            {
                var name = date.ToString("yyyy");
                var item = data.FirstOrDefault(c => c.date == date);

                var value = GetPropertyValue<decimal>(item, field);
                dr[name] = value;
                if (vals.Count == 0)
                {
                    vals.Add(value);
                }
                else if (vals.Count == 1)
                {
                    var p = Math.Round((vals.FirstOrDefault() - value) / value, 2) * 100;
                    percent.Add(p);
                    vals.Clear();
                }
            }
            dr["平均增长"] = $"{percent.Sum() / percent.Count}%";
            dr["去年增长"] = $"{percent.FirstOrDefault()}%";
            this.DataSource.Rows.Add(dr);
            return dr;
        }

        string GetPropertyDesc(Type type, string field)
        {
            var prep = type.GetProperties().FirstOrDefault(c => c.Name == field);
            if (prep == null) return field;

            var attr = prep.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
            if (attr == null) return field;

            return (attr as DescriptionAttribute).Description;
        }

        T GetPropertyValue<T>(object obj, string field)
        {
            if (obj == null) return default(T);
            var propertyInfos = obj.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name.ToLower() == field.ToLower())
                {
                    var val = propertyInfo.GetValue(obj);
                    if (val == null) return default(T);
                    return TypeSerializer.DeserializeFromString<T>($"{val}");
                }
            }
            return default(T);
        }

        private void VoluatieForm_Load(object sender, EventArgs e)
        {
        }

    }
}
