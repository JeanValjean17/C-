//To compile the project you need the extended version of the CADImport .NET library
//that allows to access CAD entities directly.  You can get this version of the 
//library free of charge by emailing your request to: info@cadsofttools.com
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CADImport;
using CADImport.CADImportForms;
using CADImport.FaceModule;
using CADImport.RasterImage;
using CADImport.Professional;
using CADImport.HPGL2;
//using CADImport.DXF;
using CADImport.Export;
using CADImport.Export.DirectCADtoDXF;
using CADImport.Printing;

namespace CADImportNetDemos.CADEditorDemo
{
	#region Help
	#if RUSHELP
	/// <summary>
	/// Представляет собой главную форму демонстрационной программы Редактор CAD файлов
	/// </summary>
	#else
	/// <summary>
	/// Represents the main application form where CAD image and added CAD entities can be viewed.
	/// </summary>
	#endif
	#endregion Help
	public class MainForm: System.Windows.Forms.Form
	{
        private CADImport.CADImportForms.GridForm gForm;
		private CADImport.CADImportForms.LayerForm lForm;		
		private CADImport.CADImportForms.AboutForm aboutForm;
		private CADImport.CADImportForms.SHXForm shxFrm;
		private CADImport.Printing.PrintingForm prtForm;		
		private CADImport.CADImportForms.SetRasterSizeForm rasterSizeForm;
		private static CADImport.CADImportForms.BugReportForm bugReport;
		#region protect
#if protect
		private RegForm regFrm;
#endif
		#endregion protect		

		private PointF old_Pos;	
		private PointF pos;
		private SizeF visibleArea;
		private float sc;				
		private int cX, cY;
		private bool detMouseDown;
		private float prev_sc;		
		private bool moveMarker;				
		private CADImage cadImage;
		private ClipRect clipRectangle;
		private MultipleLanguage mlng;
		private int curLngInd;		
		private string lngFile;
		private SortedList settingsLst;
		private SaveSettings svSet;		
		private int addEntity;
		private bool colorDraw;
		private bool dimVisible;
		private bool showLineWeight;
		private bool textVisible;
		private bool cntrlDown;			
		private bool stopSnap;
		private bool enableSnap;
		private EntitiesCreator entCreator;		
		private Point startPoint;
		private Point endPoint;		
		private CreatorType curAddEntityType;		
		private static string fileName;	
		private static readonly string fileSettingsName;
		private bool useSHXFonts = true;
        private int btnOffset = 8;
        private int btnLast;
        private bool selectedEntitiesChanged;
		private CADImport.FaceModule.CADPictureBox cadPictBox;

		private System.Windows.Forms.StatusBar stBar;
		private System.Windows.Forms.StatusBarPanel sbOpen;
		private System.Windows.Forms.StatusBarPanel sbScale;
		private System.Windows.Forms.ToolBarButton tlbOpen;
		private System.Windows.Forms.ToolBarButton tlbZoomIn;
		private System.Windows.Forms.ToolBarButton tlbZoomOut;
		private System.Windows.Forms.ToolBarButton tlbLay;
		private System.Windows.Forms.ToolBarButton tlbWhite;
		private System.Windows.Forms.ToolBarButton tlbBlack;
		private System.Windows.Forms.ToolBarButton tlbSave;
		private System.Windows.Forms.ToolBar tlbTool;
		private System.Windows.Forms.StatusBarPanel sbCoord;		
		private System.Windows.Forms.ImageList toolBtnImageList;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.SaveFileDialog saveImgDlg;
		private System.Windows.Forms.SaveFileDialog saveDXFDlg;
        private System.Windows.Forms.ToolBarButton tlbShx;								
		private System.Windows.Forms.ToolBarButton tlbAddImgEnt;
		private System.Windows.Forms.ToolBarButton tlbOptions;
		private System.Windows.Forms.ToolBarButton tlbLeader;
		private System.Windows.Forms.ToolBarButton tlbNew;
		private System.Windows.Forms.ToolBarButton tlbLine;
		private System.Windows.Forms.ToolBarButton tlbRect;
		private System.Windows.Forms.ToolBarButton tlbEllipse;
		private System.Windows.Forms.ToolBarButton tlbText;
		private System.Windows.Forms.ToolBarButton tlbPoly;
		private System.Windows.Forms.ToolBarButton tlbPen;
		private System.Windows.Forms.ToolBarButton tlbAddMText;
		private System.Windows.Forms.Splitter spltPictBox;
		private System.Windows.Forms.ToolBarButton tlbGetExt;
		private System.Windows.Forms.PrintPreviewDialog printPrevDlg;
		private System.Windows.Forms.MenuItem miLng;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.ToolBarButton tlbPoint;
		private System.Windows.Forms.ToolBarButton tlbDel;
		private System.Windows.Forms.ToolBarButton tlbUndo;
		private System.Windows.Forms.ToolBarButton tlbRedo;			
		private System.Windows.Forms.MenuItem miUndo;
		private System.Windows.Forms.MenuItem miRedo;
		private System.Windows.Forms.MenuItem miDelete;
		private System.Windows.Forms.MenuItem miPrintCustom;		
		private System.Windows.Forms.ColorDialog setColorDlg;
		private System.Windows.Forms.ToolBarButton tlbCloud;
		private System.Windows.Forms.ToolBarButton tlbAddHatch;
		private System.Windows.Forms.MenuItem miCopy;
		private System.Windows.Forms.MenuItem miCut;
		private System.Windows.Forms.MenuItem miPaste;
		private System.Windows.Forms.MenuItem miFile;
		private System.Windows.Forms.MenuItem miAbout;
		private System.Windows.Forms.MenuItem miView;
		private System.Windows.Forms.OpenFileDialog openFileDlg;
		private System.Windows.Forms.MenuItem miSerializeEntities;
		private System.Windows.Forms.MenuItem miDeserializeEntities;
		private System.Windows.Forms.SaveFileDialog dlgSaveSerializeFile;
		private System.Windows.Forms.OpenFileDialog dlgOpenSerializeFile;
		private System.Windows.Forms.MenuItem miOpenFile;
		private System.Windows.Forms.MenuItem miSaveFile;
		private System.Windows.Forms.MenuItem miSaveAsDXF;
		private System.Windows.Forms.MenuItem miPrint;
		private System.Windows.Forms.MenuItem miPrintPreview;
		private System.Windows.Forms.MenuItem miClose;
		private System.Windows.Forms.MenuItem miExit;
		private System.Windows.Forms.MenuItem miSep1;
		private System.Windows.Forms.MenuItem miCopyAsBMP;
		private System.Windows.Forms.MenuItem miSep2;
		private System.Windows.Forms.MenuItem miZoomIn;
		private System.Windows.Forms.MenuItem miSep3;
		private System.Windows.Forms.MenuItem miZoomOut;
		private System.Windows.Forms.MenuItem miEntitiesTree;
		private System.Windows.Forms.MenuItem miScale;
		private System.Windows.Forms.MenuItem miZoom10;
		private System.Windows.Forms.MenuItem miZoom25;
		private System.Windows.Forms.MenuItem miZoom50;
		private System.Windows.Forms.MenuItem miZoom100;
		private System.Windows.Forms.MenuItem miZoom200;
		private System.Windows.Forms.MenuItem miZoom400;
		private System.Windows.Forms.MenuItem miZoom800;
		private System.Windows.Forms.MenuItem miFit;
		private System.Windows.Forms.MenuItem miSHXFontsPaths;
		private System.Windows.Forms.MenuItem miOptions;
		private System.Windows.Forms.MenuItem miColorDraw;
		private System.Windows.Forms.MenuItem miBlackDraw;
		private System.Windows.Forms.MenuItem miWhiteBack;
		private System.Windows.Forms.MenuItem miBlackBack;
		private System.Windows.Forms.MenuItem miSep4;
        private System.Windows.Forms.MenuItem miShowLineWeight;
		private System.Windows.Forms.MenuItem miDimShow;
		private System.Windows.Forms.MenuItem miTextsShow;
		private System.Windows.Forms.MenuItem miSep5;
        private System.Windows.Forms.MenuItem miLayersShow;
		private System.Windows.Forms.MenuItem miEdit;
		private System.Windows.Forms.MenuItem miCADFiles;
        private System.Windows.Forms.MenuItem miReg;
        private ToolBarButton tlbGrid;
        private ToolBarButton tlbOrtho;
        private ToolBarButton tlbCircle;
        private ToolBarButton tlbArc;
		private System.Windows.Forms.ToolBarButton tlbSnap;	

