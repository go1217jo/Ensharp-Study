using System;
using System.Windows;
using System.Windows.Controls;

namespace WindowExplorer
{
    public class ResizablePanel : ContentControl
    {
        static ResizablePanel()
        {
            // This will allow us to create a Style in Generic.xaml with target type ResizablePanel.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizablePanel), new FrameworkPropertyMetadata(typeof(ResizablePanel)));
        }
    }
}
