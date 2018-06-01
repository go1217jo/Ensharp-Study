using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WindowExplorer
{
    class Resizer : Thumb
    {
        public static DependencyProperty ThumbDirectionProperty = DependencyProperty.Register("ThumbDirection", typeof(ResizeDirections), typeof(Resizer));

        public enum ResizeDirections
        {
            TopLeft = 0, Left, BottomLeft, Bottom,
            BottomRight, Right, TopRight, Top
        }

        public ResizeDirections ThumbDirection
        {
            get { return (ResizeDirections)GetValue(ThumbDirectionProperty); }
            set { SetValue(Resizer.ThumbDirectionProperty, value); }
        }

        static Resizer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Resizer), new FrameworkPropertyMetadata(typeof(Resizer)));
        }

        public Resizer()
        {
            DragDelta += new DragDeltaEventHandler(Resizer_DragDelta);
        }

        void Resizer_DragDelta(object sender, DragDeltaEventArgs e)
        {
             Control designerItem = this.DataContext as Control;

            if (designerItem != null)
            {
                double deltaHorizontal;

                switch(ThumbDirection)
                {
                    case ResizeDirections.TopLeft:
                    case ResizeDirections.Left:
                    case ResizeDirections.BottomLeft:
                        deltaHorizontal = ResizeLeft(e, designerItem);
                        break;
                    case ResizeDirections.BottomRight:
                    case ResizeDirections.Right:
                    case ResizeDirections.TopRight:
                        deltaHorizontal = ResizeRight(e, designerItem);
                        break;
                }
            }
            e.Handled = true;
        }

        private static double ResizeRight(DragDeltaEventArgs e, Control designerItem)
        {
            double deltaHorizontal = Math.Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
            designerItem.Width -= deltaHorizontal;
            if (designerItem.Width > 440)
                designerItem.Width = 440;
            return deltaHorizontal;
        }

        private static double ResizeLeft(DragDeltaEventArgs e, Control designerItem)
        {
            double deltaHorizontal = Math.Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
            Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaHorizontal);
            designerItem.Width -= deltaHorizontal;
            return deltaHorizontal;
        }
    }

}