		#region Help
		#if RUSHELP
		/// <summary>
		/// Получает или задает коэффициент мастаба рисуемого изображения
		/// </summary>
		#else
		/// <summary>
		/// Gets or sets scale of the drawing image
		/// </summary>
		#endif
		#endregion
		public float ImageScale
		{
			get
			{
				return this.sc;
			}
			set
			{
				this.sc = value;
			}
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Получает или задает базовые размеры области вывода чертежа
		/// </summary>
		#else
		/// <summary>
		/// Gets or sets base dimensions of the image drawing area
		/// </summary>
		#endif
		#endregion
		public SizeF VisibleAreaSize
		{
			get
			{
				return this.visibleArea;
			}
			set
			{
				this.visibleArea = value;
			}
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Получает текущий размер и позицию рисуемого изображения
		/// </summary>
		#else
		/// <summary>
		/// Gets the current size and position of the drawing image
		/// </summary>
		#endif
		#endregion
		public RectangleF ImageRectangleF
		{
			get
			{
				return new RectangleF(pos.X, pos.Y, VisibleAreaSize.Width * ImageScale, VisibleAreaSize.Height * ImageScale);
			}
		}

		#region Help
		/// <summary>
		/// A previous value of the scale in which the current CAD image is displayed in the viewer.
		/// </summary>
		#endregion Help
		public float ImagePreviousScale
		{
			get
			{
				return this.prev_sc;
			}
			set
			{
				this.prev_sc = value;
			}
		}


		#region Help
#if RUSHELP
		/// <summary>
		/// Получает текущий размер и позицию рисуемого изображения
		/// </summary>
#else
		/// <summary>
		/// Gets the current size and position of the drawing image
		/// </summary>
#endif
		#endregion
		public DRect ImageDRect
		{
			get
			{
				return new DRect(pos.X, pos.Y, VisibleAreaSize.Width * ImageScale, VisibleAreaSize.Height * ImageScale);								
			}
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Получает или задает позицию рисуемого изображения
		/// </summary>
		#else
		/// <summary>
		/// Gets or sets a position of the drawing image
		/// </summary>
		#endif
		#endregion
		public PointF Position
		{
			get
			{
				return this.pos;
			}
			set
			{
				this.pos = value;
			}
		}
		
		#region Help
		/// <summary>
		/// A previous position of the current CAD image in relation to the top-left corner of the viewer.
		/// </summary>
		#endregion Help
		public PointF PreviousPosition
		{
			get
			{
				return this.old_Pos;
			}
			set
			{
				this.old_Pos = value;
			}
		}	

		#region Help
#if RUSHELP
		/// <summary>
		/// Получает или задает позицию рисуемого изображения по оси X
		/// </summary>
#else	
		/// <summary>
		/// Gets or sets a position of the drawing image by X axis
		/// </summary>
		
#endif
		#endregion
		public float LeftImagePosition
		{
			get
			{
				return this.pos.X;
			}
			set
			{
				this.pos.X = value;
			}
		}

		#region Help
#if RUSHELP
		/// <summary>
		/// Получает или задает позицию рисуемого изображения по оси Y
		/// </summary>
#else
		/// <summary>
		/// Gets or sets a position of the drawing image by Y axis
		/// </summary>
#endif
		#endregion
		public float TopImagePosition
		{
			get
			{
				return this.pos.Y;
			}
			set
			{
				this.pos.Y = value;
			}
		}
		
		#region Help
		#if RUSHELP	
		/// <summary>
		/// Получает элемент управления <see cref="CADImport.FaceModule.CADPictureBox">CADPictureBox</see> на котором осуществляется отрисовка изображения
		/// </summary>
		#else
		/// <summary>
		/// Gets <see cref="CADImport.FaceModule.CADPictureBox">CADPictureBox</see> control, where an image is drawn
		/// </summary>
		#endif
		#endregion
		public CADImport.FaceModule.CADPictureBox EditorCADPictureBox
		{
			get
			{
				 return this.cadPictBox;
			}
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Выбраная до операции (перемещения или изменения по маркеру) изначальная точка примитива
		/// </summary>
		#else
		/// <summary>
		/// Gets or sets entity point which was selected before offsetting or editing by marker
		/// </summary>
		#endif
		#endregion
		public Point StartPoint 
		{
			get
			{
				return this.startPoint;
			}
			set
			{
				this.startPoint = value;
			}
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Конечная точка примитива, после завершения операции над примитивом(перемещения или изменения по маркеру)
		/// </summary>
		#else
		/// <summary>
		/// Gets or sets entity point which entity has after offsetting or editing by marker
		/// </summary>
		#endif
		#endregion
		public Point EndPoint 
		{
			get
			{
				return this.endPoint;
			}
			set
			{
				this.endPoint = value;
			}
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Получает объект <see cref="CADImport.CADImage"/>CADImage,
		/// представляющий собой основной класс для чтения/отрисовки 
		/// просмотра содержимого cad файлов
		/// </summary>
		#else
	
		/// <summary>
		/// Gets or sets <see cref="CADImport.CADImage">CADImage</see> object where new entities are added
		/// </summary>
	
		#endif
		#endregion Help
		public CADImage Image
		{
			get
			{
				return this.cadImage;
			}
		}
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.stBar = new System.Windows.Forms.StatusBar();
            this.sbOpen = new System.Windows.Forms.StatusBarPanel();
            this.sbScale = new System.Windows.Forms.StatusBarPanel();
            this.sbCoord = new System.Windows.Forms.StatusBarPanel();
            this.tlbTool = new System.Windows.Forms.ToolBar();
            this.tlbOpen = new System.Windows.Forms.ToolBarButton();
            this.tlbZoomIn = new System.Windows.Forms.ToolBarButton();
            this.tlbZoomOut = new System.Windows.Forms.ToolBarButton();
            this.tlbWhite = new System.Windows.Forms.ToolBarButton();
            this.tlbBlack = new System.Windows.Forms.ToolBarButton();
            this.tlbLay = new System.Windows.Forms.ToolBarButton();
            this.tlbSave = new System.Windows.Forms.ToolBarButton();
            this.tlbShx = new System.Windows.Forms.ToolBarButton();
            this.tlbAddImgEnt = new System.Windows.Forms.ToolBarButton();
            this.tlbPoint = new System.Windows.Forms.ToolBarButton();
            this.tlbLine = new System.Windows.Forms.ToolBarButton();
            this.tlbLeader = new System.Windows.Forms.ToolBarButton();
            this.tlbRect = new System.Windows.Forms.ToolBarButton();
            this.tlbPoly = new System.Windows.Forms.ToolBarButton();
            this.tlbPen = new System.Windows.Forms.ToolBarButton();
            this.tlbCloud = new System.Windows.Forms.ToolBarButton();
            this.tlbCircle = new System.Windows.Forms.ToolBarButton();
            this.tlbEllipse = new System.Windows.Forms.ToolBarButton();
            this.tlbArc = new System.Windows.Forms.ToolBarButton();
            this.tlbAddHatch = new System.Windows.Forms.ToolBarButton();
            this.tlbText = new System.Windows.Forms.ToolBarButton();
            this.tlbAddMText = new System.Windows.Forms.ToolBarButton();
            this.tlbOptions = new System.Windows.Forms.ToolBarButton();
            this.tlbNew = new System.Windows.Forms.ToolBarButton();
            this.tlbGetExt = new System.Windows.Forms.ToolBarButton();
            this.tlbDel = new System.Windows.Forms.ToolBarButton();
            this.tlbUndo = new System.Windows.Forms.ToolBarButton();
            this.tlbRedo = new System.Windows.Forms.ToolBarButton();
            this.tlbSnap = new System.Windows.Forms.ToolBarButton();
            this.tlbGrid = new System.Windows.Forms.ToolBarButton();
            this.tlbOrtho = new System.Windows.Forms.ToolBarButton();
            this.toolBtnImageList = new System.Windows.Forms.ImageList(this.components);
            this.saveImgDlg = new System.Windows.Forms.SaveFileDialog();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.miFile = new System.Windows.Forms.MenuItem();
            this.miOpenFile = new System.Windows.Forms.MenuItem();
            this.miSaveFile = new System.Windows.Forms.MenuItem();
            this.miSaveAsDXF = new System.Windows.Forms.MenuItem();
            this.miSerializeEntities = new System.Windows.Forms.MenuItem();
            this.miDeserializeEntities = new System.Windows.Forms.MenuItem();
            this.miPrint = new System.Windows.Forms.MenuItem();
            this.miPrintCustom = new System.Windows.Forms.MenuItem();
            this.miPrintPreview = new System.Windows.Forms.MenuItem();
            this.miClose = new System.Windows.Forms.MenuItem();
            this.miSep1 = new System.Windows.Forms.MenuItem();
            this.miExit = new System.Windows.Forms.MenuItem();
            this.miEdit = new System.Windows.Forms.MenuItem();
            this.miCopyAsBMP = new System.Windows.Forms.MenuItem();
            this.miUndo = new System.Windows.Forms.MenuItem();
            this.miRedo = new System.Windows.Forms.MenuItem();
            this.miDelete = new System.Windows.Forms.MenuItem();
            this.miCopy = new System.Windows.Forms.MenuItem();
            this.miCut = new System.Windows.Forms.MenuItem();
            this.miPaste = new System.Windows.Forms.MenuItem();
            this.miView = new System.Windows.Forms.MenuItem();
            this.miEntitiesTree = new System.Windows.Forms.MenuItem();
            this.miSep2 = new System.Windows.Forms.MenuItem();
            this.miZoomIn = new System.Windows.Forms.MenuItem();
            this.miZoomOut = new System.Windows.Forms.MenuItem();
            this.miScale = new System.Windows.Forms.MenuItem();
            this.miZoom10 = new System.Windows.Forms.MenuItem();
            this.miZoom25 = new System.Windows.Forms.MenuItem();
            this.miZoom50 = new System.Windows.Forms.MenuItem();
            this.miZoom100 = new System.Windows.Forms.MenuItem();
            this.miZoom200 = new System.Windows.Forms.MenuItem();
            this.miZoom400 = new System.Windows.Forms.MenuItem();
            this.miZoom800 = new System.Windows.Forms.MenuItem();
            this.miFit = new System.Windows.Forms.MenuItem();
            this.miSep3 = new System.Windows.Forms.MenuItem();
            this.miSHXFontsPaths = new System.Windows.Forms.MenuItem();
            this.miOptions = new System.Windows.Forms.MenuItem();
            this.miCADFiles = new System.Windows.Forms.MenuItem();
            this.miColorDraw = new System.Windows.Forms.MenuItem();
            this.miBlackDraw = new System.Windows.Forms.MenuItem();
            this.miWhiteBack = new System.Windows.Forms.MenuItem();
            this.miBlackBack = new System.Windows.Forms.MenuItem();
            this.miSep4 = new System.Windows.Forms.MenuItem();
            this.miShowLineWeight = new System.Windows.Forms.MenuItem();
            this.miDimShow = new System.Windows.Forms.MenuItem();
            this.miTextsShow = new System.Windows.Forms.MenuItem();
            this.miSep5 = new System.Windows.Forms.MenuItem();
            this.miLayersShow = new System.Windows.Forms.MenuItem();
            this.miReg = new System.Windows.Forms.MenuItem();
            this.miLng = new System.Windows.Forms.MenuItem();
            this.miAbout = new System.Windows.Forms.MenuItem();
            this.saveDXFDlg = new System.Windows.Forms.SaveFileDialog();
            this.spltPictBox = new System.Windows.Forms.Splitter();
            this.printPrevDlg = new System.Windows.Forms.PrintPreviewDialog();
            this.setColorDlg = new System.Windows.Forms.ColorDialog();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.dlgSaveSerializeFile = new System.Windows.Forms.SaveFileDialog();
            this.dlgOpenSerializeFile = new System.Windows.Forms.OpenFileDialog();
            this.cadPictBox = new CADImport.FaceModule.CADPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.sbOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbCoord)).BeginInit();
            this.SuspendLayout();
            // 
            // stBar
            // 
            this.stBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.stBar.Location = new System.Drawing.Point(0, 647);
            this.stBar.Name = "stBar";
            this.stBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbOpen,
            this.sbScale,
            this.sbCoord});
            this.stBar.ShowPanels = true;
            this.stBar.Size = new System.Drawing.Size(1218, 16);
            this.stBar.TabIndex = 9;
            // 
            // sbOpen
            // 
            this.sbOpen.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbOpen.MinWidth = 100;
            this.sbOpen.Name = "sbOpen";
            this.sbOpen.ToolTipText = "File Name";
            // 
            // sbScale
            // 
            this.sbScale.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbScale.Name = "sbScale";
            this.sbScale.Text = "100%";
            this.sbScale.ToolTipText = "Scale";
            this.sbScale.Width = 42;
            // 
            // sbCoord
            // 
            this.sbCoord.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.sbCoord.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbCoord.Name = "sbCoord";
            this.sbCoord.Text = "0 - 0 - 0";
            this.sbCoord.Width = 51;
            // 
            // tlbTool
            // 
            this.tlbTool.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tlbTool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlbTool.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tlbOpen,
            this.tlbZoomIn,
            this.tlbZoomOut,
            this.tlbWhite,
            this.tlbBlack,
            this.tlbLay,
            this.tlbSave,
            this.tlbShx,
            this.tlbAddImgEnt,
            this.tlbPoint,
            this.tlbLine,
            this.tlbLeader,
            this.tlbRect,
            this.tlbPoly,
            this.tlbPen,
            this.tlbCloud,
            this.tlbCircle,
            this.tlbEllipse,
            this.tlbArc,
            this.tlbAddHatch,
            this.tlbText,
            this.tlbAddMText,
            this.tlbOptions,
            this.tlbNew,
            this.tlbGetExt,
            this.tlbDel,
            this.tlbUndo,
            this.tlbRedo,
            this.tlbSnap,
            this.tlbGrid,
            this.tlbOrtho});
            this.tlbTool.ButtonSize = new System.Drawing.Size(20, 20);
            this.tlbTool.DropDownArrows = true;
            this.tlbTool.ImageList = this.toolBtnImageList;
            this.tlbTool.Location = new System.Drawing.Point(3, 0);
            this.tlbTool.Name = "tlbTool";
            this.tlbTool.ShowToolTips = true;
            this.tlbTool.Size = new System.Drawing.Size(1215, 29);
            this.tlbTool.TabIndex = 10;
            this.tlbTool.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tlbTool_ButtonClick);
            this.tlbTool.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CADPictBoxKeyDown);
            // 
            // tlbOpen
            // 
            this.tlbOpen.ImageIndex = 1;
            this.tlbOpen.Name = "tlbOpen";
            this.tlbOpen.ToolTipText = "Open Picture";
            // 
            // tlbZoomIn
            // 
            this.tlbZoomIn.Enabled = false;
            this.tlbZoomIn.ImageIndex = 2;
            this.tlbZoomIn.Name = "tlbZoomIn";
            this.tlbZoomIn.ToolTipText = "Zoom In (+)";
            // 
            // tlbZoomOut
            // 
            this.tlbZoomOut.Enabled = false;
            this.tlbZoomOut.ImageIndex = 3;
            this.tlbZoomOut.Name = "tlbZoomOut";
            this.tlbZoomOut.ToolTipText = "Zoom Out (-)";
            // 
            // tlbWhite
            // 
            this.tlbWhite.Enabled = false;
            this.tlbWhite.ImageIndex = 4;
            this.tlbWhite.Name = "tlbWhite";
            this.tlbWhite.ToolTipText = "White Background";
            // 
            // tlbBlack
            // 
            this.tlbBlack.Enabled = false;
            this.tlbBlack.ImageIndex = 5;
            this.tlbBlack.Name = "tlbBlack";
            this.tlbBlack.ToolTipText = "Black Background";
            // 
            // tlbLay
            // 
            this.tlbLay.Enabled = false;
            this.tlbLay.ImageIndex = 6;
            this.tlbLay.Name = "tlbLay";
            this.tlbLay.ToolTipText = "Show Layers";
            // 
            // tlbSave
            // 
            this.tlbSave.Enabled = false;
            this.tlbSave.ImageIndex = 7;
            this.tlbSave.Name = "tlbSave";
            this.tlbSave.ToolTipText = "Save Picture...";
            // 
            // tlbShx
            // 
            this.tlbShx.Enabled = false;
            this.tlbShx.ImageIndex = 8;
            this.tlbShx.Name = "tlbShx";
            this.tlbShx.Pushed = true;
            this.tlbShx.ToolTipText = "Use SHX fonts";
            // 
            // tlbAddImgEnt
            // 
            this.tlbAddImgEnt.ImageIndex = 12;
            this.tlbAddImgEnt.Name = "tlbAddImgEnt";
            this.tlbAddImgEnt.ToolTipText = "Add new ImageEnt";
            // 
            // tlbPoint
            // 
            this.tlbPoint.ImageIndex = 21;
            this.tlbPoint.Name = "tlbPoint";
            // 
            // tlbLine
            // 
            this.tlbLine.ImageIndex = 13;
            this.tlbLine.Name = "tlbLine";
            this.tlbLine.ToolTipText = "Add new line";
            // 
            // tlbLeader
            // 
            this.tlbLeader.ImageIndex = 10;
            this.tlbLeader.Name = "tlbLeader";
            this.tlbLeader.ToolTipText = "Add new leader";
            // 
            // tlbRect
            // 
            this.tlbRect.ImageIndex = 14;
            this.tlbRect.Name = "tlbRect";
            this.tlbRect.ToolTipText = "Add new rect";
            // 
            // tlbPoly
            // 
            this.tlbPoly.ImageIndex = 17;
            this.tlbPoly.Name = "tlbPoly";
            this.tlbPoly.ToolTipText = "Add new poly";
            // 
            // tlbPen
            // 
            this.tlbPen.ImageIndex = 18;
            this.tlbPen.Name = "tlbPen";
            this.tlbPen.ToolTipText = "Add new pen";
            // 
            // tlbCloud
            // 
            this.tlbCloud.ImageIndex = 25;
            this.tlbCloud.Name = "tlbCloud";
            // 
            // tlbCircle
            // 
            this.tlbCircle.ImageIndex = 30;
            this.tlbCircle.Name = "tlbCircle";
            this.tlbCircle.ToolTipText = "Add new circle";
            // 
            // tlbEllipse
            // 
            this.tlbEllipse.ImageIndex = 15;
            this.tlbEllipse.Name = "tlbEllipse";
            this.tlbEllipse.ToolTipText = "Add new ellipse";
            // 
            // tlbArc
            // 
            this.tlbArc.ImageIndex = 19;
            this.tlbArc.Name = "tlbArc";
            this.tlbArc.ToolTipText = "Add new arc";
            // 
            // tlbAddHatch
            // 
            this.tlbAddHatch.ImageIndex = 26;
            this.tlbAddHatch.Name = "tlbAddHatch";
            // 
            // tlbText
            // 
            this.tlbText.ImageIndex = 16;
            this.tlbText.Name = "tlbText";
            this.tlbText.ToolTipText = "Add new text";
            // 
            // tlbAddMText
            // 
            this.tlbAddMText.ImageIndex = 16;
            this.tlbAddMText.Name = "tlbAddMText";
            this.tlbAddMText.ToolTipText = "Add MText";
            // 
            // tlbOptions
            // 
            this.tlbOptions.ImageIndex = 9;
            this.tlbOptions.Name = "tlbOptions";
            this.tlbOptions.ToolTipText = "Options";
            // 
            // tlbNew
            // 
            this.tlbNew.ImageIndex = 11;
            this.tlbNew.Name = "tlbNew";
            this.tlbNew.ToolTipText = "New Picture";
            // 
            // tlbGetExt
            // 
            this.tlbGetExt.ImageIndex = 20;
            this.tlbGetExt.Name = "tlbGetExt";
            this.tlbGetExt.ToolTipText = "Fit drawing to window";
            // 
            // tlbDel
            // 
            this.tlbDel.Enabled = false;
            this.tlbDel.ImageIndex = 24;
            this.tlbDel.Name = "tlbDel";
            // 
            // tlbUndo
            // 
            this.tlbUndo.Enabled = false;
            this.tlbUndo.ImageIndex = 23;
            this.tlbUndo.Name = "tlbUndo";
            this.tlbUndo.ToolTipText = "Undo";
            // 
            // tlbRedo
            // 
            this.tlbRedo.Enabled = false;
            this.tlbRedo.ImageIndex = 22;
            this.tlbRedo.Name = "tlbRedo";
            // 
            // tlbSnap
            // 
            this.tlbSnap.ImageIndex = 27;
            this.tlbSnap.Name = "tlbSnap";
            this.tlbSnap.Pushed = true;
            // 
            // tlbGrid
            // 
            this.tlbGrid.ImageIndex = 28;
            this.tlbGrid.Name = "tlbGrid";
            this.tlbGrid.ToolTipText = "Grid";
            // 
            // tlbOrtho
            // 
            this.tlbOrtho.ImageIndex = 29;
            this.tlbOrtho.Name = "tlbOrtho";
            // 
            // toolBtnImageList
            // 
            this.toolBtnImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolBtnImageList.ImageStream")));
            this.toolBtnImageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.toolBtnImageList.Images.SetKeyName(0, "");
            this.toolBtnImageList.Images.SetKeyName(1, "");
            this.toolBtnImageList.Images.SetKeyName(2, "");
            this.toolBtnImageList.Images.SetKeyName(3, "");
            this.toolBtnImageList.Images.SetKeyName(4, "");
            this.toolBtnImageList.Images.SetKeyName(5, "");
            this.toolBtnImageList.Images.SetKeyName(6, "");
            this.toolBtnImageList.Images.SetKeyName(7, "");
            this.toolBtnImageList.Images.SetKeyName(8, "");
            this.toolBtnImageList.Images.SetKeyName(9, "");
            this.toolBtnImageList.Images.SetKeyName(10, "");
            this.toolBtnImageList.Images.SetKeyName(11, "");
            this.toolBtnImageList.Images.SetKeyName(12, "");
            this.toolBtnImageList.Images.SetKeyName(13, "");
            this.toolBtnImageList.Images.SetKeyName(14, "");
            this.toolBtnImageList.Images.SetKeyName(15, "");
            this.toolBtnImageList.Images.SetKeyName(16, "");
            this.toolBtnImageList.Images.SetKeyName(17, "");
            this.toolBtnImageList.Images.SetKeyName(18, "");
            this.toolBtnImageList.Images.SetKeyName(19, "");
            this.toolBtnImageList.Images.SetKeyName(20, "");
            this.toolBtnImageList.Images.SetKeyName(21, "");
            this.toolBtnImageList.Images.SetKeyName(22, "");
            this.toolBtnImageList.Images.SetKeyName(23, "");
            this.toolBtnImageList.Images.SetKeyName(24, "");
            this.toolBtnImageList.Images.SetKeyName(25, "");
            this.toolBtnImageList.Images.SetKeyName(26, "");
            this.toolBtnImageList.Images.SetKeyName(27, "");
            this.toolBtnImageList.Images.SetKeyName(28, "");
            this.toolBtnImageList.Images.SetKeyName(29, "");
            this.toolBtnImageList.Images.SetKeyName(30, "");
            // 
            // saveImgDlg
            // 
            this.saveImgDlg.Filter = resources.GetString("saveImgDlg.Filter");
            this.saveImgDlg.RestoreDirectory = true;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miFile,
            this.miEdit,
            this.miView,
            this.miCADFiles,
            this.miReg});
            // 
            // miFile
            // 
            this.miFile.Index = 0;
            this.miFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miOpenFile,
            this.miSaveFile,
            this.miSaveAsDXF,
            this.miSerializeEntities,
            this.miDeserializeEntities,
            this.miPrint,
            this.miPrintCustom,
            this.miPrintPreview,
            this.miClose,
            this.miSep1,
            this.miExit});
            this.miFile.Text = "&File";
            // 
            // miOpenFile
            // 
            this.miOpenFile.Index = 0;
            this.miOpenFile.Text = "Open...";
            this.miOpenFile.Click += new System.EventHandler(this.miOpenFile_Click);
            // 
            // miSaveFile
            // 
            this.miSaveFile.Enabled = false;
            this.miSaveFile.Index = 1;
            this.miSaveFile.Text = "Save...";
            // 
            // miSaveAsDXF
            // 
            this.miSaveAsDXF.Enabled = false;
            this.miSaveAsDXF.Index = 2;
            this.miSaveAsDXF.Text = "Save as DXF...";
            // 
            // miSerializeEntities
            // 
            this.miSerializeEntities.Enabled = false;
            this.miSerializeEntities.Index = 3;
            this.miSerializeEntities.Text = "Serialize entities...";
            this.miSerializeEntities.Click += new System.EventHandler(this.miSerializeEntities_Click);
            // 
            // miDeserializeEntities
            // 
            this.miDeserializeEntities.Index = 4;
            this.miDeserializeEntities.Text = "Deserialize entities...";
            this.miDeserializeEntities.Click += new System.EventHandler(this.miDeserializeEntities_Click);
            // 
            // miPrint
            // 
            this.miPrint.Enabled = false;
            this.miPrint.Index = 5;
            this.miPrint.Text = "Print ";
            // 
            // miPrintCustom
            // 
            this.miPrintCustom.Enabled = false;
            this.miPrintCustom.Index = 6;
            this.miPrintCustom.Text = "Custom Print Preview...";
            this.miPrintCustom.Click += new System.EventHandler(this.miPrintCustom_Click);
            // 
            // miPrintPreview
            // 
            this.miPrintPreview.Enabled = false;
            this.miPrintPreview.Index = 7;
            this.miPrintPreview.Text = "Print Preview...";
            // 
            // miClose
            // 
            this.miClose.Enabled = false;
            this.miClose.Index = 8;
            this.miClose.Text = "Close";
            // 
            // miSep1
            // 
            this.miSep1.Index = 9;
            this.miSep1.Text = "-";
            // 
            // miExit
            // 
            this.miExit.Index = 10;
            this.miExit.Text = "Exit";
            // 
            // miEdit
            // 
            this.miEdit.Index = 1;
            this.miEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miCopyAsBMP,
            this.miUndo,
            this.miRedo,
            this.miDelete,
            this.miCopy,
            this.miCut,
            this.miPaste});
            this.miEdit.Text = "&Edit";
            // 
            // miCopyAsBMP
            // 
            this.miCopyAsBMP.Enabled = false;
            this.miCopyAsBMP.Index = 0;
            this.miCopyAsBMP.Text = "Copy as BMP";
            // 
            // miUndo
            // 
            this.miUndo.Enabled = false;
            this.miUndo.Index = 1;
            this.miUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.miUndo.Text = "Undo";
            this.miUndo.Click += new System.EventHandler(this.miUndo_Click);
            // 
            // miRedo
            // 
            this.miRedo.Enabled = false;
            this.miRedo.Index = 2;
            this.miRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.miRedo.Text = "Redo";
            this.miRedo.Click += new System.EventHandler(this.miRedo_Click);
            // 
            // miDelete
            // 
            this.miDelete.Enabled = false;
            this.miDelete.Index = 3;
            this.miDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.miDelete.Text = "Delete";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // miCopy
            // 
            this.miCopy.Enabled = false;
            this.miCopy.Index = 4;
            this.miCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.miCopy.Text = "&Copy";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miCut
            // 
            this.miCut.Enabled = false;
            this.miCut.Index = 5;
            this.miCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.miCut.Text = "Cu&t";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miPaste
            // 
            this.miPaste.Enabled = false;
            this.miPaste.Index = 6;
            this.miPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.miPaste.Text = "Paste";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // miView
            // 
            this.miView.Index = 2;
            this.miView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miEntitiesTree,
            this.miSep2,
            this.miZoomIn,
            this.miZoomOut,
            this.miScale,
            this.miFit,
            this.miSep3,
            this.miSHXFontsPaths,
            this.miOptions});
            this.miView.Text = "&View";
            // 
            // miEntitiesTree
            // 
            this.miEntitiesTree.Checked = true;
            this.miEntitiesTree.Index = 0;
            this.miEntitiesTree.Text = "Show entities panel";
            this.miEntitiesTree.Click += new System.EventHandler(this.miEntitiesTree_Click);
            // 
            // miSep2
            // 
            this.miSep2.Index = 1;
            this.miSep2.Text = "-";
            // 
            // miZoomIn
            // 
            this.miZoomIn.Enabled = false;
            this.miZoomIn.Index = 2;
            this.miZoomIn.Text = "Zoom In (+)";
            this.miZoomIn.Click += new System.EventHandler(this.miZoomIn_Click);
            // 
            // miZoomOut
            // 
            this.miZoomOut.Enabled = false;
            this.miZoomOut.Index = 3;
            this.miZoomOut.Text = "Zoom Out (-)";
            this.miZoomOut.Click += new System.EventHandler(this.miZoomOut_Click);
            // 
            // miScale
            // 
            this.miScale.Enabled = false;
            this.miScale.Index = 4;
            this.miScale.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miZoom10,
            this.miZoom25,
            this.miZoom50,
            this.miZoom100,
            this.miZoom200,
            this.miZoom400,
            this.miZoom800});
            this.miScale.Text = "Scale";
            // 
            // miZoom10
            // 
            this.miZoom10.Index = 0;
            this.miZoom10.Text = "10%";
            this.miZoom10.Click += new System.EventHandler(this.miZoom_Click);
            // 
            // miZoom25
            // 
            this.miZoom25.Index = 1;
            this.miZoom25.Text = "25%";
            this.miZoom25.Click += new System.EventHandler(this.miZoom_Click);
            // 
            // miZoom50
            // 
            this.miZoom50.Index = 2;
            this.miZoom50.Text = "50%";
            this.miZoom50.Click += new System.EventHandler(this.miZoom_Click);
            // 
            // miZoom100
            // 
            this.miZoom100.Index = 3;
            this.miZoom100.Text = "100%";
            this.miZoom100.Click += new System.EventHandler(this.miZoom_Click);
            // 
            // miZoom200
            // 
            this.miZoom200.Index = 4;
            this.miZoom200.Text = "200%";
            this.miZoom200.Click += new System.EventHandler(this.miZoom_Click);
            // 
            // miZoom400
            // 
            this.miZoom400.Index = 5;
            this.miZoom400.Text = "400%";
            this.miZoom400.Click += new System.EventHandler(this.miZoom_Click);
            // 
            // miZoom800
            // 
            this.miZoom800.Index = 6;
            this.miZoom800.Text = "800%";
            this.miZoom800.Click += new System.EventHandler(this.miZoom_Click);
            // 
            // miFit
            // 
            this.miFit.Enabled = false;
            this.miFit.Index = 5;
            this.miFit.Text = "Fit drawing to window";
            this.miFit.Click += new System.EventHandler(this.miFit_Click);
            // 
            // miSep3
            // 
            this.miSep3.Index = 6;
            this.miSep3.Text = "-";
            // 
            // miSHXFontsPaths
            // 
            this.miSHXFontsPaths.Index = 7;
            this.miSHXFontsPaths.Text = "SHX Fonts";
            // 
            // miOptions
            // 
            this.miOptions.Index = 8;
            this.miOptions.Text = "Options";
            this.miOptions.Click += new System.EventHandler(this.miOptions_Click);
            // 
            // miCADFiles
            // 
            this.miCADFiles.Index = 3;
            this.miCADFiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miColorDraw,
            this.miBlackDraw,
            this.miWhiteBack,
            this.miBlackBack,
            this.miSep4,
            this.miShowLineWeight,
            this.miDimShow,
            this.miTextsShow,
            this.miSep5,
            this.miLayersShow});
            this.miCADFiles.Text = "&CAD files";
            // 
            // miColorDraw
            // 
            this.miColorDraw.Checked = true;
            this.miColorDraw.Enabled = false;
            this.miColorDraw.Index = 0;
            this.miColorDraw.RadioCheck = true;
            this.miColorDraw.Text = "Color drawing";
            this.miColorDraw.Click += new System.EventHandler(this.miColorDraw_Click);
            // 
            // miBlackDraw
            // 
            this.miBlackDraw.Enabled = false;
            this.miBlackDraw.Index = 1;
            this.miBlackDraw.RadioCheck = true;
            this.miBlackDraw.Text = "Black drawing";
            this.miBlackDraw.Click += new System.EventHandler(this.miBlackDraw_Click);
            // 
            // miWhiteBack
            // 
            this.miWhiteBack.Enabled = false;
            this.miWhiteBack.Index = 2;
            this.miWhiteBack.RadioCheck = true;
            this.miWhiteBack.Text = "White Background";
            this.miWhiteBack.Click += new System.EventHandler(this.miWhiteBack_Click);
            // 
            // miBlackBack
            // 
            this.miBlackBack.Enabled = false;
            this.miBlackBack.Index = 3;
            this.miBlackBack.RadioCheck = true;
            this.miBlackBack.Text = "Black Background";
            this.miBlackBack.Click += new System.EventHandler(this.miBlackBack_Click);
            // 
            // miSep4
            // 
            this.miSep4.Enabled = false;
            this.miSep4.Index = 4;
            this.miSep4.Text = "-";
            // 
            // miShowLineWeight
            // 
            this.miShowLineWeight.Checked = true;
            this.miShowLineWeight.Enabled = false;
            this.miShowLineWeight.Index = 5;
            this.miShowLineWeight.Text = "Show Lineweight";
            this.miShowLineWeight.Click += new System.EventHandler(this.miShowLineWeight_Click);
            // 
            // miDimShow
            // 
            this.miDimShow.Checked = true;
            this.miDimShow.Enabled = false;
            this.miDimShow.Index = 6;
            this.miDimShow.Text = "Dimensions Show";
            this.miDimShow.Click += new System.EventHandler(this.miDimShow_Click);
            // 
            // miTextsShow
            // 
            this.miTextsShow.Checked = true;
            this.miTextsShow.Enabled = false;
            this.miTextsShow.Index = 7;
            this.miTextsShow.Text = "Texts Show";
            this.miTextsShow.Click += new System.EventHandler(this.miTextsShow_Click);
            // 
            // miSep5
            // 
            this.miSep5.Enabled = false;
            this.miSep5.Index = 8;
            this.miSep5.Text = "-";
            // 
            // miLayersShow
            // 
            this.miLayersShow.Enabled = false;
            this.miLayersShow.Index = 9;
            this.miLayersShow.Text = "Show Layers";
            this.miLayersShow.Click += new System.EventHandler(this.miLayersShow_Click);
            // 
            // miReg
            // 
            this.miReg.Index = 4;
            this.miReg.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miLng,
            this.miAbout});
            this.miReg.Text = "?";
            // 
            // miLng
            // 
            this.miLng.Index = 0;
            this.miLng.Text = "Language";
            // 
            // miAbout
            // 
            this.miAbout.Index = 1;
            this.miAbout.Text = "About...";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // saveDXFDlg
            // 
            this.saveDXFDlg.DefaultExt = "*.dxf";
            this.saveDXFDlg.Filter = "*.dxf|*.dxf";
            // 
            // spltPictBox
            // 
            this.spltPictBox.Location = new System.Drawing.Point(0, 0);
            this.spltPictBox.Name = "spltPictBox";
            this.spltPictBox.Size = new System.Drawing.Size(3, 647);
            this.spltPictBox.TabIndex = 15;
            this.spltPictBox.TabStop = false;
            // 
            // printPrevDlg
            // 
            this.printPrevDlg.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPrevDlg.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPrevDlg.ClientSize = new System.Drawing.Size(400, 300);
            this.printPrevDlg.Enabled = true;
            this.printPrevDlg.Icon = ((System.Drawing.Icon)(resources.GetObject("printPrevDlg.Icon")));
            this.printPrevDlg.Name = "printPrevDlg";
            this.printPrevDlg.Visible = false;
            // 
            // openFileDlg
            // 
            this.openFileDlg.DefaultExt = "*.dxf;*.dwg;*.dwt";
            this.openFileDlg.Filter = resources.GetString("openFileDlg.Filter");
            this.openFileDlg.ShowHelp = true;
            // 
            // dlgSaveSerializeFile
            // 
            this.dlgSaveSerializeFile.Filter = "*.dat|*.dat";
            // 
            // dlgOpenSerializeFile
            // 
            this.dlgOpenSerializeFile.Filter = "*.dat|*.dat";
            // 
            // cadPictBox
            // 
            this.cadPictBox.BackColor = System.Drawing.Color.Black;
            this.cadPictBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cadPictBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.cadPictBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cadPictBox.DoubleBuffering = true;
            this.cadPictBox.Location = new System.Drawing.Point(0, 0);
            this.cadPictBox.Name = "cadPictBox";
            this.cadPictBox.Ortho = false;
            this.cadPictBox.Position = new System.Drawing.Point(0, 0);
            this.cadPictBox.ScrollBars = CADImport.FaceModule.ScrollBarsShow.None;
            this.cadPictBox.Size = new System.Drawing.Size(1218, 647);
            this.cadPictBox.TabIndex = 13;
            this.cadPictBox.TabStop = false;
            this.cadPictBox.VirtualSize = new System.Drawing.Size(0, 0);
            this.cadPictBox.Paint += new System.Windows.Forms.PaintEventHandler(this.cadPictBox_Paint);
            this.cadPictBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cadPictBox_MouseDown);
            this.cadPictBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cadPictBox_MouseMove);
            this.cadPictBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cadPictBox_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1218, 663);
            this.Controls.Add(this.tlbTool);
            this.Controls.Add(this.spltPictBox);
            this.Controls.Add(this.cadPictBox);
            this.Controls.Add(this.stBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.sbOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbCoord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private void MainForm_Shown(object sender, EventArgs e)
        {
#if DemoInfo
            using (DemoForm form = new DemoForm())
                form.ShowDialog();
#endif
        }
		#endregion

		static MainForm()
		{					
			fileSettingsName = Application.StartupPath + @"\Settings.txt";			
		}

		[STAThread]
		static void Main(string[] args) 
		{
			bugReport = new CADImport.CADImportForms.BugReportForm();
			Application.EnableVisualStyles();
			Application.DoEvents();						
			if(args.Length != 0)
				if(File.Exists(args[0]))
					fileName = args[0];
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(DoExceptionWorking);
			Application.Run(new MainForm());	
		}

		private static void DoExceptionWorking(object sender, System.Threading.ThreadExceptionEventArgs args)
		{				
			bugReport.ShowErrorDialog(args.Exception);							
		}

		private string RealScale
		{
			get
			{
				if(cadImage != null)
					return string.Format("{0,2:F}%", (((this.VisibleAreaSize.Width * ImageScale) / this.cadImage.AbsWidth) * cadImage.MMToPixelX) * 100.0);
				else 
					return string.Format("{0}%", ImageScale);
			}
		}

		private double RealScaleDouble
		{
			get
			{
				if(cadImage != null)
					return ((this.VisibleAreaSize.Width * ImageScale) / this.cadImage.AbsWidth) * cadImage.MMToPixelX;
				else 
					return ImageScale;
			}
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Изменяет масштаб отображаемого чертежа
		/// </summary>
		/// <param name="i">Коеффициент на который изменяется масштаб чертежа</param>
		#else
		/// <summary>
		/// Adjusts the image size according to the specified scale.
		/// </summary>
		/// <param name="i">A scale value.</param>
		/// <remarks>If the scale value is more than one the image size increases; otherwise, decreases.</remarks>
		#endif
		#endregion Help
		public void Zoom(float i)
		{
			if(cadImage == null)
				return;
			ImageScale = ImageScale * i;
			if(ImageScale < 0.005f)
				ImageScale = 0.005f;	
			cadPictBox.Invalidate();						
			this.stopSnap = true;
			this.entCreator.Disable();
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Сдвигает чертеж при изменении масштаба, на позицию соответстующую старой  позиции для новых размеров
		/// </summary>
		#else
		#endif
		#endregion
		private void Shift()
		{
			LeftImagePosition = PreviousPosition.X - (PreviousPosition.X - LeftImagePosition) * ImageScale / ImagePreviousScale;
			TopImagePosition = PreviousPosition.Y - (PreviousPosition.Y - TopImagePosition) * ImageScale / ImagePreviousScale;
			ImagePreviousScale = ImageScale;		
		}

		private void InitParams()
		{
			ImageScale = 1.0f;
			ImagePreviousScale = 1.0f;
			colorDraw = true;
			dimVisible = true;
			showLineWeight = true;
			textVisible = true;
			cntrlDown = false;
			startPoint = Point.Empty;
			endPoint = Point.Empty;
			PreviousPosition = PointF.Empty;
		}

		private void InitialCADImportForms()
		{
            gForm = new GridForm();
			lForm = new LayerForm();
            lForm.LayerChanged += LForm_LayerChanged;
			aboutForm = new AboutForm();
			prtForm = new CADImport.Printing.PrintingForm();			
			prtForm.LayerForm = lForm;
			shxFrm = new SHXForm();
			rasterSizeForm = new SetRasterSizeForm();
			#region protect
#if protect
			regFrm = new RegForm();
#endif
			#endregion protect
		}

        private void LForm_LayerChanged(object sender, LayerChangedEventArgs e)
        {
            this.cadImage.ClearSelection();
            this.cadImage.ClearMarkers();
            InitialNewPrintPages();
            cadPictBox.Invalidate();
        }

		private void InitParamsState()
		{
            this.btnLast = this.tlbTool.Buttons.Count - 1;
            this.gForm.GridControl = this.cadPictBox;
			this.clipRectangle = new ClipRect(this.cadPictBox);
			this.clipRectangle.MultySelect = true;
			SetAddEntityMode(CreatorType.Undetected);
			this.entCreator = new EntitiesCreator(this.cadPictBox, Color.White);		
			lngFile = ApplicationConstants.defaultstr;
			curLngInd = 0;
			this.cadPictBox.MouseWheel += new MouseEventHandler(CADPictBoxMouseWeel);
			this.cadPictBox.ScrollEvent += new CADImport.FaceModule.ScrollEventHandlerExt(CADPictBoxScroll);
			this.cadPictBox.KeyDown += new System.Windows.Forms.KeyEventHandler(CADPictBoxKeyDown);				
			this.cadPictBox.KeyUp += new System.Windows.Forms.KeyEventHandler(CADPictBoxKeyUp);				
			addEntity = -1;		
			enableSnap = true;
			this.entCreator.EndEntityDraw += new CADImport.CADImportForms.ChangeOptionsEventHandler(this.EndAddEntity); 			
			this.entCreator.CreateNewCADImageEvent += new CADImport.Professional.CreateNewObjectEventHandler(this.CreateNewImage); 						
			this.entCreator.GetRealPointEvent += new  CADImport.Professional.GetRealPointEvent(this.GetRealPoint); 									
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Создает экземпляр класса главной формы приложения
		/// </summary>
		#else
		/// <summary>
		/// Initializes a new instance of the <see cref="CADImportNetDemos.CADEditorDemo.MainForm">
		/// CADImportNetDemos.CADEditorDemo.MainForm</see> class.
		/// </summary>
		#endif	
		#endregion Help
		public MainForm()
		{		
			InitParams();			
			InitialCADImportForms();
			InitializeComponent();
			InitParamsState();											
			InitLng();
		}	
		private void CADPictBoxKeyUp(object sender, KeyEventArgs e)
		{
			if((e.KeyCode == Keys.ControlKey)||(e.KeyCode == Keys.ShiftKey))
				this.cntrlDown = false;
		}		
					
		private void CADPictBoxKeyDown(object sender, KeyEventArgs e)
		{		
			if(!this.cadPictBox.Focused)
				this.cadPictBox.Focus();	
			int cn = 2;
			if(e.Shift)
				cn = 5;
			switch(e.KeyCode)
			{					
				case Keys.Up:	
					MoveEntity(0, cn, 0, cn);																				
					break;
				case Keys.Down:
					MoveEntity(0, -cn, 0, -cn);
					break;
				case Keys.Left:
					MoveEntity(cn, 0, cn, 0);
					break;
				case Keys.Right:
					MoveEntity(-cn, 0, -cn, 0);
					break;
				case Keys.Escape:
					ClearSelection();
					break;								
				case Keys.Enter:					
					this.entCreator.EndEntity();
					break;			
				case Keys.ControlKey:	
				case Keys.ShiftKey:
					this.cntrlDown = true;
					break;
			}											
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Перемещает выбранные примитивы на указанную позицию
		/// </summary>
		/// <param name="x1">Начальная x позиция</param>
		/// <param name="y1">Начальная y позиция</param>
		/// <param name="x2">Конечная x позиция</param>
		/// <param name="y2">Конечная y позиция</param>
		#else
		/// <summary>
		/// Moves selected entities to the specified position
		/// </summary>
		/// <param name="x1">Start X position</param>
		/// <param name="y1">Start Y position</param>
		/// <param name="x2">End X position</param>
		/// <param name="y2">End Y position</param>
		#endif
		#endregion
		public void MoveEntity(int x1, int y1, int x2, int y2)
		{
            cadImage.ClearSnapTrace();
            DPoint p1 = this.GetRealPoint(x1, y1, !this.cadPictBox.Ortho);
            DPoint p2 = this.GetRealPoint(x1 - x2, y1 - y2, !this.cadPictBox.Ortho);
            cadImage.SetNewPosEntitiesExt(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z, this.cadPictBox.CreateGraphics());
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Отмена выделения и перерисовка примитивов
		/// </summary>
		#else
		/// <summary>
		/// Clears selection and redraws entities
		/// </summary>
		#endif
		#endregion
		public void ClearSelection()
		{
			this.entCreator.Disable();
			this.stopSnap = true;
			if(this.cadImage == null) 
				return;
            this.cadImage.ClearSelection();
			this.cadImage.ClearMarkers();
			this.cadPictBox.Invalidate();			
		}

		private void CADPictBoxMouseWeel(object sender, System.Windows.Forms.MouseEventArgs e)
		{			
			if(e.Delta < 0)	
				Zoom(0.7f);
			else 
				Zoom(1.3f);
			this.Shift();			
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Прокручивает чертеж при возникновении событий прокручивания полос прокрутки
		/// </summary>
		/// <param name="sender">Объект вызвавший событие</param>
		/// <param name="e">Параметры события</param>
		#else
		/// <summary>
		/// Scrolls a drawing on scroll bar event appearance
		/// </summary>
		/// <param name="sender">An object which has invoked the event</param>
		/// <param name="e">Event parameters</param>
		#endif
		#endregion
		public void CADPictBoxScroll(object sender, CADImport.FaceModule.ScrollEventArgsExt e)
		{			
			if((e.NewValue == 0) && (e.OldValue == 0))
				e.NewValue = -5;
            if (e.ScrollOrientation == CADImport.FaceModule.ScrollOrientation.VerticalScroll)			
				this.TopImagePosition -= (e.NewValue - e.OldValue);
            if (e.ScrollOrientation == CADImport.FaceModule.ScrollOrientation.HorizontalScroll)						
				this.LeftImagePosition -= (e.NewValue - e.OldValue);								
			this.cadPictBox.Invalidate();
		}		

		#region Help
		#if RUSHELP
		/// <summary>
		/// Устанавливает по <see cref="MainForm.settingsLst">списку настроек</see> текущие настройки для контролов
		/// </summary>
		#else
		/// <summary>
		/// Sets current properties for controls by <see cref="MainForm.settingsLst">properties list</see>
		/// </summary>
		#endif
		#endregion		
		public void SetSettings()
		{
			if(settingsLst == null) return;
			string tmp;
			int cn = 0;
			//Language path
			string key = ApplicationConstants.languagepath;
			if(settingsLst.ContainsKey(key))
			{
				this.mlng.Path = Convert.ToString(settingsLst[key]);
				this.entCreator.OptionsForm.LngDir = this.mlng.Path;
			}
			//Language
			key = ApplicationConstants.languagestr;
			if(settingsLst.ContainsKey(key))
				lngFile = (string)settingsLst[key];
			mlng.LoadLNG(lngFile);
			this.Text = mlng.SetLanguage(this.Controls, this.Menu, this.Text);
			//Language ID
			key = ApplicationConstants.languageIDstr;
			if(settingsLst.ContainsKey(key))
				this.curLngInd = Convert.ToByte(settingsLst[key]);
			//BackgroundColor
			key = ApplicationConstants.backcolorstr;
			tmp = ApplicationConstants.blackstr;
			if(settingsLst.ContainsKey(key))
				tmp = Convert.ToString(settingsLst[key]);
			if(tmp.ToUpper() == ApplicationConstants.blackstr)
				this.Black_Click();
			else 
				this.White_Click();
			//Show entity panel
			key = ApplicationConstants.showentitystr;
			if(settingsLst.ContainsKey(key))
				tmp = Convert.ToString(settingsLst[key]);
			if(tmp.ToUpper() == ApplicationConstants.truestr) 
			{
				this.spltPictBox.Visible = true;
			}
			else
			{
				this.spltPictBox.Visible = false;
			}
			//Color drawing
			key = ApplicationConstants.colordrawstr;
			if(settingsLst.ContainsKey(key))
				tmp = Convert.ToString(settingsLst[key]);
			if(tmp.ToUpper() == ApplicationConstants.truestr) 
				this.colorDraw = true;
			else this.colorDraw = false;
			//SHXPathCount
			key = ApplicationConstants.shxpathcnstr;
			if(settingsLst.ContainsKey(key))
				cn = Convert.ToInt32(settingsLst[key]);
			//SHXPaths
			for(int i = 0; i < cn; i++)
			{
				key = string.Format("SHXPath_{0}", (i + 1));
				if(settingsLst.ContainsKey(key))
					this.shxFrm.AddPath(Convert.ToString(settingsLst[key]));
			}
			//First start
			key = ApplicationConstants.installstr;
			if(settingsLst.ContainsKey(key))
			{
                if (cadImage.Converter.SHXSettings.SearchSHXPaths)
				{
					this.shxFrm.lstDir.Items.Clear();
					this.shxFrm.lstPath.Clear();
					//ArrayList vPaths = new ArrayList();
                    List<string> vPaths = new List<string>();
					CADConst.FindAutoCADSHXPaths(vPaths);
					for(int i = 0; i < vPaths.Count; i++)
					{
						tmp = (string)vPaths[i];
						this.shxFrm.lstDir.Items.Add(tmp);
						this.shxFrm.lstPath.Add(tmp, string.Empty);
                        cadImage.Converter.SHXSettings.SHXSearchPaths += tmp + ApplicationConstants.sepstr3;
					}
				}
			}
			#region protect
#if protect
			//License
			key = "Type_license";
			if(settingsLst.ContainsKey(key))
			{
				tmp = (string)settingsLst[key];
					Protection.LicenseType = LicenseType.Register;
			}
#endif
			#endregion
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Создает по текущиму положению контролов <see cref="MainForm.settingsLst">список настроек</see>
		/// </summary>
		#else
		/// <summary>
		/// Creates <see cref="MainForm.settingsLst">properties list</see> by current position of the controls
		/// </summary>
		#endif
		#endregion
		public void CreateNewSettingsList()
		{
			string tmp;
			settingsLst = new SortedList();
			//Language path
			settingsLst.Add(ApplicationConstants.languagepath, this.mlng.Path);
			//Language
			settingsLst.Add(ApplicationConstants.languagestr, lngFile);
			//Language ID
			settingsLst.Add(ApplicationConstants.languageIDstr, this.curLngInd);
			//BackgroundColor
			if(cadPictBox.BackColor == Color.Black)
				tmp = ApplicationConstants.blackstr2;
			else tmp = ApplicationConstants.whitestr;
			settingsLst.Add(ApplicationConstants.backcolorstr, tmp);
			//Show entity panel
			//Color drawing
			settingsLst.Add(ApplicationConstants.colordrawstr, this.colorDraw);
			//SHXPathCount
			int cn = this.shxFrm.lstDir.Items.Count;
			settingsLst.Add(ApplicationConstants.shxpathcnstr, this.shxFrm.lstDir.Items.Count);
			//SHXPaths
			for(int i = 0; i < cn; i++)
			{
				settingsLst.Add(string.Format("SHXPath_{0}", (i + 1)), this.shxFrm.lstDir.Items[i]);
			}
			#region protect
#if protect
			//License

#else
				settingsLst.Add("Type license", "register");
#endif
			#endregion
		}
	
		#region Help
		#if RUSHELP
		/// <summary>
		/// Инициализирует класс многоязыковой поддержки
		/// </summary>
		#else
		/// <summary>
		/// Initializes a class of multiple language support
		/// </summary>
		#endif
		#endregion
		public void InitLng()
		{
			mlng = new MultipleLanguage(this.GetType());
			//Save value of NoName elements
			if(this.mainMenu != null)
				mlng.SaveNameMenuItem(this.mainMenu.MenuItems);
			if(this.ContextMenu != null)
				mlng.SaveNameMenuItem(this.ContextMenu.MenuItems);
			mlng.SaveNameNoNameElement(this.Controls);
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Устанавливает текущее состояние контролов
		/// </summary>
		#else
		/// <summary>
		/// Sets a current mode of the controls
		/// </summary>
		#endif
		#endregion
		public void ChangeControlState()
		{
			bool detNullImg = cadImage != null;
			EnableButton(detNullImg);
			bool tmp = false;
			//back color
			if(cadPictBox.BackColor == Color.White)
				tmp = true;
			miWhiteBack.Checked = tmp;
			miBlackBack.Checked = ! tmp;
			if(detNullImg)
			{
				tlbTool.Buttons[3].Pushed = tmp;
				tlbTool.Buttons[4].Pushed = ! tmp;
			}
			else 
			{
				tlbTool.Buttons[3].Pushed = false;
				tlbTool.Buttons[4].Pushed = false;
			}
			//text show
			tmp = this.textVisible;
			miTextsShow.Checked = tmp;
			//dimension visible
			tmp = this.dimVisible;
			miDimShow.Checked = tmp;
			//tree visible
			//color draw
			tmp = this.colorDraw;
			miColorDraw.Checked = tmp;
			miBlackDraw.Checked = ! tmp;
			//use win ellipse
            //if(cadImage != null)
            //    tmp = cadImage.UseWinEllipse;
			//show LineWeight 
			miShowLineWeight.Checked = this.showLineWeight;
			//lng
			if(curLngInd < this.mainMenu.MenuItems[4].MenuItems[0].MenuItems.Count)
				this.mainMenu.MenuItems[4].MenuItems[0].MenuItems[curLngInd].Checked = false;
			if(this.mainMenu.MenuItems[4].MenuItems[0].MenuItems.Count > this.curLngInd)
				this.mainMenu.MenuItems[4].MenuItems[0].MenuItems[this.curLngInd].Checked = true;
			tlbTool.Buttons[btnOffset - 1].Pushed = useSHXFonts;
            tlbTool.Buttons[btnLast - 2].Pushed = this.enableSnap;
            tlbTool.Buttons[btnLast - 1].Pushed = this.cadPictBox.Grid.IsActive;
            tlbTool.Buttons[btnLast].Pushed = this.cadPictBox.Ortho;
		}

		#region Help
		/// <summary>
		/// Cleans up any resources being used.
		/// </summary>
		/// <param name="disposing">A value indicating if both managed and unmanaged resources have to be released (<b>true</b>) or only unmanaged (<b>false</b>). 
		///</param>
		#endregion Help
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void Open_Click(object sender, System.EventArgs e)
		{
			LoadFile(true);	
			ChangeControlState();
		}

		private void InvalidateImage(object sender, System.EventArgs e)
		{
			this.cadPictBox.Invalidate();
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Задает новое положение для  <see cref="CADImport.CADEntity"/>Entity при перемещении 
		/// исходя из <see cref="MainForm.StartPoint">начального</see> и 
		/// <see cref="MainForm.EndPoint">конечного</see> положений
		/// </summary>
		#else
		/// <summary>
		/// Sets new position for <see cref="CADImport.CADEntity"/>Entity on offsetting
		/// using <see cref="StartPoint">start</see> and 
		/// <see cref="EndPoint">end</see> positions
		/// </summary>
		#endif
		#endregion
		public void SetNewEntityPosition()
		{
			if((this.startPoint.X != 0)||(this.startPoint.Y != 0))
			{
                if (this.endPoint == Point.Empty)
                    this.endPoint = new Point(cX, cY);
                DPoint p = this.GetRealPoint(this.endPoint.X - this.startPoint.X, this.endPoint.Y - this.startPoint.Y, !this.cadPictBox.Ortho, false);
				cadImage.SetNewPosEntitiesExt(p.X, p.Y, p.Z, this.cadPictBox.CreateGraphics());
				this.startPoint.X = 0;
				this.startPoint.Y = 0;
				this.endPoint.X = 0;
				this.endPoint.Y = 0;				
			}
		}
		#region Help
#if RUSHELP
		/// <summary>
		/// Задает новое положение для <see cref="CADImport.CADEntity"/>Entity при 
		/// изменении по <see cref="CADImport.Professional.Marker"/>маркерам 
		/// исходя из <see cref="MainForm.StartPoint">начального</see> и 
		/// <see cref="MainForm.EndPoint">конечного</see> положений 
		/// </summary>
#else
		/// <summary>
		/// Sets new position for <see cref="CADImport.CADEntity"/>Entity 
		/// on editing by <see cref="CADImport.Professional.Marker"/>markers
		/// using <see cref="MainForm.StartPoint">start</see> and 
		/// <see cref="MainForm.EndPoint">end</see> positions
		/// </summary>
#endif
		#endregion
		public void SetNewMarkerPosition()
		{
			if((this.startPoint.X != 0)||(this.startPoint.Y != 0))
			{
                if (this.endPoint == Point.Empty)
                    this.endPoint = new Point(cX, cY);
				DPoint p = this.GetRealPoint(this.endPoint.X - this.startPoint.X, this.endPoint.Y - this.startPoint.Y, !this.cadPictBox.Ortho, false);
				cadImage.SetNewEntityMarkerPos(p.X, p.Y, p.Z);
				this.startPoint.X = 0; 
				this.startPoint.Y = 0;
				this.endPoint.X = 0;
				this.endPoint.Y = 0;
			}
		}
		#region Help
		#if RUSHELP
		/// <summary>		
		/// Очищает текущий объект <see cref="MainForm.Image"/>CADImage
		/// </summary>
		#else
		/// <summary>
		/// Clears current <see cref="Image"/>CADImage object
		/// </summary>
		#endif
		#endregion
		public void ClearCADImage()
		{
			lForm.LayerList.Clear();
			if(cadImage != null)
			{	
				cadImage.Dispose();
				cadImage = null;
			}
			ImageScale = 1.0f;
			ImagePreviousScale = 1.0f;
			Position = new PointF();		
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Устанавливает текущие настройки для <see cref="MainForm.Image"/>CADImage при загрузке нового файла
		/// </summary>
		#else
		/// <summary>
		/// Sets current properties for <see cref="MainForm.Image"/>CADImage for loading file
		/// </summary>
		#endif
		#endregion
		public void SetCADImageOptions()
		{
			SetSnapMode();
            cadImage.SelectionMode = SelectionEntityMode.Enabled;

            cadImage.GraphicsOutMode = DrawGraphicsMode.GDIPlus;
            cadImage.ChangeDrawMode(cadImage.GraphicsOutMode, cadPictBox);
            cadImage.IsWithoutMargins = true;
            
            //cadImage.UseWinEllipse = false;
			if(cadPictBox.BackColor == Color.White)
				White_Click();
			else 
				Black_Click();
			ViewLayouts();
			if(this.colorDraw == false) 
				DoBlackColor();
			SetLayList();
			DoResize();			
			ObjEntity.cadImage = cadImage;			//for object inspector
			this.prtForm.Image = this.cadImage;
			this.entCreator.Image = this.cadImage;	
		}

		#region Help
		/// <summary>
		/// Loads a CAD file into the main application's form.
		/// </summary>
		/// <param name="dlg">A value indicating if an <see cref="System.Windows.Forms.OpenFileDialog">OpenFileDialog</see> 
		/// box is invoked for selecting and loading a CAD file. <b>true</b>, if an <see cref="System.Windows.Forms.OpenFileDialog">OpenFileDialog</see> 
		/// box is invoked; otherwise, <b>false</b>.</param>
		#endregion Help
		public void LoadFile(bool dlg)
		{
			SetPictureBoxLoadState(true, string.Empty);			
			if(dlg)
			{
				DialogResult dlgRes = openFileDlg.ShowDialog(this);
				if((dlgRes != DialogResult.OK)||(string.IsNullOrEmpty(openFileDlg.FileName)))
				{
					SetPictureBoxLoadState(false, string.Empty);
					return;
				}
                fileName = openFileDlg.FileName;
			}
			ClearCADImage();
            this.cadImage = CADImage.CreateImageByExtension(fileName);			
			if(this.cadImage is CADRasterImage)
				(this.cadImage as CADRasterImage).Control = this.cadPictBox;			
			if(this.cadImage == null)
			{
				SetPictureBoxLoadState(false, string.Empty);
				return;
			}					
			CADImage.CodePage = System.Text.Encoding.Default.CodePage;
			CADImage.LastLoadedFilePath = Path.GetDirectoryName(fileName);
            CADConst.DefaultSHXParameters.SHXSearchPaths = shxFrm.SHXPaths;

            if (CADConst.IsWebPath(fileName))
                this.cadImage.LoadFromWeb(fileName);
			else
                cadImage.LoadFromFile(fileName);

			SetCADImageOptions();									
			EnableButton(true);
            SetPictureBoxLoadState(false, fileName);
		}

		private void SetPictureBoxLoadState(bool val, string name)
		{
            this.cadPictBox.Visible = !val;
			if(val)
			{
				this.Cursor = Cursors.WaitCursor;
				stBar.Panels[0].Text = ApplicationConstants.loadfilestr;
			}
			else
			{
				this.Cursor = Cursors.Default;
                stBar.Panels[0].Text = (name != string.Empty) ? name : fileName;
			}
		}

		#region Help
		/// <summary>
		/// Creates a list of layouts of the CAD image and sets a current layout. 
		/// </summary>
		#endregion Help
		public void ViewLayouts()
		{			
			if(cadImage == null) 
				return;
			cadImage.SetCurrentLayout(cadImage.DefaultLayoutIndex);
		}

		#region Help
		/// <summary>
		/// Enables or disables buttons of the toolbar and menu items of the main menu.
		/// </summary>
		/// <param name="aVal"><b>true</b> if to enable the buttons and menu items; <b>false</b> if to disable them.</param>
		/// <remarks>The buttons and menu items are enabled after loading a CAD file and become disabled after closing the file.</remarks>
		#endregion Help
		public void EnableButton(bool aVal)
		{
			for(int i = 1; i < btnOffset; i++)
				tlbTool.Buttons[i].Enabled = aVal;
			for(int i = btnLast - 5; i < btnLast - 2; i++)
				tlbTool.Buttons[i].Enabled = aVal;
			#region Export
#if Export
			this.miSaveAsDXF.Enabled = aVal;
#endif
			#endregion
			miScale.Enabled = aVal;
			miSaveFile.Enabled = aVal;
			miCopyAsBMP.Enabled = aVal;
			miZoomIn.Enabled = aVal;
			miZoomOut.Enabled = aVal;
			miFit.Enabled = aVal;
			miColorDraw.Enabled = aVal;
			miBlackDraw.Enabled = aVal;
			miWhiteBack.Enabled = aVal;
			miBlackBack.Enabled = aVal;
			miShowLineWeight.Enabled = aVal;
			miDimShow.Enabled = aVal;
			miTextsShow.Enabled = aVal;
			miLayersShow.Enabled = aVal;			
			miClose.Enabled = aVal;
			miPrint.Enabled = aVal;
			miPrintCustom.Enabled = aVal;
			miPrintPreview.Enabled = aVal;
			miUndo.Enabled = aVal;
			miRedo.Enabled = aVal;
			miDelete.Enabled = aVal;
			miCopy.Enabled = aVal;
			miPaste.Enabled = aVal;
			miCut.Enabled = aVal;
			miSerializeEntities.Enabled = aVal;			
		}

		private void ZoomIn_Click(object sender, System.EventArgs e)
		{
			DoZoomIn();
		}

		#region Help
		/// <summary>
		/// Increases a scale of the CAD image in two times.
		/// </summary>
		#endregion Help
		public void DoZoomIn()
		{
			if(cadImage == null) 
				return;
			PreviousPosition = new PointF(cadPictBox.ClientRectangle.Width / 2.0f, cadPictBox.ClientRectangle.Height / 2.0f);
			ImageScale = ImageScale * 2.0f;
			cadPictBox.Invalidate();						
			this.stopSnap = false;	
		}

		private void ZoomOut_Click(object sender, System.EventArgs e)
		{
			DoZoomOut();
		}

		#region Help
		/// <summary>
		/// Decreases a scale of the CAD image in two times.
		/// </summary>
		#endregion Help
		public void DoZoomOut()
		{
			if(cadImage == null) return;
			PreviousPosition = new PointF(cadPictBox.ClientRectangle.Width / 2.0f, cadPictBox.ClientRectangle.Height / 2.0f);
			ImageScale = ImageScale / 2.0f;
			cadPictBox.Invalidate();						
			this.stopSnap = false;
		}

		private void cadPictBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			DrawCADImage(e.Graphics, sender as Control);
			stBar.Panels[1].Text = RealScale;							
		}

		#region Help
		/// <summary>
		/// Draws a CAD image from the loaded CAD file.
		/// </summary>
		/// <param name="gr">A <see cref="System.Drawing.Graphics">Graphics</see> object used to draw the CAD entities.</param>
		#endregion Help
		public void DrawCADImage(Graphics gr, Control render)
		{
			if(cadImage == null) return;							
			Shift();
			RectangleF tmp = ImageRectangleF;								
			SetSizePictureBox(new Size((int)tmp.Width, (int)tmp.Height));				
			SetPictureBoxPosition(Position);	
			try
			{
				cadImage.Draw(gr, tmp, render);								
			}
			catch
			{
				return;				
			}
		}
		#region Help
#if RUSHELP
		/// <summary>
		/// Установливает размер области прокрутки для <see cref="CADImport.FaceModule.CADPictureBox">CADPictureBox</see>
		/// </summary>
		/// <param name="sz">Устанавливаемый размер</param>
#else
		/// <summary>
		/// Sets scroll region for <see cref="CADImport.FaceModule.CADPictureBox">CADPictureBox</see>
		/// </summary>
		/// <param name="sz">Size to set</param>
#endif
		#endregion
		public void SetSizePictureBox(Size sz)
		{			
			if((((pos.X < 0) || (pos.Y < 0))||
				((pos.X + sz.Width > this.cadPictBox.Width)||
				(pos.Y + sz.Height > this.cadPictBox.Height)))&&(SizeEqualSizeImageAndPictBox()))
			{
				if(pos.X < 0)
					sz.Width = (int)(this.cadPictBox.Width - pos.X);
				if(pos.Y < 0)
					sz.Height = (int)(this.cadPictBox.Height - pos.Y);
				if(pos.X + sz.Width > this.cadPictBox.Width)
					sz.Width = (int)(this.cadPictBox.Width + pos.X);
				if(pos.Y + sz.Height > this.cadPictBox.Height)
					sz.Height = (int)(this.cadPictBox.Height + pos.Y);				
			}			
			this.cadPictBox.SetVirtualSizeNoInvalidate(sz);			
		}

		private bool SizeEqualSizeImageAndPictBox()
		{
			RectangleF tmp = ImageRectangleF;
			return ((tmp.Width <= this.cadPictBox.Size.Width)&&(tmp.Height <= this.cadPictBox.Height));
		}

		private void SetPictureBoxPosition(PointF value)
		{
			int w1, h1;
			if(value.X > 0)
				w1 = 0;
			else
				w1 = (int)Math.Abs(value.X);
			if(w1 > this.cadPictBox.VirtualSize.Width)
				w1 = this.cadPictBox.VirtualSize.Width;
			if(value.Y > 0)
				h1 = 0;
			else
				h1 = (int)Math.Abs(value.Y);
			if(h1 > this.cadPictBox.VirtualSize.Height)
				h1 = this.cadPictBox.VirtualSize.Height;
			this.cadPictBox.SetPositionNoInvalidate(new Point(w1, h1));			
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Проверяет целесообразность смещения примитива
		/// (в случае если смещение минимально возвращает <b>true</b>)
		/// </summary>
		/// <param name="x">Позиция по оси X</param>
		/// <param name="y">Позиция по оси X</param>
		/// <returns>В случае если смещение минимально возвращает <b>true</b></returns>
		#else	
		/// <summary>
		/// Verifies suitability of the entity moving
		/// </summary>
		/// <param name="x">X axis position</param>
		/// <param name="y">Y  axis position</param>
		/// <returns>Returns <b>true</b> if the offset is minimal</returns>
		#endif
		#endregion
		public bool ChancelMove(int x, int y)
		{
			if(((Math.Abs(cX - x) < 2)&&(Math.Abs(cY - y) < 2))||
                (this.cadImage.SelectEntitiesCount == 0))					
			{				
				return true;
			}
			return false;
		}
		
		private bool ChangeEntityOnMouseUp(Point pt)
		{
			#region Professional
#if Professional
			if(cadImage.CurrentMarker != null)
			{
				if(! ChancelMove(pt.X, pt.Y))
				{
					SetNewMarkerPosition();				
					return true;
				}
			}
			else
			{				
				if(! CreateNewEntity)
				{
					if(! ChancelMove(pt.X, pt.Y))
					{
						SetNewEntityPosition();				
						return true;
					}
				}
			}			
			return false;
#else
			return false;
#endif

			#endregion
		}

		private void cadPictBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{						
			if(cadImage == null) return;
            if (e.Button != MouseButtons.Right)
                if (ChangeEntityOnMouseUp(new Point(e.X, e.Y)))
                    cadPictBox.Invalidate();
			detMouseDown = false;			
			if(this.clipRectangle.Enabled)
				if((this.clipRectangle.Type == RectangleType.Zooming))
					UseClipRect();									
			if(selectedEntitiesChanged && !CreateNewEntity)
			{				
				this.stopSnap = true;
				cadPictBox.Invalidate();									
			}
            if (cadImage != null)
                cadImage.SelectedEntities.PropertyChanged -= new PropertyChangedEventHandler(this.SelectedEntitiesChanged);
            selectedEntitiesChanged = false;

		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Включает инструмент <see cref="CADImport.FaceModule"/>ClipRect
		/// для выбора <see cref="CADImport.CADEntityCollection">набора примитивов</see>
		/// </summary>
		#else
		/// <summary>
		/// Enables <see cref="CADImport.FaceModule"/>ClipRect tool
		/// for <see cref="CADImport.CADEntityCollection">set of entities</see> selection
		/// </summary>
		#endif
		#endregion
		public void UseClipRect()
		{
			if((this.clipRectangle.ClientRectangle.Width <= 10)||
				(this.clipRectangle.ClientRectangle.Height <= 10)) 
				return; 					
            MultipleSelect();
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Выделяет примитив по указанным координатам, а также устанавливает
		/// ряд настроек при выделении примитива
		/// </summary>
		/// <param name="x">Позиция по оси X</param>
		/// <param name="y">Позиция по осе Y</param>
		/// <returns><b>true</b> в случае если по указанным координатам есть примитив и он выделен</returns>	
		#else
		/// <summary>
		/// Selects an entity by the specified coordinates.
		/// Sets a number of settings on entity selection.
		/// </summary>
		/// <param name="x">X axis position</param>
		/// <param name="y">Y axis position</param>
		/// <returns>Returns <b>true</b> if there is an entity in specified position and it is selected</returns>
		#endif
		#endregion
		public bool SelectEntity(int x, int y)
		{		
			if(this.CreateNewEntity) 
				return false;
			moveMarker = false;
			if(cadImage == null) 
				return false;
            bool det = this.cadImage.SelectedEntities.Count == 0;
			CADEntity ent = cadImage.SelectExt(x, y, cntrlDown, true);
			if(ent != null) 
			{				
				moveMarker = true;		
				this.stopSnap = true;				
			}			
			else
			{
				if(! det)
				{	
					this.stopSnap = true;					
				}
			}			
			return false;
		}
		
		private void cadPictBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            selectedEntitiesChanged = false;
            if (cadImage != null)
                cadImage.SelectedEntities.PropertyChanged += new PropertyChangedEventHandler(this.SelectedEntitiesChanged);
			if(e.Button != MouseButtons.Right)			
			{
				if(! CreateNewEntity)
					DisableEntityCreator();				
				if(this.cadImage != null)
					if(cadImage.SelectionMode == SelectionEntityMode.Enabled)
					{							
						if(SelectEntity(e.X, e.Y))
							return;										
					}	
			}	
			cX = e.X;
			cY = e.Y;			
			detMouseDown = true;		
			if(CreateNewEntity)
				if(EnableNewEntityCreator())
				{		
					return;
				}
			if((e.Button == MouseButtons.Left)&&(! moveMarker))
				this.clipRectangle.EnableRect(RectangleType.Zooming, new Rectangle(e.X, e.Y, 0, 0));			
		}

		private void DisableEntityCreator()
		{
			if(this.entCreator.Enabled)
				if((this.entCreator.Type == CreatorType.Pen)||((this.entCreator.Type == CreatorType.Polyline) 
					&& this.entCreator.EndPoly)||(this.entCreator.Type == CreatorType.Point))
					this.entCreator.Disable();			
		}

		private bool EnableNewEntityCreator()
		{
            CreatorType seltp = (CreatorType)addEntity;
			if(seltp != CreatorType.Undetected)	
			{
				this.entCreator.EnableCreator(seltp);											
				curAddEntityType = seltp;							
				return true;
			}
			return false;
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Завершает добавление <see cref="CADImport.CADEntity"/>примитива
		/// </summary>
		/// <param name="val">Флаг завершения добавления <see cref="CADImport.CADEntity"/>примитива</param>
		#else
		/// <summary>
		/// Finishes adding of the <see cref="CADImport.CADEntity"/>entity
		/// </summary>
		/// <param name="val">A finishing flag of the <see cref="CADImport.CADEntity"/>entity adding</param>
		#endif
		#endregion
		public void EndAddEntity(bool val)
		{
			if(curAddEntityType != CreatorType.Undetected)
			{
				SetAddEntityMode(curAddEntityType);
				SetEnableChecked();				
				ChangeControlState();
			}
			this.detMouseDown = val;
		}	
		#region Help
		#if RUSHELP
		/// <summary>
		/// Изменяет позицию <see cref="CADImport.CADEntity"/>примитива по изменению положения 
		/// <see cref="CADImport.Professional.Marker"/>маркера или
		/// при перетаскивании <see cref="CADImport.CADEntity"/>примитива 
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		#else
		/// <summary>
		/// Changes <see cref="CADImport.CADEntity"/>entity position by
		/// changing <see cref="CADImport.CADEntity"/>marker position or
		/// <see cref="CADImport.CADEntity"/>entity dragging 
		/// </summary>
		/// <param name="pt">New coordinates</param>
		/// <returns>Returns <b>true</b> if the changing is successful</returns>
		#endif
		#endregion
		public bool ChangeEntityOnMouseMove(Point pt)
		{
			#region Professional
#if Professional
			if(this.cadImage.SelectedEntities.Count != 0)
			{		
				if(this.CreateNewEntity) return true;

                this.cadImage.CorrectByGridAndOrtho(ref pt, cX, cY, this.cadPictBox.Ortho);

				if(cadImage.CurrentMarker != null)
				{									
					ChangeMarkerPosition(pt.X, pt.Y);
					return true;
				}
				else if (moveMarker)
				{						
					ChangeEntityPositionExt(pt.X, pt.Y);
					return true;					
				}				
			}			
#endif		
			#endregion
			return false;
		}

        private void SelectedEntitiesChanged(object sender, PropertyChangedEventArgs args)
        {
            selectedEntitiesChanged = true;
        }

		private void cadPictBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{			
			if(cadImage == null) 
				return;	
			if(!this.cadPictBox.Focused)
				this.cadPictBox.Focus();
			if((!stopSnap) && (!this.clipRectangle.Enabled) && (this.cadPictBox.ClientRectangle.Contains(e.X, e.Y)))			
			{			
				RectangleF tmpRect = ImageRectangleF;
				cadImage.DrawSnapTrace(e.X, e.Y, this.cadPictBox, tmpRect);		
			}
			else
			{
				this.cadImage.RefreshSnapTrace(this.cadPictBox);
				this.stopSnap = false;
			}
            PreviousPosition = new PointF(e.X, e.Y);
            DPoint vPt = GetRealPoint(e.X, e.Y, true);
            stBar.Panels[2].Text = string.Format("{0} : {1} : {2}", vPt.X, vPt.Y, vPt.Z);
			if(detMouseDown)
			{
				if(e.Button != MouseButtons.Right)
					if(ChangeEntityOnMouseMove(new Point(e.X, e.Y)))										
						return;					
				if((!this.clipRectangle.Enabled) && (!this.CreateNewEntity))
				{					
					LeftImagePosition -= (cX - e.X);
					TopImagePosition -= (cY - e.Y);					
					cadPictBox.Invalidate();					
				}
				cX = e.X;
				cY = e.Y;				
			}	
		}	

		private void ChangeEntityPositionExt(int x, int y)
		{
			Graphics gr = this.cadPictBox.CreateGraphics();	
			this.cadImage.DrawMoveEntityTrace(-this.startPoint.X, -this.startPoint.Y, this.cadPictBox);
            this.cadImage.DrawMoveEntityTrace(x - cX, y - cY, this.cadPictBox);
            this.startPoint.X = cX - x;
            this.startPoint.Y = cY - y;			
		}
		#region Professional
#if Professional
		private void ChangeMarkerPosition(int x, int y)
		{
			Graphics gr = this.cadPictBox.CreateGraphics();
			//repaint old
			if((this.startPoint.X != 0)||(this.startPoint.Y != 0))
			{
				this.cadImage.DrawChangeEntity(this.startPoint.X, this.startPoint.Y, this.cadPictBox);  				
			}
			else
			{
				endPoint.X = x;
				endPoint.Y = y;
			}
			//paint new
			this.cadImage.DrawChangeEntity((cX - x), (cY - y), this.cadPictBox);  
			this.startPoint.X = (cX - x);
			this.startPoint.Y = (cY - y);
		}
#endif
		#endregion

		private bool MultipleSelect()
		{
            if (cadImage.SelectionMode == SelectionEntityMode.Enabled)
			{
				int l = this.clipRectangle.ClientRectangle.Left;
				int t = this.clipRectangle.ClientRectangle.Top;				
				DPoint pt1 = this.GetRealPoint(l, t, false);
				DPoint pt2 = this.GetRealPoint(this.clipRectangle.ClientRectangle.Right, this.clipRectangle.ClientRectangle.Bottom, false);
				DRect tmpRect = new DRect(pt1.X, pt1.Y, pt2.X, pt2.Y);				
				#region Professional
#if Professional
				cadImage.MultipleSelectExt(tmpRect, cntrlDown, true);				
#else
				cadImage.MultipleSelect(tmpRect, true,true);
#endif
				#endregion				
				return true;
			}
			return false;
		}

		#region Help
		/// <summary>
		/// Gets a three-dimensional point of the current CAD image from the specified screen point.
		/// </summary>
		/// <param name="x">An X coordinate of the screen point.</param>
		/// <param name="y">A Y coordinate of the screen point.</param>
        /// <param name="useGrid">Defines if the point will be reduced to a grid node.</param>
		/// <returns>A three-dimensional point.</returns>
		#endregion Help
		public DPoint GetRealPoint(int x, int y, bool useGrid)
		{
            return GetRealPoint(x, y, useGrid, true);
		}

        #region Help
        /// <summary>
        /// Gets a three-dimensional point of the current CAD image from the specified screen point.
        /// </summary>
        /// <param name="x">An X coordinate of the screen point.</param>
        /// <param name="y">A Y coordinate of the screen point.</param>
        /// <param name="useGrid">Defines if the point will be reduced to a grid node.</param>
        /// <param name="rounding">Defines if rounding will be applied.</param>
        /// <returns>A three-dimensional point.</returns>
        #endregion Help
        public DPoint GetRealPoint(int x, int y, bool useGrid, bool rounding)
        {
            RectangleF tmpRect = ImageRectangleF;
            DPoint realPt = CADConst.GetRealPoint(x, y, this.cadImage, tmpRect, rounding);
            if (useGrid)
                this.cadImage.CorrectByGrid(ref realPt);
            return realPt;
        }

		private void MoveToPosition(ref DPoint point, DPoint aPos, int direction)
		{  
			point.X = point.X + direction * aPos.X;
			point.Y = point.Y + direction * aPos.Y;
			point.Z = point.Z + direction * aPos.Z;
		}

		private void miZoom_Click(object sender, System.EventArgs e)
		{
			if(cadImage == null) 
				return;
			float i = ImageScale;
			switch((sender as MenuItem).Index)
			{
				case 0:
					i = 0.1f;
					break;
				case 1:
					i = 0.25f;
					break;
				case 2:
					i = 0.5f;
					break;
				case 3:
					i = 1.0f;
					break;
				case 4:
					i = 2.0f;
					break;
				case 5:
					i = 4.0f;
					break;
				case 6:
					i = 8.0f;
					break;
			}
			ResetScaling();
			this.PreviousPosition = new PointF(cadPictBox.ClientRectangle.Width / 2.0f, cadPictBox.ClientRectangle.Height / 2.0f);
			ImageScale = i;
			cadPictBox.Invalidate();			
		}

		#region Help
		/// <summary>
		/// Paints the CAD image background in the white color.
		/// </summary>
		#endregion Help
		public void White_Click()
		{
			cadPictBox.BackColor = Color.White;
			if(this.cadImage != null)
				this.cadImage.BackgroundColor = Color.White;
			if(cadImage != null)
                cadImage.DefaultColor = Color.Black;
			if(this.clipRectangle != null)
				this.clipRectangle.Color = Color.Black;
		}

		#region Help
		/// <summary>
		/// Paints the CAD image background in the black color.
		/// </summary>
		#endregion Help
		public void Black_Click()
		{
			cadPictBox.BackColor = Color.Black;
			if(this.cadImage != null)
				this.cadImage.BackgroundColor = Color.Black;		
			if(cadImage != null)
                cadImage.DefaultColor = Color.White;
			if(this.clipRectangle != null)
				this.clipRectangle.Color = Color.White;
		}

		private void tlbTool_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
            int btnIndex = tlbTool.Buttons.IndexOf(e.Button);
            switch (btnIndex)
			{
				case 0:
					LoadFile(true);
					break; 
				case 1:
					DoZoomIn();
					break; 
				case 2:
					DoZoomOut();
					break;
				case 3:
					White_Click();
					break;
				case 4:
					Black_Click();
					break;
				case 5:
					SetLayList();
					lForm.ShowDialog();
					break;
				case 6:
					break;
				case 7:
					if(MessageBox.Show(ApplicationConstants.messagestr, ApplicationConstants.appnamestr, MessageBoxButtons.YesNo) == DialogResult.Yes)
						ChangeTextsType(! useSHXFonts);
					break;
				case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                    SetAddEntityMode((CreatorType)(btnIndex - btnOffset));
					break;
				case 22:
					this.entCreator.OptionsForm.ShowDialog();
					break;
				case 23:
					CreateNewImage();
					break;
				case 24:
					GetExtents();
					this.DoResize();
					break;
				case 25:
					RemoveEntity();
					break;
				case 26:
					UndoChangeEntity();
					break;
				case 27:
					RedoChangeEntity();
					break;
				case 28:
					this.enableSnap = ! this.enableSnap;
					SetSnapMode();
					break;
                case 29:
                    gForm.ShowDialog();
                    break;
                case 30:
                    this.cadPictBox.Ortho = !this.cadPictBox.Ortho;
                    break;
			}
			SetEnableChecked();			
			ChangeControlState();					
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Устанавливает режим привязки(снап) 
		/// </summary>
		#else
		/// <summary>
		/// Sets snap mode
		/// </summary>
		#endif
		#endregion
		public void SetSnapMode()
		{			
			if(this.cadImage != null)
				this.cadImage.EnableSnap = this.enableSnap;
		}

		private void UndoChangeEntity()
		{
			this.cadImage.Undo();
			this.cadPictBox.Invalidate();			
		}

		private void RedoChangeEntity()
		{
			this.cadImage.Redo();
			this.cadPictBox.Invalidate();
		}

		private void RemoveEntity()
		{
			if(this.cadImage != null)
				this.cadImage.RemoveEntity();
			this.cadPictBox.Invalidate();
		}

		private void SetAddEntityMode(CreatorType val)
		{
			int tmpVal = addEntity;
			addEntity = -1;
            int ct = (int)val;
            if (tmpVal != ct)
                addEntity = ct;				
		}

		#region Help
		#if RUSHELP
		/// <summary>
		/// Получает <b>true</b> если в данный момент выбран режим создания
		/// какого либо из примитивов
		/// </summary>
		#else
		/// <summary>
		/// Gets <b>true</b> if entity creating mode is enabled at the moment
		/// </summary>
		#endif
		#endregion
		public bool CreateNewEntity
		{
			get
			{
                bool isHatch = (CreatorType)addEntity == CreatorType.Hatch;
				if((this.cadImage == null) && isHatch) 
					return false; 
				if(this.cadImage != null)
                    if ((this.cadImage.SelectedEntities.Count == 0) && isHatch) 
						return false;
				return (addEntity != -1);
			}
		}

		private void GetExtents()
		{
			if(cadImage == null)
                return;
            ImagePreviousScale = ImageScale;
            ImageScale = 1;
            Shift();
			cadImage.GetExtents();
			//LeftImagePosition = (cadPictBox.ClientRectangle.Width - VisibleAreaSize.Width) / 2.0f;
			//TopImagePosition = (cadPictBox.ClientRectangle.Height - VisibleAreaSize.Height) / 2.0f;
			
			DoResize();
		}
		#region Help
		/// <summary>
		/// Create new <see cref="CADImport.CADImage">CADImage</see> object
		/// </summary>
		#endregion
		public void CreateNewImage()
		{
			if(cadImage != null)
				cadImage.Dispose();

			ImageScale = 1.0f;
			ImagePreviousScale = 1.0f;
			Position = new PointF();
			cadImage = new CADImage();
            this.cadImage.InitialNewImage();
			EnableButton(true);

            if (ObjEntity.LayersList != null)
                ObjEntity.LayersList = null;
            if (lForm.LayerList != null)
                lForm.LayerList.Clear();

            SetCADImageOptions();
		}

		private void SetEnableChecked()
		{
            for (int i = 0; i < 14; i++)    
			    tlbTool.Buttons[btnOffset + i].Pushed = (addEntity == i);		
		}
		
		private void ChangeTextsType(bool val)
		{
			useSHXFonts = val;
			CADConst.DefaultSHXParameters.UseSHXFonts = useSHXFonts;
			CADConst.DefaultSHXParameters.UseTTFFonts = !useSHXFonts;
			CADConst.DefaultSHXParameters.UseMultyTTFFonts = !useSHXFonts;
			this.ReOpen();
		}
	
		#region Help
		/// <summary>
		/// Reopens a file after selecting or deselecting SHX fonts.
		/// </summary>
		/// <remarks>This method is also called when the paths to the 
		/// files containing SHX fonts have been edited in the <b>SHX Paths</b> window 
		/// invoked by the <b>SHX Fonts</b> menu item.</remarks>
		#endregion Help
		public void ReOpen()
		{
			if(openFileDlg.FileName != null)
				if(File.Exists(openFileDlg.FileName))
					this.LoadFile(false);
		}
		#region Help
		/// <summary>
		/// Sets the scale and position of the CAD image to its original values.
		/// </summary>
		#endregion Help
		public void ResetScaling()
		{
			ImageScale = 1.0f;
			ImagePreviousScale = 1.0f;
			LeftImagePosition = (cadPictBox.ClientRectangle.Width - VisibleAreaSize.Width) / 2.0f;
			TopImagePosition = (cadPictBox.ClientRectangle.Height - VisibleAreaSize.Height) / 2.0f;
			cadPictBox.Invalidate();
		}

		#region Help
#if RUSHELP
		/// <summary>
		/// Устанавливает параметр <see cref="CADImport.CADLayer.Visible">CADLayer.Visible</see> для слоя по указанному номеру <see cref="CADImport.CADLayer">слоя</see>
		/// </summary>
		/// <param name="visible">Устанавливаемое значение</param>
		/// <param name="index">Номер слоя в коллекции <see cref="CADImport.CADConverter.Layers">слоев</see></param>
#else
		/// <summary>
		/// Sets <see cref="CADImport.CADLayer.Visible">CADLayer.Visible</see> parameter for the layer
		/// by the specified <see cref="CADImport.CADLayer">layer</see> index
		/// </summary>
		/// <param name="visible">Setting value</param>
		/// <param name="index">An index of the layer in the <see cref="CADImport.CADConverter.Layers">слоев</see> collection</param>
#endif
		#endregion
		public void SetVisibleLayer(bool visible, int index)
		{
			cadImage.SetLayerVisible(index, visible);
            this.cadImage.ClearSelection();
			this.cadImage.ClearMarkers();
			InitialNewPrintPages();			
			cadPictBox.Invalidate();			
		}
		#region Help
#if RUSHELP
		/// <summary>
		/// Устанавливает параметр <see cref="CADImport.CADLayer.Frozen">CADLayer.Frozen</see> для слоя по указанному номеру <see cref="CADImport.CADLayer">слоя</see>
		/// </summary>
		/// <param name="visible">Устанавливаемое значение</param>
		/// <param name="index">Номер слоя в коллекции <see cref="CADImport.CADConverter.Layers">слоев</see></param>
#else
		/// <summary>
		/// Sets <see cref="CADImport.CADLayer.Frozen">CADLayer.Frozen</see> parameter for the layer
		/// by the specified <see cref="CADImport.CADLayer">layer</see> index
		/// </summary>
		/// <param name="visible">Setting value</param>
		/// <param name="index">An index of the layer in the <see cref="CADImport.CADConverter.Layers">layers</see> collection</param>
#endif
		#endregion
		public void SetFreezeLayer(bool visible, int index)
		{
			cadImage.SetLayerFreeze(index, visible);
			InitialNewPrintPages();
			cadPictBox.Invalidate();			
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Устанавливает цвет для слоя по указанному номеру <see cref="CADImport.CADLayer">слоя</see>
		/// </summary>
		/// <param name="color">Устанавливаемый <see cref="System.Drawing.Color">цвет</see></param>
		/// <param name="index">Номер слоя в коллекции <see cref="CADImport.CADConverter.Layers">слоев</see></param>
		#else
		/// <summary>
		/// Sets layer color by the specified <see cref="CADImport.CADLayer">layer</see> index
		/// </summary>
		/// <param name="color">Setting <see cref="System.Drawing.Color">color</see></param>
		/// <param name="index">An index of the layer in the <see cref="CADImport.CADConverter.Layers">слоев</see> collection</param>

		#endif
		#endregion
		public void SetColorLayer(Color color, int index)
		{
			cadImage.SetLayerColor(index, color);
			InitialNewPrintPages();
			cadPictBox.Invalidate();
		}

		private void InitialNewPrintPages()
		{
			this.prtForm.Image = this.cadImage;
			this.prtForm.CreateNewPages();	
		}

		private void SetLayList()
		{
			if((lForm.LayerList.Items.Count != 0)||(cadImage == null)) 
				return;
            lForm.SetLayList(cadImage.Converter.Layers);
        }

		#region Help
		/// <summary>
		/// Resizes the CAD image to fit drawing's bounds.
		/// </summary>
		/// <remarks>This method is invoked when changing layout or working with 3D Orbit tool.</remarks>
		#endregion Help
        public void DoResize()
        {
            if (cadImage == null)
                return;
            DoResize(new RectangleF(0, 0, (float)cadImage.AbsWidth, (float)cadImage.AbsHeight));
            cadPictBox.Invalidate();
        }

        public void DoResize(RectangleF rect)
        {
            if (cadImage == null)
                return;
            float wh = rect.Width / rect.Height;

            SizeF cadPictBoxSize = new SizeF(cadPictBox.Size - cadPictBox.BorderSize);

            float new_wh = cadPictBoxSize.Width / cadPictBoxSize.Height;
            if (cadImage is CADRasterImage)
                VisibleAreaSize = rect.Size;
            else
                VisibleAreaSize = cadPictBoxSize;
            if (new_wh > wh)
                this.visibleArea.Width = VisibleAreaSize.Height * wh;
            else
            {
                if (new_wh < wh)
                    this.visibleArea.Height = VisibleAreaSize.Width / wh;
                else
                    VisibleAreaSize = cadPictBoxSize;
            }
            this.cadImage.visibleArea = VisibleAreaSize;

            LeftImagePosition = (cadPictBoxSize.Width - VisibleAreaSize.Width) / 2.0f;
            TopImagePosition = (cadPictBoxSize.Height - VisibleAreaSize.Height) / 2.0f;
        }

		#region Help
		/// <summary>
		/// Closes a currently open CAD file.
		/// </summary>
		#endregion Help
		public void CloseFile()
		{
			cadImage.Dispose();
			cadImage = null;
			this.entCreator.Image = null;			
			EnableButton(false);
			this.cadPictBox.Invalidate();
		}
		
		#region Help
		/// <summary>
		/// Makes the layers of the loaded CAD image visible or invisible.
		/// </summary>
		/// <param name="sender">A <see cref="System.Windows.Forms.CheckedListBox">CheckedListBox</see> that initializes changing a visibility of the layers.</param>
		/// <param name="e">An <see cref="System.Windows.Forms.ItemCheckEventArgs">ItemCheckEventArgs</see> object that provides data for the event of changing a layer visibility.</param>
		#endregion Help
		public void chLay_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if(e.NewValue == CheckState.Checked)
				cadImage.SetLayerVisible(e.Index, true);
			else if(e.NewValue == CheckState.Unchecked) 
				cadImage.SetLayerVisible(e.Index, false);
			cadPictBox.Invalidate();
		}

		private void MainForm_Deactivate(object sender, System.EventArgs e)
		{
			detMouseDown = false;
		}		

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Protection.CloseApplication();
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{			
			
			VisibleAreaSize = cadPictBox.Size;			
			Protection.Register(this);
			ChangeControlState();
		}

		private void miAbout_Click(object sender, System.EventArgs e)
		{
            aboutForm.ShowDialog();            
		}

		private void miOpenFile_Click(object sender, System.EventArgs e)
		{
			LoadFile(true);
		}


		private void propGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			this.cadImage.ClearMarkers();
			this.cadImage.DoSelectEntities(this.cadImage.SelectedEntities);			
			this.cadPictBox.Invalidate();
		}

		private void trvEntity_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			cadPictBox.Invalidate();
		}

		private void trvEntity_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			CADImportFace.DecodeEntity(e.Node, cadImage, this.cadPictBox, this.ImageScale);
		}

	
        private void SaveImageAsFormat(string filename, ImageFormat format)
        {
            DRect tmpRect;
            //DRect tmpRect = new DRect(0, 0, ImageRectangleF.Width, ImageRectangleF.Height);
            if (!this.clipRectangle.Enabled)
            {
                tmpRect = new DRect(0, 0, ImageRectangleF.Width, ImageRectangleF.Height);

                rasterSizeForm.Image = this.cadImage;
                rasterSizeForm.CurrentSize = tmpRect;
                rasterSizeForm.SaveFileName = filename;
                if (rasterSizeForm.ShowDialog() == DialogResult.OK)
                    tmpRect = rasterSizeForm.SizeImage;
                else
                    return;
            }
            else
            {
                tmpRect = new DRect(LeftImagePosition, TopImagePosition, LeftImagePosition + ImageRectangleF.Width, TopImagePosition + ImageRectangleF.Height);
            }

            int tmpNumberOfPartsInCircle = this.cadImage.NumberOfPartsInCircle;
            cadImage.Painter.Settings.SaveParams();
            cadImage.Painter.Settings.DefaultColor = Color.Black.ToArgb();
            if ((format == ImageFormat.Emf) || (format == ImageFormat.Wmf))
            {
                cadImage.Painter.Settings.BackgroundColor = Color.Transparent.ToArgb();
                cadImage.Painter.Settings.IsShowBackground = false;
            }
            else
            {
                cadImage.Painter.Settings.BackgroundColor = Color.White.ToArgb();
                cadImage.Painter.Settings.IsShowBackground = true;
            }

            try
            {
                this.cadImage.NumberOfPartsInCircle = CADConst.SetNumberOfPartsInCurve(tmpRect);
                if (this.clipRectangle.Enabled)
                {
                    if ((format == ImageFormat.Emf) || (format == ImageFormat.Wmf))
                        cadImage.ExportToMetafile(filename, tmpRect, this.clipRectangle.ClientRectangle);
                    else
                        cadImage.SaveToFile(filename, format, tmpRect, this.clipRectangle.ClientRectangle, rasterSizeForm.ImagePixelFormat);
                }
                else
                {
                    if ((format == ImageFormat.Emf) || (format == ImageFormat.Wmf))
                        cadImage.ExportToMetafile(filename, tmpRect);
                    else
                        cadImage.SaveToFile(filename, format, tmpRect, rasterSizeForm.ImagePixelFormat);
                }
            }
            finally
            {
                this.cadImage.NumberOfPartsInCircle = tmpNumberOfPartsInCircle;
                cadImage.Painter.Settings.RestoreParams();
            }
            //if (dlgSave.FilterIndex > 4 && dlgSave.FilterIndex < 12)
            //{
            //    cadPictBox.Navigator.CADImage.Painter.SaveToImage(cadPictBox.Navigator.CADImage, dlgSave.FileName, format,
            //        cadPictBox.Navigator.ImageRectangle,
            //         Manager.Editor.ClipRectTool.ClientRectangle,
            //        PixelFormat.Format32bppArgb);
            //}
        }


		private void PutToClipboard()
		{
			if(this.clipRectangle.Enabled)
			{
				DRect tmpRect = ImageDRect;
                cadImage.SaveImageToClipboard(tmpRect, this.clipRectangle.ClientRectangle);
			}
			else
			{
				DRect tmp = new DRect(0.0, 0.0, VisibleAreaSize.Width * ImageScale, VisibleAreaSize.Height * ImageScale);
                cadImage.SaveImageToClipboard(tmp);
			}
		}

	

		private void miEntitiesTree_Click(object sender, System.EventArgs e)
		{
			this.ChangeControlState();
		}

		private void miZoomIn_Click(object sender, System.EventArgs e)
		{
			DoZoomIn();
		}

		private void miZoomOut_Click(object sender, System.EventArgs e)
		{
			DoZoomOut();
		}

		private void miOptions_Click(object sender, System.EventArgs e)
		{
			this.entCreator.OptionsForm.ShowDialog();
		}

		private void miFit_Click(object sender, System.EventArgs e)
		{
			GetExtents();
		}

		#region Help
		/// <summary>
		/// Displays a CAD image in black and white colors.
		/// </summary>
		#endregion Help
		public void DoBlackColor()
		{
			if(cadImage == null) return;
			cadImage.DrawMode = CADDrawMode.Black;
			this.colorDraw = false;
			cadPictBox.Invalidate();
		}

		#region Help
		/// <summary>
		/// Displays a CAD image in all used colors.
		/// </summary>
		#endregion Help
		public void DoNormalColor()
		{
			if(cadImage == null) 
				return;
			cadImage.DrawMode = CADDrawMode.Normal;
			this.colorDraw = true;
			cadPictBox.Invalidate();
		}

		private void miColorDraw_Click(object sender, System.EventArgs e)
		{
			DoNormalColor();
			this.ChangeControlState();
		}

		private void miBlackDraw_Click(object sender, System.EventArgs e)
		{
			DoBlackColor();
			this.ChangeControlState();
		}

		private void miWhiteBack_Click(object sender, System.EventArgs e)
		{
			White_Click();
			this.ChangeControlState();
		}

		private void miBlackBack_Click(object sender, System.EventArgs e)
		{
			Black_Click();
			this.ChangeControlState();
		}

		private void miShowLineWeight_Click(object sender, System.EventArgs e)
		{
			ChangeShowLineWeight();
			this.ChangeControlState();
		}

		private void miArcsSplit_Click(object sender, System.EventArgs e)
		{
			UseWinEllipse();
			this.ChangeControlState();
		}

		private void miDimShow_Click(object sender, System.EventArgs e)
		{
			ChangeDimensionsVisiblity();
			this.ChangeControlState();
		}

		private void miTextsShow_Click(object sender, System.EventArgs e)
		{
			ChangeTextsVisiblity();
			this.ChangeControlState();
		}

		private void miLayersShow_Click(object sender, System.EventArgs e)
		{
			SetLayList();
			lForm.ShowDialog();
		}

		#region Help
		/// <summary>
		/// Shows or hides dimensions in the current CAD image.
		/// </summary>
		#endregion Help
		public void ChangeDimensionsVisiblity()
		{
			this.dimVisible = ! this.dimVisible;
			this.cadImage.DimensionsVisible = this.dimVisible;
			cadPictBox.Invalidate();
		}

		#region Help
		/// <summary>
		/// Shows or hides texts in the current CAD image.
		/// </summary>
		#endregion Help
		public void ChangeTextsVisiblity()
		{
			this.textVisible = ! this.textVisible;
			this.cadImage.TextVisible = this.textVisible;
			cadPictBox.Invalidate();
		}

		#region Help
		/// <summary>
		/// Alternately changes a boolean value that indicates if a thickness of lines is determined by the file data.
		/// </summary>
		#endregion Help
		public void ChangeShowLineWeight()
		{
			this.showLineWeight = ! this.showLineWeight;
			cadImage.IsShowLineWeight = this.showLineWeight;
			this.cadPictBox.Invalidate();
		}

		#region Help
		/// <summary>
		/// Alternately changes a boolean value indicating whether arcs and circles in the CAD image 
		/// are drawn with GDI+ drawing methods without linearization. 
		/// </summary>
		#endregion HelpBestAce
		public void UseWinEllipse()
		{
			//cadImage.UseWinEllipse = ! cadImage.UseWinEllipse; 
			cadPictBox.Invalidate();
		}

		private void ChangeSelectionCond(bool val)
		{
			if(val) 
				cadImage.SelectionMode = SelectionEntityMode.Enabled;
			else
                cadImage.SelectionMode = SelectionEntityMode.Disabled;			
			this.cadImage.ClearSelectCollection();
			this.cadPictBox.Invalidate();
		}

		private void miUndo_Click(object sender, System.EventArgs e)
		{
			this.UndoChangeEntity();
		}

		private void miRedo_Click(object sender, System.EventArgs e)
		{
			this.RedoChangeEntity();
		}

		private void miDelete_Click(object sender, System.EventArgs e)
		{
			this.entCreator.Disable();
			this.RemoveEntity();
			this.stopSnap = true;
		}

		private void miPrintCustom_Click(object sender, System.EventArgs e)
		{							
			DoExtentsForPrint();
			prtForm.ShowDialog();						
		}

		private void DoExtentsForPrint()
		{
			this.cadImage.ClearSelection();
			this.cadImage.ClearMarkers();
			this.cadImage.GetExtents();			
		}

		private void miCopy_Click(object sender, System.EventArgs e)
		{
			this.cadImage.CopyEntities();
		}

		private void miPaste_Click(object sender, System.EventArgs e)
		{
			this.cadImage.PasteEntities();			
			this.cadPictBox.Invalidate();
		}

		private void miCut_Click(object sender, System.EventArgs e)
		{
			this.cadImage.CutEntities();				
			this.cadPictBox.Invalidate();
		}	
		
		private void miSerializeEntities_Click(object sender, System.EventArgs e)
		{		
			if(this.cadImage == null) 
				return;	
			string name = this.cadImage.Converter.Source;
			if(name != null)
			{
				name = Path.GetFileName(name).Replace(Path.GetExtension(name), ApplicationConstants.datextstr);			
				this.dlgSaveSerializeFile.FileName = name;
			}
			if(this.dlgSaveSerializeFile.ShowDialog() != DialogResult.OK) 
				return;		
			SortedList serializeObjects = new SortedList();		
            ArrayList ImageSettings = new ArrayList();
            ImageSettings.Add(pos.X);
            ImageSettings.Add(pos.Y);
            ImageSettings.Add(VisibleAreaSize.Width);
            ImageSettings.Add(visibleArea.Height);
            ImageSettings.Add(ImageScale);
            ImageSettings.Add(cadImage.CurrentLayoutIndex);
            ImageSettings.Add(cadImage.Painter.DrawMatrix);

            if (this.cadImage.SelectedEntities.Count != 0)
				serializeObjects.Add(ApplicationConstants.entitiesstr, this.cadImage.SelectedEntities);
			serializeObjects.Add(ApplicationConstants.blocksstr, this.cadImage.Converter.Blocks);
			serializeObjects.Add(ApplicationConstants.vportsstr, this.cadImage.Converter.VPorts);
			serializeObjects.Add(ApplicationConstants.ltypesstr, this.cadImage.Converter.LTypes);
			serializeObjects.Add(ApplicationConstants.layoutsstr, this.cadImage.Layouts);
			serializeObjects.Add(ApplicationConstants.layersstr, this.cadImage.Converter.Layers);
			serializeObjects.Add(ApplicationConstants.stylesstr, this.cadImage.Converter.Styles);
			serializeObjects.Add(ApplicationConstants.imagesettingsstr, ImageSettings);
			//serializeObjects.Add(ApplicationConstants.xrefsstr, this.cadImage.Converter.XRefs);			
			Serialize(this.dlgSaveSerializeFile.FileName, serializeObjects);
		}

		private void miDeserializeEntities_Click(object sender, System.EventArgs e)
		{
			if(this.dlgOpenSerializeFile.ShowDialog() != DialogResult.OK)
				return;

			if (cadImage != null)
			{
				if (MessageBox.Show("Close current image?", "", MessageBoxButtons.YesNo) == DialogResult.Yes) CloseFile();
				else return;
			}
				this.cadImage = new CADImage();				
				this.cadImage.InitialNewImage();	
				this.cadImage.Converter.Layers.Clear();		
				this.cadImage.Converter.Layouts.Entities.Clear();	

			SortedList deserializeObjects = Deserialize(this.dlgOpenSerializeFile.FileName) as SortedList;
			if(deserializeObjects == null) 
				return;					
			LoadEntities(deserializeObjects, this.cadImage.Converter.Layers, ApplicationConstants.layersstr);
			LoadEntities(deserializeObjects, this.cadImage.Layouts, ApplicationConstants.layoutsstr);			
			LoadEntities(deserializeObjects, this.cadImage.Converter.Blocks, ApplicationConstants.blocksstr);
			LoadEntities(deserializeObjects, this.cadImage.Converter.VPorts, ApplicationConstants.vportsstr);			
			LoadEntities(deserializeObjects, this.cadImage.Converter.LTypes, ApplicationConstants.ltypesstr);			
			LoadEntities(deserializeObjects, this.cadImage.Converter.Styles, ApplicationConstants.stylesstr);						
            this.cadImage.Painter.DrawMatrix = (CADMatrix)(deserializeObjects[ApplicationConstants.imagesettingsstr] as ArrayList)[6];

            cadImage.DefaultLayoutIndex = (int)(deserializeObjects[ApplicationConstants.imagesettingsstr] as ArrayList)[5];
				SetCADImageOptions();										
				EnableButton(true);			

            LoadEntities(deserializeObjects, this.cadImage.SelectedEntities, ApplicationConstants.entitiesstr);
           	this.pos.X = (float)(deserializeObjects[ApplicationConstants.imagesettingsstr] as ArrayList)[0];
            this.pos.Y = (float)(deserializeObjects[ApplicationConstants.imagesettingsstr] as ArrayList)[1];
			old_Pos.X = pos.X - 1;
			old_Pos.Y = pos.Y - 1;
            this.ImageScale = (float)(deserializeObjects[ApplicationConstants.imagesettingsstr] as ArrayList)[4];
			this.ImagePreviousScale = ImageScale;
            this.visibleArea.Width = (float)(deserializeObjects[ApplicationConstants.imagesettingsstr] as ArrayList)[2];
            this.visibleArea.Height = (float)(deserializeObjects[ApplicationConstants.imagesettingsstr] as ArrayList)[3];
            	
                SetPictureBoxLoadState(false, dlgOpenSerializeFile.FileName);
		}
		#region Help
		#if RUSHELP
		/// <summary>
		/// Серилизует примитивы
		/// </summary>
		/// <param name="fileName">Имя файла</param>
		/// <param name="sender">Серилизуемые объекты</param>
		#else
		/// <summary>
		/// Serializes entities
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="sender">Objects to serialize</param>
		#endif		
		#endregion
		public void Serialize(string fileName, object sender)
		{
			Stream stm = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
			BinaryFormatter bf = new BinaryFormatter();				
			bf.Serialize(stm, sender);
			stm.Close();
		}
		#region Help
#if RUSHELP
		/// <summary>
		/// Десерилизует примитивы
		/// </summary>
		/// <param name="fileName">Имя файла</param>		
#else
		/// <summary>
		/// Deserializes entities
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>Returns deserialized objects</returns>
#endif		
		#endregion
		public object Deserialize(string fileName)
		{
			Stream stm = File.Open(fileName, FileMode.Open, FileAccess.Read);
			BinaryFormatter bf = new BinaryFormatter();			
			object res = bf.Deserialize(stm);
			stm.Close();			
			return res;
		}

		private void LoadEntities(SortedList deserializeObjects, object collectAdd, string section)
		{
			CADEntity ent;		
			Object tmp;
			CADEntityCollection entities = null; 
			SortedList entitiesLst = null; 
			CADLayoutCollection layouts = null; 
			if(deserializeObjects.ContainsKey(section))
			{
				if(section == ApplicationConstants.layoutsstr)
					layouts = (CADLayoutCollection)deserializeObjects[section];						
				else
				{
					tmp = deserializeObjects[section];					
					entities = tmp as CADEntityCollection;						
					entitiesLst = tmp as SortedList;
				}
			}
			if(entities != null)			
			{
				for(int i = 0; i < entities.Count; i++)
				{
					ent = entities[i] as CADEntity;		
					if(ent == null)
						continue;							
					LoadEntity(ent, true, collectAdd);					
				}
				return;
			}
			if(entitiesLst != null)			
			{
				for(int i = 0; i < entitiesLst.Count; i++)
				{
					ent = entitiesLst.GetByIndex(i) as CADEntity;		
					if(ent == null)
						continue;							
					LoadEntity(ent, true, collectAdd);					
				}
				return;
			}
			if(layouts != null)			
			{
				for(int i = 0; i < layouts.Count; i++)
				{
					ent = layouts[i] as CADEntity;						
					if(ent == null)
						continue;							
					LoadEntity(ent, true, collectAdd);					
				}
			}
		}

		private void LoadEntity(CADEntity ent, bool detAdd, object collectAdd)
		{			
			CADLayout layout;
			CADInsert ins = ent as CADInsert;		
			if(ent is CADLeader)
				ins = (ent as CADLeader).Insert;
			if(ins != null)
			{				
				if(ins.Block != null)
				{
					for(int i = 0; i < ins.Block.Count; i++)						
						LoadEntity(ins.Block.Entities[i], false, collectAdd);											
				}				
			}			
				ent.Loaded(this.cadImage.Converter);			
			this.cadImage.Converter.OnCreate(ent);									
			if(detAdd)				
			{				
				if(collectAdd is CADLayoutCollection)
				{	
					layout = ent as CADLayout;
					for(int i = 0; i < layout.Entities.Count; i++)						
						LoadEntity(layout.Entities[i], false, collectAdd);								
					(collectAdd as CADLayoutCollection).Entities.Add(layout);
				}
				else
					if(collectAdd is CADEntityCollection)
					(collectAdd as CADEntityCollection).Add(ent);				
			}
		}

        private void menuItem1_Click(object sender, EventArgs e)
        {
            //if (cadImage != null)
            //{
            //    long av=0;
            //    for(int i=0;i<cadImage.list.Count;i++)
            //    {
            //        av+=cadImage.list[i];
            //    }
            //    av/=cadImage.list.Count;
            //    MessageBox.Show(String.Format("{0}", av));
            //}
        }		
	}	

	internal abstract class ApplicationConstants
	{
		public static readonly string defaultstr;
		public static readonly string languagepath;
		public static readonly string loadfilestr;
		public static readonly string msgBoxDebugCaption;
		public static readonly string sepstr;
		public static readonly string notsupportedstr;
		public static readonly string notsupportedextstr;
		public static readonly string appnamestr;
		public static readonly string sepstr2;
		public static readonly string jpgextstr;
		public static readonly string bmpextstr;
		public static readonly string tiffextstr;
		public static readonly string gifextstr;
		public static readonly string emfextstr;
		public static readonly string wmfextstr;
		public static readonly string pngextstr;			
		public static readonly string dxfextstr;
		public static readonly string lngextstr;
		public static readonly string languagestr;
		public static readonly string languageIDstr;
		public static readonly string backcolorstr;
		public static readonly string blackstr;
		public static readonly string showentitystr;
		public static readonly string drawmodestr;
		public static readonly string truestr;
		public static readonly string shxpathcnstr;
		public static readonly string installstr;
		public static readonly string sepstr3;
		public static readonly string typelcstr;
		public static readonly string floatingstr;
		public static readonly string hoststr;
		public static readonly string portstr;
		public static readonly string appconst;
		public static readonly string blackstr2;
		public static readonly string whitestr;
		public static readonly string registerstr;
		public static readonly string colordrawstr;
		public static readonly string datextstr;
		public static readonly string messagestr;
		public static readonly string headstr1;
		public static readonly string headstr2;
		public static readonly string headstr3;
		public static readonly string headstr4;
		public static readonly string namestr;
		public static readonly string visiblestr;
		public static readonly string freezestr;
		public static readonly string colorstr;		
		public static readonly string entitiesstr;
		public static readonly string blocksstr;
		public static readonly string vportsstr;
		public static readonly string ltypesstr;
		public static readonly string layoutsstr;
		public static readonly string layersstr;
		public static readonly string stylesstr;
		public static readonly string xrefsstr;
		public static readonly string imagesettingsstr;

		static ApplicationConstants()
		{
			defaultstr = "Default";
			languagepath = "LanguagePath";
			loadfilestr = "Loading file...";
			msgBoxDebugCaption = "Debug application message";
			sepstr = " - ";
			notsupportedstr = "not supported";
			notsupportedextstr = "Not supported in current version!";
			appnamestr = "CADImportNet Demo";
			sepstr2 = " : ";			
			jpgextstr = ".JPG";
			bmpextstr = ".BMP";
			tiffextstr = ".TIFF";
			gifextstr = ".GIF";
			emfextstr = ".EMF";
			wmfextstr = ".WMF";
			pngextstr = ".PNG";			
			dxfextstr = ".DXF";
			lngextstr = ".lng";
			languagestr = "Language";
			languageIDstr = "LanguageID";
			backcolorstr = "BackgroundColor";
			blackstr = "BLACK";
			showentitystr = "ShowEntity";
			drawmodestr = "drawMode";
			truestr = "TRUE";
			shxpathcnstr = "SHXPathCount";
			installstr = "Install";
			sepstr3 = ";";
			typelcstr = "Type_license";
			floatingstr = "floating";
			hoststr = "Host";
			portstr = "Port";
			appconst = "Application";
			blackstr2 = "Black";
			whitestr = "White";
			registerstr = "register";
			colordrawstr = "ColorDraw";
			datextstr = ".dat";
			messagestr = "If you proceed with this operation, all added objects will be lost. Continue?";
			headstr1 = "Head1";
			headstr2 = "Head2";
			headstr3 = "Head3";
			headstr4 = "Head4";
			namestr = "Name";
			visiblestr = "Visible";
			freezestr = "Freeze";
			colorstr = "Color";	
			entitiesstr = "Entities";
			blocksstr = "Blocks";
			vportsstr = "VPorts";
			ltypesstr = "LTypes";
			layoutsstr = "Layouts";
			layersstr = "Layers";
			stylesstr = "Styles";
			xrefsstr = "XRefs";
			imagesettingsstr = "ImageSettings";
		}
	}
}
