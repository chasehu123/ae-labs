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
    public partial class Form1 : Form
    {
        IMapDocument pMapDocument;
        public Form1()
        {
            InitializeComponent();
            lab5 test = new lab5();
            test.Show();
        }

        

        private void addShpToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            /////////////////////// Add Shp /////////////////
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "shapefile 文件(*.shp)|*.shp";
            //openFileDialog1.InitialDirectory = @"D:\GIS-Data";
            openFileDialog1.Multiselect = false;
            DialogResult pDialogResult = openFileDialog1.ShowDialog();
            if (pDialogResult != DialogResult.OK)
                return;
            string pPath = openFileDialog1.FileName;
            string pFolder = System.IO.Path.GetDirectoryName(pPath);
            string pFileName = System.IO.Path.GetFileName(pPath);
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pFolder, 0);
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;//QI 跳转
            IFeatureClass pFC = pFeatureWorkspace.OpenFeatureClass(pFileName);//可实例化类
            IFeatureLayer pFLayer = new FeatureLayerClass();
            pFLayer.FeatureClass = pFC;
            pFLayer.Name = pFC.AliasName;
            ILayer pLayer = pFLayer as ILayer;
            IMap pMap = axMapControl1.Map;
            pMap.AddLayer(pLayer);
            axMapControl1.ActiveView.Refresh();
        }

        private void openMxdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////////////////// Add Mxd /////////////////////
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "地图文档(*.mxd)|*.mxd"; openFileDialog1.Multiselect = false;
            DialogResult pDialogResult = openFileDialog1.ShowDialog();
            if (pDialogResult != DialogResult.OK)
                return;
            string pPath = openFileDialog1.FileName;
            //将数据加载入 pMapDocument 并与 map 控件联系起来

            //IMapMapDocument pMapDocument; 
            pMapDocument = new MapDocumentClass();
            pMapDocument.Open(pPath, "");
            for (int i = 0; i < pMapDocument.MapCount; i++)
            {
                //遍历可能的 Map 对象
                axMapControl1.Map = pMapDocument.get_Map(i);
            }//刷新地图
            axMapControl1.Refresh();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///////////////// Mxds Open ////////////////
            lab.Mxds.OpenMapDoc(axMapControl1);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /////////////////// Mxds New //////////////
            lab.Mxds.NewMapDoc(axMapControl1);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///////////////// Mxds Save ///////////
            lab.Mxds.SaveMapDoc();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /////////////////// Mxds Save as //////////////////
            lab.Mxds.SaveAsMapDoc();
        }
    }
}
