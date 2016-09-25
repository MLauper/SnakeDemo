using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

/// <summary>
/// The snake with points that implements an active contour
/// </summary>
class Snake
{  
   #region members
   Form1 frm;                 // reference to the parent form

   /// <summary>
   /// Snake point type
   /// </summary>
   private class SnakePoint   
   {  public Vec2F pos;       // position
      public Vec2F F_elas;    // internal elastic force vector (blue)
      public Vec2F F_curv;    // internal curvature equalization force vector (cyan)
      public Vec2F F_ball;    // internal balloon expansion force vector (magenta)
      public Vec2F F_img;     // external image force vector (green)
   }
   private List<SnakePoint> m_points = new List<SnakePoint>();

   private Vec2F m_F_elasSum   = new Vec2F(0, 0); // sum of all elastic forces
   private Vec2F m_F_curvSum   = new Vec2F(0, 0); // sum of all curvature forces
   private Vec2F m_F_ballSum   = new Vec2F(0, 0); // sum of all curvature forces
   private Vec2F m_F_imgSum    = new Vec2F(0, 0); // sum of all image forces

   private Vec2F m_center = new Vec2F(0, 0); // center of all snake points

   // some helper constants
   const float RAD2DEG = (float)(180 / Math.PI);
   const float DEG2RAD = (float)(Math.PI / 180);
   #endregion

   public Snake(Form1 parentForm)
   {
      frm = parentForm;
   }

   /// <summary>
   /// Returns the no. of points in the snake
   /// </summary>
   public int PointsCount
   {  get { return m_points.Count; } 
   }

   /// <summary>
   /// Adds a new point to the snake at the currente mousPos
   /// </summary>
   /// <param name="mousePos">Mouse down point</param>
   public void addPoint(Vec2F mousePos)
   {
      SnakePoint p = new SnakePoint();
      p.pos    = mousePos;
      p.F_elas = new Vec2F(0, 0);
      p.F_curv = new Vec2F(0, 0);
      p.F_ball = new Vec2F(0, 0);
      p.F_img  = new Vec2F(0, 0);
      m_points.Add(p);
   }

   /// <summary>
   /// Doubles the no. of points in the snake
   /// </summary>
   public void doublePoints()
   {
      // Create copy
      SnakePoint[] snakepointsCopy = new SnakePoint[m_points.Count];
      m_points.CopyTo(snakepointsCopy);

      m_points.Clear();
      
      // Refill points with middle points
      for (int i=0; i<snakepointsCopy.Length; ++i)
      {  
         SnakePoint p = snakepointsCopy[i];
         SnakePoint p_next;
         if (i < snakepointsCopy.Length-1)
              p_next = snakepointsCopy[i+1];
         else p_next = snakepointsCopy[0];
         
         addPoint(p.pos);

         Vec2F midPoint = new Vec2F((p.pos + p_next.pos) * 0.5f);
         addPoint(midPoint);
      }
   }

   /// <summary>
   /// Cuts the no. of points of the snake in half
   /// </summary>
   public void halfPoints()
   {
      if (m_points.Count > 4)
      {
         // Create copy
         SnakePoint[] snakepointsCopy = new SnakePoint[m_points.Count];
         m_points.CopyTo(snakepointsCopy);
         m_points.Clear();
      
         // Add every second point
         for (int i=0; i<snakepointsCopy.Length; i+=2)
            addPoint(snakepointsCopy[i].pos);
      } else
      {
         m_points.Clear();
         m_center  = Vec2F.Zero;
         m_F_elasSum = Vec2F.Zero;
         m_F_curvSum    = Vec2F.Zero;
         m_F_imgSum   = Vec2F.Zero;
         frm.btnDeleteSnake.Enabled = false;
         frm.btnDoublePoints.Enabled = false;
         frm.btnHalfPoints.Enabled = false;
      }
   }

   /// <summary>
   /// Draws the snake
   /// </summary>
   /// <param name="g">graphics context for drawing</param>
   /// <param name="bmp">bitmap from witch the image forces are calculated</param>
   public void draw(Graphics g, Bitmap bmp)
   {
      if (m_points.Count == 0) return;

      calculateFroces(bmp);

      const float C = 50.0f; // Constant scale factor for the vector visualization

      for (int i=0; i<m_points.Count; ++i)
      {  
         SnakePoint p = m_points[i];
         SnakePoint p_next;
         if (i < m_points.Count-1)
              p_next = m_points[i+1];
         else p_next = m_points[0];
         
         // Draw outgoing line and circle around p1.pos
         g.DrawLine(Pens.Red, p.pos.x, p.pos.y, p_next.pos.x, p_next.pos.y);
         g.DrawEllipse(Pens.Yellow, p.pos.x-1.0f, p.pos.y-1.0f, 2.0f, 2.0f);

       
         if (frm.chkShowForces.Checked)
         {
            // Draw elastic force vector
            Vec2F eF = p.pos + p.F_elas * C;
            g.DrawLine(Pens.Blue,  p.pos.x, p.pos.y, eF.x, eF.y);

            // Draw curvature force vector
            Vec2F cF = p.pos + p.F_curv * C;
            g.DrawLine(Pens.Cyan,  p.pos.x, p.pos.y, cF.x, cF.y);

            // Draw balloon force vector
            Vec2F bF = p.pos + p.F_ball * C;
            g.DrawLine(Pens.Magenta,  p.pos.x, p.pos.y, bF.x, bF.y);

            // Draw image force vector
            Vec2F iF = p.pos + p.F_img * C;
            g.DrawLine(Pens.Green, p.pos.x, p.pos.y, iF.x, iF.y);
         }
      }

      if (frm.chkShowForces.Checked)
      {
         // Draw summed up forces
         g.DrawEllipse(Pens.White, m_center.x-1.0f, m_center.y-1.0f, 2.0f, 2.0f);
         Vec2F sum_eF = m_center + m_F_elasSum * C;
         Vec2F sum_cF = m_center + m_F_curvSum * C;
         Vec2F sum_bF = m_center + m_F_ballSum * C;
         Vec2F sum_iF = m_center + m_F_imgSum  * C;
         g.DrawLine(Pens.Blue,    m_center.x, m_center.y, sum_eF.x, sum_eF.y);
         g.DrawLine(Pens.Cyan,    m_center.x, m_center.y, sum_cF.x, sum_cF.y);
         g.DrawLine(Pens.Magenta, m_center.x, m_center.y, sum_bF.x, sum_bF.y);
         g.DrawLine(Pens.Green,   m_center.x, m_center.y, sum_iF.x, sum_iF.y);
      }
   }

