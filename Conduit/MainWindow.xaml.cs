using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Linq; // added
using System.IO;
using Conduit;
using System;

namespace Conduit
{
    [Category("Highlights")]
    [Description("HousePlan is a continuation of the NodesEditor project, which includes more complex UI elements, movable connection points, curved connections, and the ability to expand/collapse nodes.")]
    //[RelatedUrl("http://stackoverflow.com/questions/15819318/how-to-create-and-connect-custom-user-buttons-controls-with-lines-using-windows", "How to create and connect custom user buttons/controls with lines using windows forms")]
    public partial class MainWindow
    {
        public List<Node> Nodes { get; set; }
        public List<Node2> Nodes2 { get; set; }
        public List<Connector> Connectors { get; set; }
        public List<Node> NodesThatExist { get; set; }

        private string User = "";
        private string Password = "";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            updateNodes();
            populateAvailableNodes();
            
        }

        //Ability to drag and drop Software Nodes
        private void Thumb_Drag(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as Node;
            if (node == null)
                return;
            
            node.Location.Value = Point.Add(node.Location.Value, new Vector(e.HorizontalChange, e.VerticalChange));

        }
        //Ability to drag and drop data nodes
        private void Thumb_Drag2(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node2 = thumb.DataContext as Node2;
            if (node2 == null)
            {
                MessageBox.Show("Node2 null");
                return;
            }
            node2.Location.Value = Point.Add(node2.Location.Value, new Vector(e.HorizontalChange, e.VerticalChange));

        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var listbox = sender as ListBox;

            if (listbox == null)
                return;

            var vm = listbox.DataContext as MainViewModel;

            if (vm == null)
                return;

            if (vm.SelectedObject != null && vm.SelectedObject is Node && vm.SelectedObject.IsNew)
            {
                vm.SelectedObject.Location.Value = e.GetPosition(listbox);
            }
            else if (vm.SelectedObject != null && vm.SelectedObject is Connector && vm.SelectedObject.IsNew)
            {
                var node = GetNodeUnderMouse();
                if (node == null)
                    return;

                var connector = vm.SelectedObject as Connector;
            }
        }

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                if (vm.CreatingNewConnector)
                {
                    var node = GetNodeUnderMouse();
                    var connector = vm.SelectedObject as Connector;
                    if (node != null && connector != null && connector.Start == null)
                    {
                        node.IsHighlighted = true;
                        
                        e.Handled = true;
                        return;
                    }
                }

                if (vm.SelectedObject != null)
                    vm.SelectedObject.IsNew = false;

