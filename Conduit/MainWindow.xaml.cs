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

        //private int n = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            updateNodes();
            populateAvailableNodes();
            
        }

        private void Thumb_Drag(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as Node;
            if (node == null)
                return;
            
            node.Location.Value = Point.Add(node.Location.Value, new Vector(e.HorizontalChange, e.VerticalChange));
            //node.Location.Value = Point.Add(node.Location.Value, new Vector(e.HorizontalChange, e.VerticalChange));

        }
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

                /*if (connector.Start != null && node != connector.Start)
                    connector.End = node;*/
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
                        //connector.Start = node;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            string[] a = {"Node" +(Nodes.Count +1), "3", "4", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            string[] b = { "Output1", "Output2", "Output3" };
            string[] c = { "Input1", "Input2", "Input3", "Input4" };
            Node node = vm.CreateNewNode(5,5, a);//,b,c);

            updateNodes();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { 
            NodeCreator nc = new NodeCreator(this);
            nc.Show();
            
        }

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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            newConnector c = new newConnector(this);
            c.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Beocat b = new Beocat();
            b.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            
            SaveFile f = new SaveFile(this);
            f.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.Nodes.ToList().ForEach(x => vm.Nodes.Remove(x));
            vm.Snaps.ToList().ForEach(x => vm.Snaps.Remove(x));
            vm.Connectors.ToList().ForEach(x => vm.Connectors.Remove(x));
            vm.Nodes2.ToList().ForEach(x => vm.Nodes2.Remove(x));
            updateNodes();


        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            CreateNode2 cn = new CreateNode2(this);
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
            /*string masterLoc = loadDataDir + "SeqPurgeM.txt";
            string parLoc = loadDataDir + "SeqPurgeP.txt";

            string inTups = "adapterFile,$HERE/dependencies/illumina_adapters.fa;readsInput,/bulk/mgheffel/data/SDP/raw";
            string pTups = "#runTime,00-02:00:00;#memPerCPU,32G";
            ScriptCreator sc = new ScriptCreator(masterLoc, parLoc, pTups, inTups);
            //MessageBox.Show(sc.compileMasterScript());
            //MessageBox.Show(sc.compileParallelScript());

            File.WriteAllText(loadDataDir + "SeqPurgeM.sh", sc.compileMasterScript());
            File.WriteAllText(loadDataDir + "SeqPurgeP.sh", sc.compileParallelScript());
            */
            string masterLoc = loadDataDir + "KrakenM.txt";
            string parLoc = loadDataDir + "KrakenP.txt";
            string inTups = "pipePath,/homes/mgheffel/SDP;input,/bulk/mgheffel/data/SDP/raw_cleaned;database,/bulk/bioinfo/vdl/.dependencies/krakenDBs/DB21";
            string pTups = "#runTime,00-18:00:00";
            ScriptCreator sc = new ScriptCreator(masterLoc, parLoc, pTups, inTups);
            File.WriteAllText(loadDataDir + "KrakenM.sh", sc.compileMasterScript());
            File.WriteAllText(loadDataDir + "KrakenP.sh", sc.compileParallelScript());
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            
            Node n = NodesThatExist[ListOfNodes.SelectedIndex];
            Nodes.Add(n);
            vm.viewNodes(n);
            updateNodes();
        }
        public void populateAvailableNodes()
        {
            foreach (Node n in NodesThatExist)
            {
                ListOfNodes.Items.Remove(n.Name);
            }
            
            string cwd = Directory.GetCurrentDirectory();
            string[] cwdsplit = cwd.Split('\\');
            string loadDataDir = "";
            for (int i = 0; i < cwdsplit.Length - 2; i++)
            {
                loadDataDir += cwdsplit[i] + '\\';
            }
            loadDataDir += "data\\ExistingNodes\\ExistingNodes.txt";

            string[] filelines = File.ReadAllLines(loadDataDir);

            for (int k = 1; k < filelines.Length; k++)
            {
                string[] nodeString = filelines[k].Split(',');
                string[] strings = new string[(nodeString.Length - 3)];
                for (int j = 0; j < strings.Length; j++)
                {
                    strings[j] = nodeString[(j + 3)];
                }

                var vm = DataContext as MainViewModel;
                Node n = vm.CreateNewNode(Convert.ToInt32(nodeString[2]), 10, strings);
                n.Location.Value = new System.Windows.Point(Convert.ToInt32(nodeString[0]), Convert.ToInt32(nodeString[1]));
                updateNodes();
            }
            foreach (Node n in NodesThatExist)
            {
                ListOfNodes.Items.Add(n.Name);
            }
        }
        public void writeNode(Node n)
        {
            string cwd = Directory.GetCurrentDirectory();
            string[] cwdsplit = cwd.Split('\\');
            string loadDataDir = "";
            for (int i = 0; i < cwdsplit.Length - 2; i++)
            {
                loadDataDir += cwdsplit[i] + '\\';
            }
            loadDataDir += "data\\ExistingNodes\\ExistingNodes.txt";

            string one = "";
                one = one + n.Location.Value.X.ToString() + "," + n.Location.Value.Y.ToString() + "," + n.Fields.ToString() + "," + n.Name.ToString() + "," + n.OutputSnaps.ToString() + "," + n.InputSnaps.ToString() + "," + n.V1.ToString() + "," + n.V2.ToString() + "," + n.V3.ToString() + "," + n.V4.ToString() + "," + n.V5.ToString() + "," + n.V6.ToString() + "," + n.V7.ToString() + "," + n.V8.ToString() + "," + n.V9.ToString() + "," + n.V10.ToString() + ","
                    + n.T1.ToString() + "," + n.T2.ToString() + "," + n.T3.ToString() + "," + n.T4.ToString() + "," + n.T5.ToString() + "," + n.T6.ToString() + "," + n.T7.ToString() + "," + n.T8.ToString() + "," + n.T9.ToString() + "," + n.T10.ToString();
                foreach (SnapSpot s in n.Snaps)
                {
                    one = one + "," + s.Name.ToString();
                }
           
            using (StreamWriter sw = File.AppendText(loadDataDir))
            {
                sw.WriteLine(one);
            }
            populateAvailableNodes();
        }
        
    }
}