   /// <summary>
   /// Calculates all the forces of all snake points
   /// </summary>
   /// <param name="bmp">bitmap from witch the image force is taken</param>
   public void calculateFroces(Bitmap bmp)
   {
      if (m_points.Count < 3) return;

      const float C_ELAS = 0.1f;  // constant to scale elastic force vector
      const float C_CURV = 0.5f;  // constant to scale curvature force vector
      const float C_BALL = 0.5f;  // constant to scale balloon force vector
      const float C_IMG  = 0.1f;  // constant to scale image force
      
      // Caluclate center of mass first
      m_center  = Vec2F.Zero;
      for (int i=0; i<m_points.Count; ++i)
         m_center += ((SnakePoint)m_points[i]).pos;
      m_center /= m_points.Count;

      // Clear the sum vectors
      m_F_elasSum = Vec2F.Zero;
      m_F_curvSum = Vec2F.Zero;
      m_F_ballSum = Vec2F.Zero;
      m_F_imgSum  = Vec2F.Zero;

      for (int i=0; i<m_points.Count; ++i)
      {  
         SnakePoint p = m_points[i];
         
         // Get the neighbours
         SnakePoint p_next, p_prev;
         if (i == 0)
         {  p_next = m_points[i+1];
            p_prev = m_points[m_points.Count-1];
         } else if (i == m_points.Count-1)
         {  p_next = m_points[0];
            p_prev = m_points[i-1];
         } else
         {  p_next = m_points[i+1];
            p_prev = m_points[i-1];
         }
         
         ///////////////////////
         //  Elasticity Force //
         ///////////////////////

         // Build the elasticity force vector
         Vec2F vNext = p_next.pos - p.pos;
         Vec2F vPrev = p_prev.pos - p.pos;
         p.F_elas = vNext + vPrev;

         // Scale the force components
         p.F_elas *= (float)frm.numAlpha.Value * C_ELAS;

         // Sum up
         m_F_elasSum += p.F_elas;

         
         ///////////////////
         //  Curve Force  //
         ///////////////////

         // Calculate the curvature equalization force vector
         vNext.Normalize();
         vPrev.Normalize();
         Vec2F vCurv = vNext + vPrev;
         vCurv.Normalize();
         p.F_curv = vCurv;

         // Scale the force vector
         p.F_curv *= (float)frm.numBeta.Value * C_CURV;
         
         // Sum up
         m_F_curvSum += p.F_curv;


         /////////////////////
         //  Balloon Force  //
         /////////////////////

         // Calculate the outward pointing balloon expansion force vector
         p.F_ball = -(vNext + vPrev);

         // Scale the force vector
         p.F_ball *= (float)frm.numDelta.Value * C_BALL;
         
         // Sum up
         m_F_ballSum += p.F_ball;
         

         ///////////////////
         //  Image Force  //
         ///////////////////

         // Check first the boundries of the lookup coordinates
         int x = (int)p.pos.x;
         int y = (int)p.pos.y;
         x = Math.Min(x, bmp.Width -2); x = Math.Max(x, 1);
         y = Math.Min(y, bmp.Height-2); y = Math.Max(y, 1);
         
         // Build the image force vector
         // bmp.GetPixel(x, y).R retrieves the gray value (0-255) at pos. x,y
         // TODO: Is it really the gray value? Documentation notes: Gets the red component value
         p.F_img.x = bmp.GetPixel(x+1, y).R - bmp.GetPixel(x-1, y).R;
         p.F_img.y = bmp.GetPixel(x, y+1).R - bmp.GetPixel(x, y-1).R;

         // Scale the force components 
         p.F_img *= (float)frm.numGamma.Value * C_IMG;

         // Sum up
         m_F_imgSum += p.F_img;
      }
   }

   /// <summary>
   /// Updates all points by summing up all point forces
   /// </summary>
   public void update()
   {
      if (m_points.Count < 3) return;
      
      for (int i=0; i<m_points.Count; ++i)
      {  
         SnakePoint p = (SnakePoint)m_points[i];

         // Update position by summing up force vectors

         //////////////////////////////////////////////////
         p.pos += p.F_elas + p.F_curv + p.F_ball + p.F_img;
         //////////////////////////////////////////////////
      }
      
      frm.Invalidate();
   }

   /// <summary>
   /// Deletes all points of the snake
   /// </summary>
   public void deleteAllPoints()
   {  
      // Clear everything
      m_points.Clear();
      m_center  = Vec2F.Zero;
      m_F_elasSum = Vec2F.Zero;
      m_F_curvSum = Vec2F.Zero;
      m_F_imgSum = Vec2F.Zero;
   }
}