                vm.CreatingNewNode = false;
                vm.CreatingNewConnector = false;
            }
        }

        private Node GetNodeUnderMouse()
        {
            var item = Mouse.DirectlyOver as Border;
            if (item == null)
                return null;

            return item.DataContext as Node;
        }

        private void MidPoint_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var connector = thumb.DataContext as Connector;
            if (connector == null)
                return;

            connector.MidPoint.Value = Point.Add(connector.MidPoint.Value, new Vector(e.HorizontalChange, e.VerticalChange));
        }

        private void Snap_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var snap = thumb.DataContext as SnapSpot;
            if (snap == null)
                return;
            snap.Offset.Value = Point.Add(snap.Offset.Value, new Vector(snap.LockX ? 0 : e.HorizontalChange / 1000, snap.LockY ? 0 : e.VerticalChange / 1000));
        }

       
        //Deletes the selected element 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            Node n = vm.SelectedObject as Node;
            List<Connector> connectors = vm.Connectors.ToList();
            if (n == null)
            {
            }
            //Deletes a Software Node
            else
            {
                //Deletes its input SnapSpots
                foreach (SnapSpot s in n.InSnaps.Values)
                {
                    if (s.IsConnected)
                    {
                       foreach(Connector connector in connectors)
                        {
                            if (connector.End == s)
                            {
                                //deletes the connector
                                deleteConnector(connector);
                            }
                        }
                    }
                    //removes snaps
                    vm.Snaps.Remove(s);
                    
                }
                connectors = vm.Connectors.ToList();
                //deletes it output SnapSpots
                foreach (SnapSpot s in n.OutSnaps.Values)
                {
                    if (s.IsConnected)
                    {
                        foreach (Connector connector in connectors)
                        {
                            if (connector.Start == s)
                            {
                                //deletes the connector
                                deleteConnector(connector);
                            }
                        }
                    }
                    //removes snaps
                    vm.Snaps.Remove(s);
                }
                //removes the software node
                vm.Nodes.Remove(n);
                updateNodes();
            }

            Node2 n2 = vm.SelectedObject as Node2;
            if(n2 == null)
            {
            }
            //Deleting a Data Node
            else
            {
                foreach (SnapSpot s in n2.Snaps)
                {
                    if (s.IsConnected)
                    {
                        foreach (Connector connector in connectors)
                        {
                            if (connector.Start == s || connector.End ==s)
                            {
                                //deletes Connectors
                                deleteConnector(connector);
                            }
                        }
                    }
                    //Removes snaps
                    vm.Snaps.Remove(s);
                }
               //removes Data Node
                vm.Nodes2.Remove(n2);
            }

            Connector c = vm.SelectedObject as Connector;
            if (c == null)
            {
            }
            //deleting connector
            else
            {
                //deletes connector
                deleteConnector(c);
            }
            updateNodes();
           
        }
        //Method to delete a connector
        public void deleteConnector(Connector c)
        {
            var vm = DataContext as MainViewModel;
            SnapSpot end = c.End;
            SnapSpot start = c.Start;
            //Sets the SnapSpots to be not connected
            c.End.IsConnected = false;
            c.Start.IsConnected = false;
            //Removes the Connector
            vm.Connectors.Remove(c);
            //If that snap is being used elsewhwere it is set back to being connected
            foreach (Connector x in vm.Connectors)
            {
                if (x.Start == start)
                {
                    x.Start.IsConnected = true;
                }
                if (x.End == end)
                {
                    x.End.IsConnected = true;
                }
            }
            updateNodes();
        }

        //Opens form for the Create new Software Node
        private void Button_Click_1(object sender, RoutedEventArgs e)
        { 
            SoftwareNodeCreator nc = new SoftwareNodeCreator(this);
            nc.Show();
            
        }

        //Updates the variables in this class to match the master list in Main Window
        public void updateNodes()
        {
            var vm = DataContext as MainViewModel;
            List<Node> a = vm.Nodes.ToList();
            List<Connector> b = vm.Connectors.ToList();
            List<Node2> c = vm.Nodes2.ToList();
            NodesThatExist = vm.NodesThatExist;
            Nodes = a;
            Connectors = b;
            Nodes2 = c;

        }
        //Opens the form to add a new connector
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            newConnector c = new newConnector(this);
            c.Show();
        }
        //Opens the form to sign into Beocat
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Beocat b = new Beocat(this);
            b.Show();
        }
        //opens the form to save/load a file
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            
            SaveFile f = new SaveFile(this);
            f.Show();
        }
        //Button click to clear the entire screen
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            clearScreen();
        }
        //Method to Clear Screen
        public void clearScreen()
        {
            var vm = DataContext as MainViewModel;
            //removes Nodes
            vm.Nodes.ToList().ForEach(x => vm.Nodes.Remove(x));
            //Sets SnapSpots to not connected
            List<SnapSpot> snaps = vm.Snaps.ToList();
            foreach (SnapSpot s in snaps)
            {
                s.IsConnected = false;
            }
            //deletes SnapSpots
            vm.Snaps.ToList().ForEach(x => vm.Snaps.Remove(x));
            //deletes Connectors
            vm.Connectors.ToList().ForEach(x => vm.Connectors.Remove(x));
            //deletes DataNodes
            vm.Nodes2.ToList().ForEach(x => vm.Nodes2.Remove(x));
            updateNodes();
        }
        //Opens form to add a connectors
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            CreateDataNode cn = new CreateDataNode(this);
            cn.Show();
        }

        private void Button_Click_X(object sender, RoutedEventArgs e)
        {
            string cwd = Directory.GetCurrentDirectory();
            string[] cwdsplit = cwd.Split('\\');
            string loadDataDir = "";
            for (int i = 0; i < cwdsplit.Length - 2; i++)
            {
                loadDataDir += cwdsplit[i] + '\\';
            }
            loadDataDir += "data\\";
            /*
            string masterLoc = loadDataDir + "KrakenM.txt";
            string parLoc = loadDataDir + "KrakenP.txt";
            string inTups = "pipePath,/homes/mgheffel/SDP;input,/bulk/mgheffel/data/SDP/raw_cleaned;database,/bulk/bioinfo/vdl/.dependencies/krakenDBs/DB21";
            string pTups = "#runTime,00-18:00:00";
            ScriptCreator sc = new ScriptCreator(masterLoc, parLoc, pTups, inTups);
            File.WriteAllText(loadDataDir + "KrakenM.sh", sc.compileMasterScript());
            File.WriteAllText(loadDataDir + "KrakenP.sh", sc.compileParallelScript());*/

            var vm = DataContext as MainViewModel;
            vm.user = user;
            vm.pass = password;
            compilePipeline cp = new compilePipeline(vm);
            cp.Show();
        }

        //Adds selected software node to the screen
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            string[] skeleton = vm.NodeSkeletons[ListOfNodes.SelectedItem.ToString()];
            //creates the string needed to create the node
            string[] strings = new string[(skeleton.Length - 3)];
            for (int j = 0; j < strings.Length; j++)
            {
                strings[j] = skeleton[(j + 3)];
            }

            string name = strings[0];
            int nameCount = 0;
            bool flag = false;
            do
            {
                flag = false;
                for (int i = 0; i < vm.Nodes.Count; i++)
                {
                    if (vm.Nodes[i].Name == name)
                    {
                        flag = true;
                        nameCount += 1;
                        name = name.Split('-')[0];
                        name = name + '-' + nameCount.ToString();
                        break;
                    }
                }
            } while (flag);
            strings[0] = name;
            //creates the node
            Node n = vm.CreateNewNode(Convert.ToInt32(skeleton[2]), 10, strings);
            n.Location.Value = new System.Windows.Point(Convert.ToInt32(skeleton[0]), Convert.ToInt32(skeleton[1]));
            
            //adds it to list of shown nodes
            vm.viewNodes(n);
            updateNodes();
        }
        //populates already existing software nodes from the file in Conduit/data/ExistingNodes
        public void populateAvailableNodes()
        {
            var vm = DataContext as MainViewModel;
            ListOfNodes.Items.Clear();
            
            //gets directory
            string cwd = Directory.GetCurrentDirectory();
            string[] cwdsplit = cwd.Split('\\');
            string loadDataDir = "";
            for (int i = 0; i < cwdsplit.Length - 2; i++)
            {
                loadDataDir += cwdsplit[i] + '\\';
            }
            loadDataDir += "data\\ExistingNodes\\ExistingNodes.txt";
            //reads in the nodes from the file
            string[] filelines = File.ReadAllLines(loadDataDir);
            //for each node it adds it to the skeleton list of nodes
            for (int k = 1; k < filelines.Length; k++)
            {
                string[] nodeString = filelines[k].Split(',');
                string[] strings = new string[(nodeString.Length - 3)];
                for (int j = 0; j < strings.Length; j++)
                {
                    strings[j] = nodeString[(j + 3)];
                }
                vm.NodeSkeletons.Add(nodeString[3], nodeString);
                //adds the name to the list of available software nodes
                ListOfNodes.Items.Add(nodeString[3]);
                ListOfNodes.SelectedIndex = 0;
            }
        }

        //writes a newly created software node to the Conduit\data\ExistingNodes file
        public string writeNode(Node n)
        {
            string cwd = Directory.GetCurrentDirectory();
            string[] cwdsplit = cwd.Split('\\');
            string loadDataDir = "";
            for (int i = 0; i < cwdsplit.Length - 2; i++)
            {
                loadDataDir += cwdsplit[i] + '\\';
            }
            loadDataDir += "data\\ExistingNodes\\ExistingNodes.txt";
            //Writes the necessary information of the node to a string
            string one = "";
                one = one + n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + "," + n.V2.ToString() + "," + n.V3.ToString() + "," + n.V4.ToString() + "," + n.V5.ToString() + "," + n.V6.ToString() + "," + n.V7.ToString() + "," + n.V8.ToString() + "," + n.V9.ToString() + "," + n.V10.ToString() + ","
                    + n.T1.ToString() + "," + n.T2.ToString() + "," + n.T3.ToString() + "," + n.T4.ToString() + "," + n.T5.ToString() + "," + n.T6.ToString() + "," + n.T7.ToString() + "," + n.T8.ToString() + "," + n.T9.ToString() + "," + n.T10.ToString();
            //writes the necessary information on the software nodes snaps
            foreach (var item in n.InSnaps)
            {
                one = one + "," + item.Value.Name.ToString();
            }
            foreach (var item in n.OutSnaps)
            {
                one = one + "," + item.Value.Name.ToString();
            }
            //appends the new software node to the file
            using (StreamWriter sw = File.AppendText(loadDataDir))
            {
                sw.WriteLine(one);
            }
            //adds the name to the list of available software nodes
            ListOfNodes.Items.Add(n.Name);
            return one;
        }
        //Opens form to FTP a zipped file into Beocat or to zip a directory
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            FTP ftp = new FTP(this);
            ftp.Show();
        }


        public string password
        {
            get { return Password; }
            set
            {
                if (Password != value)
                {
                    Password = value;
                    
                }
            }
        }
        public string user
        {
            get { return User; }
            set
            {
                if (User!= value)
                {
                    User = value;

                }
            }
        }

        //Opens window that runs a kstat -- me and returns the result to a textbox
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            CheckProgress cp = new CheckProgress(this);
            cp.Show();
        }
    }
}
