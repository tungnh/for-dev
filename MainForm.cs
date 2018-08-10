using MVPSample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MVPSample.Views.Forms
{
    public partial class MainForm : Form
    {
        // Pen
        Pen penShape = new Pen(Color.White) { DashStyle = DashStyle.Solid };
        Pen penResize = new Pen(Color.White) { DashStyle = DashStyle.Solid };

        // Check
        bool bHaveMouseDown;
        bool bHaveSelect;
        bool bHaveResize;

        // Properties
        Point ptOriginal = new Point();
        Point ptLast = new Point();
        Rectangle rectCropArea;
        List<Shape> shapes = new List<Shape>();

        ResizeType currentResizeType;
        int currentIndex = 0;
        int index = 0;
        int mResizeBox = 2;
        int mOusideBox = 5;
        int defaultHeight = 20;
        int minWidth = 2;
        int minHeight = 2;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SrcPicBox.Image = Image.FromFile(@"..\..\..\MVPSample\Images\Chrysanthemum.png");
        }

        private void SrcPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Make a note that we "have the mouse down".
            bHaveMouseDown = true;

            // Set current point
            var ptCurrent = new Point(e.X, e.Y);

            // Set "last point"
            ptLast.X = ptCurrent.X;
            ptLast.Y = ptCurrent.Y;

            // Has select shape
            if (bHaveSelect)
            {
                var currentShape = this.shapes.FirstOrDefault(x => x.Index == currentIndex);
                if (currentShape != null)
                {
                    var resizeBoxInside = this.FindResizeBoxInsidePoint(currentShape, ptCurrent);
                    // If inside resize
                    if (resizeBoxInside != null)
                    {
                        bHaveResize = true;

                        // Store the "starting point" for this rubber-band rectangle.
                        ptOriginal.X = currentShape.RectOriginal.X;
                        ptOriginal.Y = currentShape.RectOriginal.Y;

                        currentResizeType = resizeBoxInside.ResizeType;
                        this.UpdateCursor(currentResizeType);
                        return;
                    }
                }
            }

            //Check edit shape
            var shapeInside = this.FindShapeInPoint(ptCurrent);
            if (shapeInside != null)
            {
                // Make a note that we "have the resize".
                bHaveSelect = true;
                bHaveResize = false;

                // Set rectangle shape
                rectCropArea = shapeInside.RectOriginal;

                // Store the "starting point" for this rubber-band rectangle.
                ptOriginal.X = rectCropArea.X;
                ptOriginal.Y = rectCropArea.Y;

                // Draw resize boxs
                this.InitialResizeBox(shapeInside);

                currentIndex = shapeInside.Index;
            }
            else
            {
                bHaveSelect = false;
                bHaveResize = false;

                // Store the "starting point" for this rubber-band rectangle.
                ptOriginal.X = ptCurrent.X;
                ptOriginal.Y = ptCurrent.Y;

                // Clear resize list
                this.shapes.ForEach(x => x.BoundingBoxs.Clear());

                var currentShapeType = this.GetShapeTypeSelect();
                var shape = new Shape() { ShapeType = currentShapeType, POriginal = ptOriginal, RectOriginal = new Rectangle(ptOriginal, new Size { Height = (currentShapeType == ShapeType.RectanleRotate) ? defaultHeight : 0 }), AngleRotate = 0, Index = index++ };
                shapes.Add(shape);
                currentIndex = shape.Index;
            }

            SrcPicBox.Refresh();
        }

        private void SrcPicBox_MouseUp(object sender, MouseEventArgs e)
        {
            // Set internal flag to know we no longer "have the mouse".
            bHaveMouseDown = false;
            // Set internal flag to know we no longer "have the resize shape".
            bHaveResize = false;

            // Set flags to know that there is no "previous" line to reverse.
            ptLast.X = -1;
            ptLast.Y = -1;
            ptOriginal.X = -1;
            ptOriginal.Y = -1;
        }

        private void SrcPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point ptCurrent = new Point(e.X, e.Y);

            // If we "have the mouse", then we draw our lines.
            if (bHaveMouseDown)
            {
                // Find current shape
                var currentShape = shapes.FirstOrDefault(x => x.Index == currentIndex);
                if (bHaveSelect)
                {
                    if (currentShape != null)
                    {
                        rectCropArea = currentShape.RectOriginal;
                        if (bHaveResize)
                        {
                            if (currentShape.ShapeType == ShapeType.RectanleRotate)
                            {
                                Point ptCenter = new Point(currentShape.RectOriginal.X + currentShape.RectOriginal.Width / 2, currentShape.RectOriginal.Y + currentShape.RectOriginal.Height / 2);
                                var dX = ptCurrent.X - ptCenter.X;
                                var dY = ptCurrent.Y - ptCenter.Y;
                                int distance = (int)Math.Sqrt(dX * dX + dY * dY);
                                if (distance > 0)
                                {
                                    double radianB = Math.Atan((double)dX / dY);
                                    var alpha = (float)(radianB * (180 / Math.PI));

                                    // Handle resize shape
                                    switch (currentResizeType)
                                    {
                                        case ResizeType.Left:
                                        case ResizeType.Right:
                                            rectCropArea.X = ptCenter.X - distance;
                                            rectCropArea.Y = ptCenter.Y - rectCropArea.Height / 2;
                                            rectCropArea.Width = distance * 2;

                                            currentShape.AngleRotate = 90 - alpha;
                                            break;
                                        case ResizeType.Top:
                                        case ResizeType.Bottom:
                                            rectCropArea.X = ptCenter.X - rectCropArea.Width / 2;
                                            rectCropArea.Y = ptCenter.Y - distance;
                                            rectCropArea.Height = distance * 2;

                                            currentShape.AngleRotate = -alpha;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                // Handle resize shape
                                switch (currentResizeType)
                                {
                                    case ResizeType.Left:
                                        if (rectCropArea.Width - (e.X - ptLast.X) >= minWidth)
                                        {
                                            rectCropArea.X += e.X - ptLast.X;
                                            rectCropArea.Width -= e.X - ptLast.X;
                                            if (currentShape.ShapeType == ShapeType.Square || currentShape.ShapeType == ShapeType.Circle)
                                            {
                                                rectCropArea.Height = rectCropArea.Width;
                                            }
                                        }
                                        
                                        break;
                                    case ResizeType.Top:
                                        if (rectCropArea.Height - (e.Y - ptLast.Y) >= minHeight)
                                        {
                                            rectCropArea.Y += e.Y - ptLast.Y;
                                            rectCropArea.Height -= e.Y - ptLast.Y;
                                            if (currentShape.ShapeType == ShapeType.Square || currentShape.ShapeType == ShapeType.Circle)
                                            {
                                                rectCropArea.Width = rectCropArea.Height;
                                            }
                                        }
                                        break;
                                    case ResizeType.Right:
                                        if (rectCropArea.Width + (e.X - ptLast.X) >= minWidth)
                                        {
                                            rectCropArea.Width += e.X - ptLast.X;
                                            if (currentShape.ShapeType == ShapeType.Square || currentShape.ShapeType == ShapeType.Circle)
                                            {
                                                rectCropArea.Height = rectCropArea.Width;
                                            }
                                        }
                                        break;
                                    case ResizeType.Bottom:
                                        if (rectCropArea.Height + (e.Y - ptLast.Y) >= minHeight)
                                        {
                                            rectCropArea.Height += e.Y - ptLast.Y;
                                            if (currentShape.ShapeType == ShapeType.Square || currentShape.ShapeType == ShapeType.Circle)
                                            {
                                                rectCropArea.Width = rectCropArea.Height;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            // Update shape
                            currentShape.PRotate = ptCurrent;
                            currentShape.RectOriginal = rectCropArea;

                            // Draw resize boxs
                            this.InitialResizeBox(currentShape);
                        }
                        else
                        {
                            // Hanlder move shape
                            if (currentShape != null)
                            {
                                rectCropArea.X += e.X - ptLast.X;
                                rectCropArea.Y += e.Y - ptLast.Y;

                                // Update rectangle
                                currentShape.RectOriginal = rectCropArea;
                            }

                            if (currentShape.BoundingBoxs != null && currentShape.BoundingBoxs.Count > 0)
                            {
                                for (int i = 0; i < currentShape.BoundingBoxs.Count; i++)
                                {
                                    Rectangle rectTmp = currentShape.BoundingBoxs[i].RectOriginal;
                                    rectTmp.X += e.X - ptLast.X;
                                    rectTmp.Y += e.Y - ptLast.Y;
                                    currentShape.BoundingBoxs[i].RectOriginal = rectTmp;
                                }
                            }
                        }

                        // Update last point.
                        ptLast = ptCurrent;
                    }
                }
                else
                {
                    if (currentShape != null)
                    {
                        rectCropArea = currentShape.RectOriginal;

                        if (currentShape.ShapeType == ShapeType.RectanleRotate)
                        {
                            var dX = ptCurrent.X - ptOriginal.X;
                            var dY = ptCurrent.Y - ptOriginal.Y;
                            int distance = (int)Math.Sqrt(dX * dX + dY * dY);
                            double radianB = Math.Atan((double)dX / dY);
                            var alpha = (float)(radianB * (180 / Math.PI));
                            
                            rectCropArea.X = ptOriginal.X - distance;
                            rectCropArea.Y = ptOriginal.Y - rectCropArea.Height / 2;
                            rectCropArea.Width = distance * 2;

                            currentShape.AngleRotate = 90 - alpha;
                            currentShape.PRotate = ptCurrent;
                        }
                        else
                        {
                            // Draw shape
                            if (e.X > ptOriginal.X && e.Y > ptOriginal.Y)
                            {
                                rectCropArea.Width = e.X - ptOriginal.X;
                                rectCropArea.Height = e.Y - ptOriginal.Y;
                            }
                            else if (e.X < ptOriginal.X && e.Y > ptOriginal.Y)
                            {
                                rectCropArea.Width = ptOriginal.X - e.X;
                                rectCropArea.Height = e.Y - ptOriginal.Y;
                                rectCropArea.X = e.X;
                                rectCropArea.Y = ptOriginal.Y;
                            }
                            else if (e.X > ptOriginal.X && e.Y < ptOriginal.Y)
                            {
                                rectCropArea.Width = e.X - ptOriginal.X;
                                rectCropArea.Height = ptOriginal.Y - e.Y;
                                rectCropArea.X = ptOriginal.X;
                                rectCropArea.Y = e.Y;
                            }
                            else
                            {
                                rectCropArea.Width = ptOriginal.X - e.X;
                                rectCropArea.Height = ptOriginal.Y - e.Y;
                                rectCropArea.X = e.X;
                                rectCropArea.Y = e.Y;
                            }

                            if (currentShape.ShapeType == ShapeType.Square || currentShape.ShapeType == ShapeType.Circle)
                            {
                                rectCropArea.Width = Math.Max(rectCropArea.Width, rectCropArea.Height);
                                rectCropArea.Height = Math.Max(rectCropArea.Width, rectCropArea.Height);
                            }
                        }
                        
                        // Update rectangle
                        currentShape.RectOriginal = rectCropArea;

                        // Update last point.
                        ptLast = ptCurrent;
                    }
                }

                SrcPicBox.Refresh();
            }
            else
            {
                if (bHaveSelect)
                {
                    //Update cusor
                    var resizeBoxInside = this.FindResizeBoxInsidePoint(this.shapes.FirstOrDefault(x => x.Index == currentIndex), ptCurrent);
                    if (resizeBoxInside != null)
                    {
                        this.UpdateCursor(resizeBoxInside.ResizeType);
                        return;
                    }
                }

                var shapeInside = this.FindShapeInPoint(ptCurrent);
                if (shapeInside != null)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void SrcPicBox_Paint(object sender, PaintEventArgs e)
        {
            this.Draw(e.Graphics);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            shapes.Clear();
            shapes.ForEach(x => x.BoundingBoxs.Clear());
            index = 0;
            currentIndex = -1;
            SrcPicBox.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Prepare a new Bitmap on which the cropped image will be drawn
            Bitmap sourceBitmap = new Bitmap(SrcPicBox.Image, SrcPicBox.Width, SrcPicBox.Height);

            foreach (var shape in shapes)
            {
                this.SaveImage(sourceBitmap, shape);
            }

            //Good practice to dispose the System.Drawing objects when not in use.
            sourceBitmap.Dispose();
        }

        private void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            if (shapes != null && shapes.Count > 0)
            {
                foreach (var item in shapes)
                {
                    using (Matrix m = new Matrix())
                    {
                        m.RotateAt(item.AngleRotate, new PointF(item.RectOriginal.X + (item.RectOriginal.Width / 2), item.RectOriginal.Y + (item.RectOriginal.Height / 2)));
                        g.Transform = m;

                        switch (item.ShapeType)
                        {
                            case ShapeType.Rectangle:
                            case ShapeType.Square:
                                g.DrawRectangle(penShape, item.RectOriginal);
                                break;
                            case ShapeType.Ellipse:
                            case ShapeType.Circle:
                                g.DrawEllipse(penShape, item.RectOriginal);
                                break;
                            case ShapeType.RectanleRotate:
                                g.DrawRectangle(penShape, item.RectOriginal);
                                break;
                            default:
                                g.DrawRectangle(penShape, item.RectOriginal);
                                break;
                        }

                        if (item != null && item.BoundingBoxs != null && item.BoundingBoxs.Count > 0)
                        {
                            item.BoundingBoxs.ForEach(x =>
                            {
                                g.DrawRectangle(penResize, x.RectOriginal);
                                g.FillRectangle(new SolidBrush(Color.White), x.RectOriginal);
                            });
                        }

                        g.ResetTransform();
                    }
                }
            }
        }

        private void SaveImage(Bitmap sourceBitmap, Shape shape)
        {
            var sourceBitmapRotate = RotateImage(sourceBitmap, -1 * shape.AngleRotate, new Point(shape.RectOriginal.X + shape.RectOriginal.Width / 2, shape.RectOriginal.Y + shape.RectOriginal.Height / 2));

            Rectangle destRect = new Rectangle(0, 0, shape.RectOriginal.Width, shape.RectOriginal.Height);
            var _editedImage = new Bitmap(destRect.Width, destRect.Height);

            Graphics g = Graphics.FromImage(_editedImage);
            g.SmoothingMode = SmoothingMode.HighQuality;

            if (shape.ShapeType == ShapeType.Circle || shape.ShapeType == ShapeType.Ellipse)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(destRect);
                g.SetClip(path);
            }

            g.DrawImage(sourceBitmapRotate, destRect, shape.RectOriginal, GraphicsUnit.Pixel);
            _editedImage.Save(string.Format(@"C:\Users\Public\Pictures\Sample Pictures\CropImage\image_{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmssffff")));
            g.Dispose();
        }

        public static Image RotateImage(Image img, float rotationAngle, Point point)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            using (Matrix m = new Matrix())
            {
                m.RotateAt(rotationAngle, point);
                gfx.Transform = m;
                gfx.DrawImage(img, new Point(0, 0));
                gfx.ResetTransform();
                gfx.Dispose();
            }

            return bmp;
        }

        private ShapeType GetShapeTypeSelect()
        {
            ShapeType shapeType = ShapeType.Rectangle;
            if (rdRectangle.Checked)
            {
                shapeType = ShapeType.Rectangle;
            }
            else if (rdSquare.Checked)
            {
                shapeType = ShapeType.Square;
            }
            else if (rdEclipse.Checked)
            {
                shapeType = ShapeType.Ellipse;
            }
            else if (rdCircle.Checked)
            {
                shapeType = ShapeType.Circle;
            } 
            else if (rdRectangleRotate.Checked)
            {
                shapeType = ShapeType.RectanleRotate;
            }

            return shapeType;
        }

        private void UpdateCursor(ResizeType resizeType)
        {
            switch (resizeType)
            {
                case ResizeType.Left:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case ResizeType.Top:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case ResizeType.Right:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case ResizeType.Bottom:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                default:
                    Cursor.Current = Cursors.Default;
                    break;
            }
        }

        private Shape FindShapeInPoint(Point currentPoint)
        {
            if (shapes != null && shapes.Count > 0)
            {
                // Foreach reveser list
                for (int i = shapes.Count - 1; i >= 0; i--)
                {
                    var currentShape = shapes[i];
                    var originalPoint = new Point(currentShape.RectOriginal.X + currentShape.RectOriginal.Width / 2, currentShape.RectOriginal.Y + currentShape.RectOriginal.Height / 2);
                    var rotatePoint = this.FindPointRotate(currentPoint, originalPoint, -1 * currentShape.AngleRotate);

                    // Check point inside bounding
                    if (this.CheckPointInsideBouding(rotatePoint, currentShape.RectOriginal))
                    {
                        return currentShape;
                    }
                }
            }
            return null;
        }

        private ResizeBox FindResizeBoxInsidePoint(Shape shape, Point currentPoint)
        {
            if (bHaveSelect)
            {
                if (shape != null && shape.BoundingBoxs != null && shape.BoundingBoxs.Count > 0)
                {
                    // Foreach reveser list
                    for (int i = shape.BoundingBoxs.Count - 1; i >= 0; i--)
                    {
                        var boudingBox = shape.BoundingBoxs[i];
                        var rectBouding = boudingBox.RectOriginal;
                        var originalPoint = new Point(shape.RectOriginal.X + shape.RectOriginal.Width / 2, shape.RectOriginal.Y + shape.RectOriginal.Height / 2);
                        var rotatePoint = this.FindPointRotate(currentPoint, originalPoint, -1 * shape.AngleRotate);

                        int xBound = boudingBox.RectOriginal.X;
                        int yBound = boudingBox.RectOriginal.Y;
                        int wBound = boudingBox.RectOriginal.Width;
                        int hBound = boudingBox.RectOriginal.Height;

                        switch (boudingBox.ResizeType)
                        {
                            case ResizeType.Left:
                                xBound -= mOusideBox;
                                yBound -= mOusideBox;
                                wBound += mOusideBox;
                                hBound += 2 * mOusideBox;
                                break;
                            case ResizeType.Top:
                                xBound -= mOusideBox;
                                yBound -= mOusideBox;
                                wBound += 2 * mOusideBox;
                                hBound += mOusideBox;
                                break;
                            case ResizeType.Right:
                                yBound -= mOusideBox;
                                wBound += mOusideBox;
                                hBound += 2 * mOusideBox;
                                break;
                            case ResizeType.Bottom:
                                xBound -= mOusideBox;
                                wBound += 2 * mOusideBox;
                                hBound += mOusideBox;
                                break;
                            default:
                                break;
                        }

                        // Check point inside bounding
                        if (this.CheckPointInsideBouding(rotatePoint, new Rectangle(xBound, yBound, wBound, hBound)))
                        {
                            return boudingBox;
                        }
                    }
                }
            }
            return null;
        }

        private void InitialResizeBox(Shape shape)
        {
            if (shapes != null && shape != null)
            {
                // Draw resize
                shapes.ForEach(x => x.BoundingBoxs.Clear());
                var rect = shape.RectOriginal;
                shape.BoundingBoxs.Add(new ResizeBox { RectOriginal = new Rectangle(rect.X - mResizeBox, rect.Y + rect.Height / 2 - mResizeBox, 2 * mResizeBox, 2 * mResizeBox), ResizeType = ResizeType.Left });
                shape.BoundingBoxs.Add(new ResizeBox { RectOriginal = new Rectangle(rect.X + rect.Width / 2 - mResizeBox, rect.Y - mResizeBox, 2 * mResizeBox, 2 * mResizeBox), ResizeType = ResizeType.Top });
                shape.BoundingBoxs.Add(new ResizeBox { RectOriginal = new Rectangle(rect.X + rect.Width - mResizeBox, rect.Y + rect.Height / 2 - mResizeBox, 2 * mResizeBox, 2 * mResizeBox), ResizeType = ResizeType.Right });
                shape.BoundingBoxs.Add(new ResizeBox { RectOriginal = new Rectangle(rect.X + rect.Width / 2 - mResizeBox, rect.Y + rect.Height - mResizeBox, 2 * mResizeBox, 2 * mResizeBox), ResizeType = ResizeType.Bottom });
            }
        }

        private Point FindPointRotate(Point currentPoint, Point originalPoint, double alpha)
        {
            var radian = Math.PI * (alpha / 180);
            var rotatePoint = currentPoint;
            rotatePoint.X = (int)((currentPoint.X - originalPoint.X) * Math.Cos(radian) - (currentPoint.Y - originalPoint.Y) * Math.Sin(radian) + originalPoint.X);
            rotatePoint.Y = (int)((currentPoint.X - originalPoint.X) * Math.Sin(radian) + (currentPoint.Y - originalPoint.Y) * Math.Cos(radian) + originalPoint.Y);
            return rotatePoint;
        }

        private bool CheckPointInsideBouding(Point point, Rectangle rectBouding)
        {
            if (point.X > rectBouding.X && point.X < rectBouding.X + rectBouding.Width && point.Y > rectBouding.Y && point.Y < rectBouding.Y + rectBouding.Height)
            {
                return true;
            }
            return false;
        }
    }
}
