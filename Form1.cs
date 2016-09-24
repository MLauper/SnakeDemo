using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AForge.Imaging;
using AForge.Imaging.Filters;

public partial class Form1 : Form
{
   #region Members
   private FileInfo  m_imageFI;
   private Bitmap    m_bmpOrig;
   private Bitmap    m_bmpGray;
   private Bitmap    m_bmpSmoo;
   private Bitmap    m_bmpEdge;
   private float     m_imageScale = 3.0f;
   private Snake     m_snake;
   #endregion

   public Form1()
   {
      InitializeComponent();

      // This is needed so that it does not flicker on repaints
      this.SetStyle(ControlStyles.DoubleBuffer, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

      // Mousewheel handlers must be added manually (!)
      this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);

      // Create the snake
      m_snake = new Snake(this);
   }

   #region Form Event Handlers
   private void Form1_Load(object sender, EventArgs e)
   {
      // Try to load the monkey image
      DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
      if (File.Exists(dir.FullName + "/images/monkey.png"))
         loadImage(dir.FullName + "/images/monkey.png");
      else
      {  dir = new DirectoryInfo(dir.Parent.FullName);
         if (dir.Exists && File.Exists(dir.FullName + "/images/monkey.png"))
            loadImage(dir.FullName + "/images/monkey.png");
      }
   }
   private void Form1_Paint(object sender, PaintEventArgs e)
   {  
      Graphics g = e.Graphics;
      GraphicsContainer gc = g.BeginContainer();
      g.ScaleTransform(m_imageScale, m_imageScale);
      g.InterpolationMode = InterpolationMode.NearestNeighbor; // for images
      g.SmoothingMode = SmoothingMode.HighQuality;             // for lines
      
      if (radOriginal.Checked && m_bmpOrig != null)
         g.DrawImage(m_bmpOrig, 0, 0, m_bmpOrig.Width, m_bmpOrig.Height);
      else if (radGrayscale.Checked && m_bmpGray != null)
         g.DrawImage(m_bmpGray, 0, 0, m_bmpGray.Width, m_bmpGray.Height);
      else if (radSmoothed.Checked && m_bmpSmoo != null)
         g.DrawImage(m_bmpSmoo, 0, 0, m_bmpSmoo.Width, m_bmpSmoo.Height);
      else if (radEdges.Checked && m_bmpEdge != null)
         g.DrawImage(m_bmpEdge, 0, 0, m_bmpEdge.Width, m_bmpEdge.Height);

      // Determine the image from witch to calculate the image force
      Bitmap bmp;
      if (radForceEdge.Checked) bmp = m_bmpEdge;
      else if (radForceSmooth.Checked) bmp = m_bmpSmoo;
      else bmp = m_bmpGray;

      m_snake.draw(g, bmp);

      g.EndContainer(gc);
   }
   private void Form1_MouseWheel(object sender, MouseEventArgs e)
   {
      if (m_bmpOrig == null) return;
      m_imageScale += Math.Sign(e.Delta);
      if (m_imageScale < 1.0f) m_imageScale = 1.0f;
      this.Text = "Snake Demo: " + m_imageFI.Name + 
                  " (" + m_bmpOrig.Width.ToString() + "x" + m_bmpOrig.Height.ToString() + 
                  ", Scale: " + m_imageScale.ToString("0") + ")";
      Invalidate();
   }
   private void Form1_MouseDown(object sender, MouseEventArgs e)
   {      
      if (m_bmpOrig==null) return;

      if (e.Button == MouseButtons.Left)
      {  
         Vec2F mousePos = new Vec2F(e.X / m_imageScale, e.Y / m_imageScale);
         m_snake.addPoint(mousePos);
         btnDeleteSnake.Enabled = true;
         btnDoublePoints.Enabled = m_snake.PointsCount > 2;
         btnHalfPoints.Enabled = m_snake.PointsCount > 2;
      }
      Invalidate();
   }
   
