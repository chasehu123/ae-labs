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
using ESRI.ArcGIS.Output;

namespace lab
{
    public partial class lab5 : Form
    {
        public lab5()
        {
            InitializeComponent();
        }

        private void lab5_Load(object sender, EventArgs e)
        {
            openMapDoc();
            //axMapControl1.AddShapeFile(@"..\Data", "area.shp");
        }


        public void MapToPage(AxMapControl axMapControl1, AxPageLayoutControl axPageLayoutControl1)
        {
            IObjectCopy pObjectCopy = new ObjectCopyClass();
            object pSourceMap = (object)axMapControl1.Map;
            object pCopiedMap = pObjectCopy.Copy(pSourceMap);
            object pOverwritedMap = (object)axPageLayoutControl1.ActiveView.FocusMap;
            pObjectCopy.Overwrite(pCopiedMap, ref pOverwritedMap);
            
        }



        private void openMapDoc()
        {
            OpenFileDialog openFileDialog;
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "打开地图文档 Open Mxd Document";
            openFileDialog.Filter = "地图文档 Map Document(*.mxd)|*.mxd";
            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;
            if (axMapControl1.CheckMxFile(filePath))
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrowHourglass; 
                axMapControl1.LoadMxFile(filePath, 0,Type.Missing); 
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            else
            {
                MessageBox.Show(filePath + "不是有效的地图文档");
            }
        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            MapToPage(axMapControl1, axPageLayoutControl1);
        }

        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView pPageLayoutView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;


            //QI 方式通过焦点地图获得对象
            pPageLayoutView.ScreenDisplay.DisplayTransformation.VisibleBounds = axMapControl1.Extent;//控制显示范围
            axPageLayoutControl1.ActiveView.Refresh();
            MapToPage(axMapControl1, axPageLayoutControl1);
        }




        //private void axMapControl1_OnMapReplaced(object sender,IMapControlEvents2_OnMapReplacedEvent e)
        //{
        //    MapToPage(axMapControl1, axPageLayoutControl1);
        //}



        //private void axMapControl1_OnAfterScreenDraw(object sender,IMapControlEvents2_OnAfterScreenDrawEvent e)
        //{
        //    IActiveView pPageLayoutView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap; //QI 方式通过焦点地图获得对象
        //    pPageLayoutView.ScreenDisplay.DisplayTransformation.VisibleBounds = axMapControl1.Extent;//控制显示范围
        //    axPageLayoutControl1.ActiveView.Refresh();
        //    MapToPage(axMapControl1, axPageLayoutControl1);
        //}
    }
}
