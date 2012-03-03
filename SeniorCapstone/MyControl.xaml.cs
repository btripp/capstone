using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
            DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            di = di.Parent.Parent.Parent;
            MessageBox.Show("The current directory is:\n"+ di);
            string message = "";
            #region commented out stuff
            //message = getDirectories(di, message);
            //if (di != null)
            //{
            //    FileInfo[] subFiles = di.GetFiles();
            //    if (subFiles.Length > 0)
            //    {
            //        message+="Files:\n";
            //        foreach (FileInfo subFile in subFiles)
            //        {
            //            message+="   " + subFile.Name + " (" + subFile.Length + " bytes)\n";
            //        }
            //    }
            //    //Console.ReadKey();
            //}
            //DirectoryInfo[] subDirs = di.GetDirectories();
            //if (subDirs.Length > 0)
            //{
            //    message+="\tSub-Directories:\n";
            //    foreach (DirectoryInfo subDir in subDirs)
            //    {
            //        message+="\t   " + subDir.Name+"\n";
            //        FileInfo[] subFiles = subDir.GetFiles();
            //        if (subFiles.Length > 0)
            //        {
            //            message+="\tFiles:\n";
            //            foreach (FileInfo subFile in subFiles)
            //            {
            //                message+="\t   " + subFile.Name + " (" + subFile.Length + " bytes)\n";
            //            }
            //        }
            //    }
            //}
            #endregion
            List<string> registeredNames = new List<string>();
            var item = new TreeViewItem() { Header = di.Name, Name = di.Name };
            treeView1.Items.Add(item); 
            item.RegisterName(di.Name, item);
            registeredNames.Add(di.Name);
            treeView1 = getDirectories(di,di.Name, treeView1, ref registeredNames);
            //var i2 = treeView1.FindName("Item2") as TreeViewItem;
            //var subitem = new TreeViewItem() { Header = "SubItem 1"};
            //i2.Items.Add(subitem);

            #region more commented out stuff
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
            //TreeViewItem node = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(myTree, "three");
            //int i = 0;
            //string msg = "";
            //foreach (TreeViewItem nodes in node.Items)
            //{
            //    i++;
            //    if (nodes.HasItems)
            //    {
            //        msg += " AND it has kids!";
            //    }
            //    MessageBox.Show("child "+i+" is "+nodes.Header.ToString()+"."+ msg);
            //}
            #endregion

        }

        public string uniqueName(string name, ref List<string> names)
        {
            string newName=name;
            int i = 1;
            // i dont know why it didnt like MyToolWindow but i add a 1 to the end of this just to get it to work
            if (name.Equals("MyToolWindow"))
            {
                newName = name + i.ToString();
            }
            while (names.Contains(newName))
            {
                newName = name + i.ToString();
                i++;
            }
            

            return newName;
        }

        public TreeView getDirectories(DirectoryInfo directory, string parentNode, TreeView tree, ref List<string> names)
        {
            // FIXED : have to pass the name of the parent node instead of using directory.Name because of the unique naming of the tree
            // elements. by passing the unique name we can find the correct parent node
            TreeViewItem parent = tree.FindName(parentNode) as TreeViewItem;
            // gets down to last directory
            DirectoryInfo[] dir = directory.GetDirectories();
            if (dir.Length > 0)
            {
                foreach (DirectoryInfo subDir in dir)
                {
                    // im just skipping over the git stuff for now. we can figure out how to deal with it later
                    if (!subDir.Name.Equals(".git"))
                    {
                        string header = subDir.Name;
                        string name = header.Split('.')[0];
                        // passes the name to a function that addes numbers to the end of the name to
                        // ensure the name is unique
                        name = uniqueName(name, ref names);
                        TreeViewItem item = new TreeViewItem() { Header = header, Name = name };
                        parent.Items.Add(item);
                        item.RegisterName(name, item);
                        names.Add(name);
                        getDirectories(subDir,name, tree, ref names);
                    }
                }
            }
            //map files to current level of tree
            FileInfo[] files = directory.GetFiles();
            if (files.Length > 0)
            {
                foreach (FileInfo file in files)
                {
                    string header = file.Name;
                    // this is more git stuff that we dont need to put in here but we might need to add more error stuff to catch weird
                    // names and such
                    if(header.Contains("git"))
                    {
                        header = header.Replace(".", "NotNeeded");
                    }
                    string name = header.Split('.')[0];
                    // passes the name to a function that addes numbers to the end of the name to
                    // ensure the name is unique
                    name = uniqueName(name, ref names);
                    TreeViewItem item = new TreeViewItem() { Header = header, Name = name };
                    parent.Items.Add(item);
                    item.RegisterName(name, item);
                    names.Add(name);
                }
            }
            return tree;
        }       

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            
        }
       


        
    }
}