using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;

namespace lab
{
    /// <summary>
    /// Summary description for Tool1.
    /// </summary>
    [Guid("32972fd5-c61e-484a-975b-62fa13e1f2dc")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("lab.Tool1")]
    public sealed class AddDateTool : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private AxToolbarControl toolbar1;
        private AxPageLayoutControl pagelayout;

        private IHookHelper m_hookHelper;

        public AddDateTool(AxToolbarControl axtoolbar, AxPageLayoutControl axpagelayout)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "�������Ԫ��"; //localizable text 
            base.m_caption = "�������";  //localizable text 
            base.m_message = "��ҳ�沼��������һ������Ԫ��";  //localizable text
            base.m_toolTip = "�������";  //localizable text
            base.m_name = "�������";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Tool1.cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
            toolbar1 = axtoolbar;
            pagelayout = axpagelayout;
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add Tool1.OnCreate implementation

            toolbar1.SetBuddyControl(pagelayout); 
            m_hookHelper.Hook = hook;
        }







        public override bool Enabled
        {
            get
            {
                // ����ʹ������
                if (m_hookHelper.ActiveView != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }




        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add Tool1.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseDown implementation

            //////////////////////////////// none ///////////////////////



            base.OnMouseDown(Button, Shift, X, Y);
            // ��ȡ���ͼ
            IActiveView activeView = m_hookHelper.ActiveView;
            // �����µ��ı�Ԫ��
            ITextElement textElement = new TextElementClass();
            // �����ı�����
            ITextSymbol textSymbol = new TextSymbolClass();
            textSymbol.Size = 25;
            // �����ı�Ԫ������
            textElement.Symbol = textSymbol;
            textElement.Text = DateTime.Now.ToShortDateString();
            // �� IElementQI
            IElement element = (IElement)textElement;
            // ����ҳ��
            IPoint point = new PointClass();

            point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            // ����Ԫ��ͼ��
            element.Geometry = point;
            // ����Ԫ�ص�ͼ�λ�������
            activeView.GraphicsContainer.AddElement(element, 0);
            // ˢ��ͼ��
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);





        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add Tool1.OnMouseUp implementation
        }
        #endregion
    }
}
