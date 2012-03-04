using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Xml;
using System.Xml.Serialization;
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

        #region things that may or not be used (not currently in use)
        public Tree tree { get; set; }

        /// <summary>
        /// this is something that didnt work out for me but im going to keep it here until we know that we arent going to use it
        /// </summary>
        /// <param name="tree"></param>
        static public void SerializeToXML(Tree tree)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Tree));
            TextWriter textWriter = new StreamWriter(@"C:\Users\Chris\Desktop\xmlSerializer.xml");
            serializer.Serialize(textWriter, tree);
            textWriter.Close();
        }
        #endregion

        #region Things im Currently using
        private XmlTextWriter xr;
        
        /// <summary>
        /// populates a tree view from an xml file... calls a recursive function to finish it
        /// </summary>
        private void populateTreeview()
        {
            // this try catch will try to populate the tree from the xml file but
            // if there is no file then it will populate the tree based on the project structure from the computer
            try
            {
                //load the tree that was serialized previously
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("xmlSerializerd.xml");
                //dont think this is needed since this will be run on load
                treeView1.Items.Clear();
                //adds the root of the tree
                TreeViewItem root = new TreeViewItem() { Header = xDoc.DocumentElement.Name, Name = xDoc.DocumentElement.Name };
                treeView1.Items.Add(root);
                //recursive function to populate the rest of the tree
                addTreeNode(xDoc.DocumentElement, root);
            }
            catch (XmlException xExc)
            //Exception is thrown is there is an error in the Xml
            {
                MessageBox.Show(xExc.Message);
            }
            // if there is no file then it will load it from the computer
            catch (FileNotFoundException)
            {
                DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                // TODO : this might have to change to a better way to find the project root. 
                di = di.Parent.Parent.Parent;
                List<string> registeredNames = new List<string>();
                //sets the root of the tree
                var item = new TreeViewItem() { Header = di.Name, Name = di.Name };
                treeView1.Items.Add(item);
                item.RegisterName(di.Name, item);
                registeredNames.Add(di.Name);
                MessageBox.Show("I couldnt find the file so im mimicing the structure from the computer");
                treeView1 = getDirectories(di, di.Name, treeView1, ref registeredNames);
            }
            catch (Exception ex) //General exception
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This function is called recursively until all nodes are loaded. called by populateTreeView
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="item"></param>
        private void addTreeNode(XmlNode xmlNode, TreeViewItem item)
        {
            XmlNode xNode;
            XmlNodeList xNodeList;
            if (xmlNode.HasChildNodes) //The current node has children
            {
                xNodeList = xmlNode.ChildNodes;
                for (int x = 0; x <= xNodeList.Count - 1; x++)
                //Loop through the child nodes
                {
                    xNode = xmlNode.ChildNodes[x];
                    TreeViewItem child = new TreeViewItem() { Header = xNode.Name, Name = xNode.Name };
                    item.Items.Add(child);
                    addTreeNode(xNode, child);
                }
            }
        }
        /// <summary>
        /// initial method call that serializes a tree to an xml file.. calls a recursive call to finish it
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="filename"></param>
        public void xmlSerialize(TreeView tv, string filename)
        {
            xr = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            xr.WriteStartDocument();
            //Write our root node
            TreeViewItem root = tv.Items[0] as TreeViewItem;
            xr.WriteStartElement(root.Name);
            foreach (TreeViewItem node in tv.Items)
            {
                mapNodes(node.Items);
            }
            //Close the root node
            xr.WriteEndElement();
            xr.Close();
        }
        /// <summary>
        /// recursive method that serializes a treeview to an xml file.. called by xmlSerialize
        /// </summary>
        /// <param name="items"></param>
        private void mapNodes(ItemCollection items)
        {
            foreach (TreeViewItem item in items)
            {
                //if there are children call recursive function again
                if (item.Items.Count > 0)
                {
                    xr.WriteStartElement(item.Name);
                    mapNodes(item.Items);
                    xr.WriteEndElement();
                }
                else //No child nodes, so we just add this node and pop out
                {
                    xr.WriteStartElement(item.Name);
                    xr.WriteEndElement();
                }
            }
        }
        #endregion
        
        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // method used to populate the treeview.. obviously
            populateTreeview();

            #region calls to things that arent being used currently
            //Tree myTree = new Tree();
            //myTree.treeview = "bullshit";
            //SerializeToXML(myTree);
            #endregion
            // this is commented out but this is how i serialized the tree the first time.
            // this is here for now just to get it working but it will not be here in the final
            //xmlSerialize(treeView1, "xmlSerializer.xml");

        }
        /// <summary>
        /// each xml element needs a unique name so this was my way of givin them that. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="names"></param>
        /// <returns></returns>
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
        /// <summary>
        /// this is the function that maps the directories from the computer recursively
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="parentNode"></param>
        /// <param name="tree"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public TreeView getDirectories(DirectoryInfo directory, string parentNode, TreeView tree, ref List<string> names)
        {
            // TODO : Maybe the name of the treeviewitems should be the fullpath? that ensures its unique and we could later
            // use the names (full paths) in order to find the file quickly for TFS stuff
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

        private void MyToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //treeView1 = tree.treeview;
        }
       


        
    }
}