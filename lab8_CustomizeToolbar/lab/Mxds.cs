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
    public static class Mxds
    {
        //定义公有静态 IMapDocument 对象，并实例化
        public static IMapDocument pMapDocument = new MapDocumentClass();
        private static System.Windows.Forms.OpenFileDialog openFileDialog2;
        private static System.Windows.Forms.SaveFileDialog SaveFileDialog2;
        //新建地图文档
        public static void NewMapDoc(AxMapControl axMapControl1)
        {
            //新建地图文档对话框,并显示
            SaveFileDialog2 = new SaveFileDialog();
            SaveFileDialog2.Title = "新建地图文档";
            SaveFileDialog2.Filter = "Mxd 文档(*.mxd)|*.mxd";
            SaveFileDialog2.ShowDialog();
            string sFilePath = SaveFileDialog2.FileName;

            //新建地图文档，打开并加载到 Mapcontrol 控件


            if (sFilePath != "")
            {
                pMapDocument.New(sFilePath);
                pMapDocument.Open(sFilePath, "");
                axMapControl1.Map = pMapDocument.get_Map(0);
            }
        }





        public static void OpenMapDoc(AxMapControl axMapControl1)
        {
            //打开地图文档对话框
            openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Title = "打开地图文档";
            openFileDialog2.Filter = "Mxd 文档(*.mxd)|*.mxd";
            openFileDialog2.ShowDialog();
            string sFilePath = openFileDialog2.FileName;
            //打开地图文档，并加载到 MapControl 控件
            if (pMapDocument.get_IsMapDocument(sFilePath))
            {
                pMapDocument.Open(sFilePath, "");//打开文档
                for (int i = 0; i < pMapDocument.MapCount; i++)
                {
                    axMapControl1.Map = pMapDocument.get_Map(i);
                }
                axMapControl1.Refresh();
            }
        }



        public static void SaveMapDoc()
        {
            if (pMapDocument.get_IsReadOnly(pMapDocument.DocumentFilename) == true)
            {
                MessageBox.Show("地图文档只读!");
                return;
            }
            pMapDocument.Save(pMapDocument.UsesRelativePaths, true);
            MessageBox.Show("保存成功!");
        }




        public static void SaveAsMapDoc()
        {
            //地图文档保存对话框
            SaveFileDialog2 = new SaveFileDialog();
            SaveFileDialog2.Title = "地图文档另存为";
            SaveFileDialog2.Filter = "Mxd 文档(*.mxd)|*.mxd";
            SaveFileDialog2.ShowDialog();
            string sFilePath = SaveFileDialog2.FileName;
            //当路径为空，返回
            if (sFilePath == "")
            {
                return;
            }
            //当文件名和当前文件相同，保存到当前文档
            if (sFilePath == pMapDocument.DocumentFilename)
            {
                //保存到当前文档：调用保存文档方法
                SaveMapDoc();
            }
            else
            {
                //另存一个新文档
                pMapDocument.SaveAs(sFilePath, true, true);
            }
        }










        // ===================
    }

}