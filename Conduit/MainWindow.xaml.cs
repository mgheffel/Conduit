using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Linq; // added
using System.IO;
using Conduit;

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

        //private int n = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            updateNodes();
            
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
            string masterLoc = loadDataDir + "SeqPurgeM.txt";
            string parLoc = loadDataDir + "SeqPurgeP.txt";

            string inTups = "adapterFile,$HERE/dependencies/illumina_adapters.fa;readsInput,/bulk/mgheffel/data/SDP/raw";
            string pTups = "#runTime,00-02:00:00;#memPerCPU,32G";
            ScriptCreator sc = new ScriptCreator(masterLoc, parLoc, pTups, inTups);
            //MessageBox.Show(sc.compileMasterScript());
            //MessageBox.Show(sc.compileParallelScript());

            File.WriteAllText(loadDataDir + "SeqPurgeM.sh", sc.compileMasterScript());
            File.WriteAllText(loadDataDir + "SeqPurgeP.sh", sc.compileParallelScript());
        }
    }
}
