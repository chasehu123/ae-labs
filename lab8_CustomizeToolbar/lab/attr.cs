using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS;

namespace lab
{
    
    public partial class attr : Form
    {
        ILayer m_layer = null;
        public attr(ILayer layer)
        {
            InitializeComponent();
            m_layer = layer;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void attr_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("加载属性");
            //提供对返回有关表和管理表的信息的成员的访问
            ITable lytable = (ITable)m_layer;
            DataTable table = new DataTable();
            //提供对返回有关字段的信息的成员的访问
            IField field;
            for (int i = 0; i < lytable.Fields.FieldCount; i++)
            {
                field = lytable.Fields.get_Field(i);
                table.Columns.Add(field.Name);
            }
            dataGridView1.DataSource = table;
            object[] values = new object[lytable.Fields.FieldCount];
            //提供对基于属性值、关系筛选数据的成员的访问
            IQueryFilter queryFilter = new QueryFilterClass();
            //提供对分发枚举行、字段集合的成员的访问，并允许更新、删除和插入行
            ICursor cursor = lytable.Search(queryFilter, true);
            IRow row;
            while ((row = cursor.NextRow()) != null) //将光标的位置前进一个，并返回位于该位置的 Row 对象
            {
                for (int j = 0; j < lytable.Fields.FieldCount; j++)
                {
                    object ob = row.get_Value(j);
                    values[j] = ob;
                }
                table.Rows.Add(values);
            }
            this.dataGridView1.DataSource = table;
        }
    }
}
