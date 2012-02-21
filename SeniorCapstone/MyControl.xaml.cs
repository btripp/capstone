using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Client;



namespace AugustaStateUniversity.SeniorCapstone
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    /// 
    public partial class MyControl : UserControl
    {

        
        
        public MyControl()
        {
            InitializeComponent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TreeView myTree = treeView1;
            //Visual myVisual = treeView1;
            //Visual childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, 0);
            //void WalkDownLogicalTree(object current)
            //{
            //    DoSomethingWithObjectInLogicalTree(current);

            //    // The logical tree can contain any type of object, not just 
            //    // instances of DependencyObject subclasses.  LogicalTreeHelper
            //    // only works with DependencyObject subclasses, so we must be
            //    // sure that we do not pass it an object of the wrong type.
            //    DependencyObject depObj = current as DependencyObject;

            //    if (depObj != null)
            //        foreach(object logicalChild in LogicalTreeHelper.GetChildren(depObj))
            //            WalkDownLogicalTree(logicalChild);
            //}
            TreeViewItem node = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(myTree, "three");
            int i = 0;
            string msg = "";
            foreach (TreeViewItem nodes in node.Items)
            {
                i++;
                if (nodes.HasItems)
                {
                    msg += " AND it has kids!";
                }
                MessageBox.Show("child "+i+" is "+nodes.Header.ToString()+"."+ msg);
            }
            

        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            
        }
       


        
    }
}