   private void btnFileOpen_Click(object sender, EventArgs e)
   {
      OpenFileDialog dlg = new OpenFileDialog();
      if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
      {  
         try
         {  
            loadImage(dlg.FileName);
            Invalidate();
         } 
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
      }
   }
   private void btnDeleteSnake_Click(object sender, EventArgs e)
   {  
      m_snake.deleteAllPoints();
      Invalidate();
      btnDeleteSnake.Enabled = false;
      radUpdateManual.Checked = true;
   }
   private void btnDoublePoints_Click(object sender, EventArgs e)
   {
      m_snake.doublePoints();
      btnHalfPoints.Enabled = true;
      Invalidate();
   }
   private void btnHalfPoints_Click(object sender, EventArgs e)
   {
      m_snake.halfPoints();
      Invalidate();
   }
   private void numGauss_ValueChanged(object sender, EventArgs e)
   {
      GaussianBlur filterGauss = new GaussianBlur(1, (int)numGauss.Value);
      m_bmpSmoo = filterGauss.Apply(m_bmpGray);

      SobelEdgeDetector filterSobel = new SobelEdgeDetector();
      m_bmpEdge = filterSobel.Apply(m_bmpSmoo);
      filterGauss.ApplyInPlace(m_bmpEdge);

      Invalidate();
   }

   private void radOriginal_CheckedChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void radGrayscale_CheckedChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void radSmoothed_CheckedChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void radEdges_CheckedChanged(object sender, EventArgs e)
   {
      Invalidate();
   }

   private void radForceGray_CheckedChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void radForceSmooth_CheckedChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void radForceEdge_CheckedChanged(object sender, EventArgs e)
   {
      Invalidate();
   }

   private void radUpdateManual_CheckedChanged(object sender, EventArgs e)
   {
      updateTimer.Stop();
   }
   private void radUpdateAutoSlow_CheckedChanged(object sender, EventArgs e)
   {
      updateTimer.Start();
   }
   private void radUpdateFast_CheckedChanged(object sender, EventArgs e)
   {
      updateTimer.Stop();
   }
   
   private void numAlpha_ValueChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void numBeta_ValueChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void numGamma_ValueChanged(object sender, EventArgs e)
   {
      Invalidate();
   }
   private void numDelta_ValueChanged(object sender, EventArgs e)
   {
      Invalidate();
   }

   private void chkShowForces_CheckedChanged(object sender, EventArgs e)
   {
   
      Invalidate();
   }
   

   private void numIntervalMS_ValueChanged_1(object sender, EventArgs e)
   {
      updateTimer.Interval = (int)numIntervalMS.Value;
   }
   private void updateTimer_Tick(object sender, EventArgs e)
   {
      m_snake.update();
   }
   #endregion

   private void loadImage(string imageFileName)
   {        
      // Clear everything
      radUpdateManual.Checked = true;
      m_snake.deleteAllPoints();
      if (m_bmpOrig != null) {m_bmpOrig.Dispose(); m_bmpOrig = null;}
      if (m_bmpGray != null) {m_bmpGray.Dispose(); m_bmpGray = null;}
      if (m_bmpSmoo != null) {m_bmpSmoo.Dispose(); m_bmpSmoo = null;}
      if (m_bmpEdge != null) {m_bmpEdge.Dispose(); m_bmpEdge = null;}
            
      // Load image
      m_bmpOrig = (Bitmap)System.Drawing.Image.FromFile(imageFileName);
      m_imageFI = new FileInfo(imageFileName);
      this.Text = "Snake Demo: " + m_imageFI.Name + 
                  " (" + m_bmpOrig.Width.ToString() + "x" + m_bmpOrig.Height.ToString() + 
                  ", Scale: " + m_imageScale.ToString("0") + ")";
      
      // Build grayscale image
      if (m_bmpOrig.PixelFormat == PixelFormat.Format8bppIndexed)
         m_bmpGray = (Bitmap)m_bmpOrig.Clone();
      else
      {
         Grayscale filterGray = new Grayscale(0.257, 0.504, 0.098);
         m_bmpGray = filterGray.Apply(m_bmpOrig);
      }

      // Build smoothed imgage with a gaussian blur filter
      GaussianBlur filterGauss = new GaussianBlur(1, (int)numGauss.Value);
      m_bmpSmoo = filterGauss.Apply(m_bmpGray);

      // Build edges with a Sobel filter
      SobelEdgeDetector filterSobel = new SobelEdgeDetector();
      m_bmpEdge = filterSobel.Apply(m_bmpSmoo);
      filterGauss.ApplyInPlace(m_bmpEdge);

      // Enabel controls
      btnDeleteSnake.Enabled = false;
      btnDoublePoints.Enabled = false;
      btnHalfPoints.Enabled = false;
      grpSnakeForces.Enabled = true;
      grpImageForce.Enabled = true;
      grpDisplay.Enabled = true;
      grpUpdate.Enabled = true;
   }

}
