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
using ESRI.ArcGIS.DataSourcesGDB;

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
            CreateCustomizeDialog();
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

        public void MapToPage(AxMapControl axMapControl1, AxPageLayoutControl axPageLayoutControl1)
        {
            IObjectCopy pObjectCopy = new ObjectCopyClass();
            object pSourceMap = (object)axMapControl1.Map;
            object pCopiedMap = pObjectCopy.Copy(pSourceMap);
            object pOverwritedMap = (object)axPageLayoutControl1.ActiveView.FocusMap;
            pObjectCopy.Overwrite(pCopiedMap, ref pOverwritedMap);

        }

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            MapToPage(axMapControl1, axPageLayoutControl1);
        }

        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            //IDisplayTransformation displayTransformation = activeView.ScreenDisplay.DisplayTransformation;
            activeView.ScreenDisplay.DisplayTransformation.VisibleBounds = axMapControl1.Extent;
            //displayTransformation.VisibleBounds = axMapControl1.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            MapToPage(axMapControl1, axPageLayoutControl1);
        }




        //自定义对话框实例
        private ICustomizeDialog2 m_CustomizeDialog = new CustomizeDialogClass();
        //声明如下两个事件变量
        private ICustomizeDialogEvents_OnStartDialogEventHandler startDialogE;//定义事件变量或事件代理、委托，创建未实例化
        private ICustomizeDialogEvents_OnCloseDialogEventHandler closeDialogE;
        private void dIYToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CustomToolbarMenu.Checked == true)
            {
                axToolbarControl1.Customize = true;
            }
            else { axToolbarControl1.Customize = false; }




            if (CustomToolbarMenu.Checked == true)
            {
                m_CustomizeDialog.StartDialog(axTOCControl1.hWnd);//调出非模态对话 框
                                                                  //axToolbarControl1.Customize = true; 
            }
            else
            {
                m_CustomizeDialog.CloseDialog();
                //axToolbarControl1.Customize = false; 
            }
        }




        private void CreateCustomizeDialog()
        {
            //一个事件接口,由 m_CustomizeDialog 进行 QI 跳转。
            ICustomizeDialogEvents_Event pCustomizeDialogEvent = m_CustomizeDialog as ICustomizeDialogEvents_Event;
            //为当前事件产生一个代理，也就是实例化一个委托
            startDialogE = new
            ICustomizeDialogEvents_OnStartDialogEventHandler(OnStartDialogHandler);
            //利用该代理实现这个事件
            pCustomizeDialogEvent.OnStartDialog += startDialogE;
            //为当前事件产生一个代理，也就是实例化一个委托
            closeDialogE = new
            ICustomizeDialogEvents_OnCloseDialogEventHandler(OnCloseDialogHandler);
            //利用该代理实现这个事件
            pCustomizeDialogEvent.OnCloseDialog += closeDialogE;
            //设置标题
            m_CustomizeDialog.DialogTitle = "Customize ToolbarControl Items"; m_CustomizeDialog.SetDoubleClickDestination(axToolbarControl1);
        }



        private void OnStartDialogHandler()
        {
            axToolbarControl1.Customize = true; if (CustomToolbarMenu.Checked == false) { CustomToolbarMenu.Checked = true; }
        }




        private void OnCloseDialogHandler() { axToolbarControl1.Customize = false; if (CustomToolbarMenu.Checked == true) { CustomToolbarMenu.Checked = false; } }

        private void openDatabasaeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //打开对话框
            openFileDialog1.Filter = "个人数据库(*.mdb)|*.mdb";
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
            DialogResult pDialogResult = openFileDialog1.ShowDialog(); if (pDialogResult != DialogResult.OK)
                return;
            string pPath = openFileDialog1.FileName;
            string pFolder = System.IO.Path.GetDirectoryName(pPath);
            string pFileName = System.IO.Path.GetFileName(pPath);
            //打开数据库
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pPath, 0);
            if (pWorkspace != null)
            {
                MessageBox.Show("个人数据库" + pFileName + "已成功打开！");
                AddDataset2Map(pWorkspace);
            }
            else { MessageBox.Show("！警告：" + pFileName + "打开不成功……"); }
        }



        private void AddDataset2Map(IWorkspace pWorkspace)
        {
            //获取工作空间内的数据集，参数为
            IEnumDataset pEnumDataset;
            pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass); IDataset pDataset;
            pEnumDataset.Reset();
            pDataset = pEnumDataset.Next(); IFeatureClass pFeatureClass = pDataset as IFeatureClass;
            //创建图层
            IFeatureLayer pLayer = new FeatureLayerClass(); pLayer.FeatureClass = pFeatureClass; pLayer.Name = pDataset.Name;
            while (pDataset != null)
            {
                MessageBox.Show("添加要素类" + pDataset.Name + "！");
                axMapControl1.AddLayer(pLayer);
                pDataset = pEnumDataset.Next();
            }
        }
    }
}
