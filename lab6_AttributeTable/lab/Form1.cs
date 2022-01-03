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
            //lab5 test = new lab5();
            //test.Show();
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

        private void linkageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /////////////////// Linkage /////////////
            lab5 a = new lab5();
            a.Show();
        }

        ILayer layer;
        private void axTOCControl1_OnBeginLabelEdit(object sender, ITOCControlEvents_OnBeginLabelEditEvent e)
        {
            IBasicMap map = null;
            //ILayer layer = null; 
            object other = null;
            object index = null;
            //确定何种类型 item 的被选中
            //m_TOCControl = AxTOCControl.Object as ITOCControl;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            if (item == esriTOCControlItem.esriTOCControlItemLayer)
            {
                e.canEdit = true;
            }
            if (item == esriTOCControlItem.esriTOCControlItemMap)
            {
                e.canEdit = true;
            }

        }

        private void axTOCControl1_OnEndLabelEdit(object sender, ITOCControlEvents_OnEndLabelEditEvent e)
        {
            if (e.newLabel.Trim() == "")
            {
                e.canEdit = false;
            }
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            axTOCControl1.ContextMenuStrip = null;
            IBasicMap map = new MapClass(); System.Object other = null; System.Object index = null;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            if (item == esriTOCControlItem.esriTOCControlItemLayer && e.button == 2)
            {
                System.Drawing.Point pt = new System.Drawing.Point();
                pt.X = e.x;
                pt.Y = e.y;
                pt = this.axTOCControl1.PointToScreen(pt);
                this.contextMenuStrip1.Show(pt);
            }
        }

        private void displayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            attr a = new attr(layer);
            a.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.axMapControl1.Map.LayerCount; i++)
            {
                if (this.axMapControl1.Map.get_Layer(i) == layer)
                {
                    this.axMapControl1.DeleteLayer(i);
                }
            }
        }
    }
}
