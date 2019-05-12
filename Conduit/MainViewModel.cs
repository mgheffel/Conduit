using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace Conduit
{

    public class MainViewModel : INotifyPropertyChanged
    {
        string conduitLocation;
        #region Collections

        private ObservableCollection<Node> _nodes;
        public ObservableCollection<Node> Nodes
        {
            get { return _nodes ?? (_nodes = new ObservableCollection<Node>()); }
        }
        private List<Node> _nodesThatExist;
        public List<Node> NodesThatExist
        {
            get { return _nodesThatExist ?? (_nodesThatExist = new List<Node>()); }
        }
        private Dictionary<string, string[]> _nodeSkeletons;
        public Dictionary<string, string[]> NodeSkeletons
        {
            get { return _nodeSkeletons ?? (_nodeSkeletons = new Dictionary<string, string[]>()); }
        }

        private ObservableCollection<Node2> _nodes2;
        public ObservableCollection<Node2> Nodes2
        {
            get { return _nodes2 ?? (_nodes2 = new ObservableCollection<Node2>()); }
        }

        private ObservableCollection<Connector> _connectors;
        public ObservableCollection<Connector> Connectors
        {
            get { return _connectors ?? (_connectors = new ObservableCollection<Connector>()); }
        }

        private ObservableCollection<SnapSpot> _snaps;
        public ObservableCollection<SnapSpot> Snaps
        {
            get { return _snaps ?? (_snaps = new ObservableCollection<SnapSpot>()); }
        }

        private DiagramObject _selectedObject;
        public DiagramObject SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                Nodes.ToList().ForEach(x => x.IsHighlighted = false);

                _selectedObject = value;
                OnPropertyChanged("SelectedObject");

                DeleteCommand.IsEnabled = value != null;

                var connector = value as Connector;
                ShowMidPointThumb = connector != null;
                if (connector != null)
                {
                    if (connector.Start != null)
                        connector.Start.IsHighlighted = true;

                    if (connector.End != null)
                        connector.End.IsHighlighted = true;
                }
                var snap = value as SnapSpot;
                if (snap != null)
                {
                    MessageBox.Show(snap.Name);
                    MessageBox.Show(snap.IsConnected.ToString());
                }
            }
        }

        #endregion

        #region Bool (Visibility) Options

        private bool _collapseAll;
        public bool CollapseAll
        {
            get { return _collapseAll; }
            set
            {
                _collapseAll = value;
                OnPropertyChanged("CollapseAll");

                Nodes.ToList().ForEach(x => x.IsCollapsed = value);
            }
        }

        private bool _showMidPointThumb;
        public bool ShowMidPointThumb
        {
            get { return _showMidPointThumb; }
            set
            {
                _showMidPointThumb = value;
                OnPropertyChanged("ShowMidPointThumb");
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            //_nodes = new ObservableCollection<Node>(NodesDataSource.GetRandomNodes());
            //_connectors = new ObservableCollection<Connector>(NodesDataSource.GetRandomConnectors(Nodes.ToList()));
            //_snaps = new ObservableCollection<SnapSpot>(Nodes.SelectMany(x => x.Snaps));
            string cwd = Directory.GetCurrentDirectory();
            string[] cwdsplit = cwd.Split('\\');
            string loadDataDir = "";
            for (int i = 0; i < cwdsplit.Length - 2; i++)
            {
                loadDataDir += cwdsplit[i] + '\\';
            }
            conduitLocation = loadDataDir;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Creating New Objects

        private bool _creatingNewNode;
        public bool CreatingNewNode
        {
            get { return _creatingNewNode; }
            set
            {
                _creatingNewNode = value;
                OnPropertyChanged("CreatingNewNode");
                string[] a = { "name", "3", "4", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                string[] b = { "Output1", "Output2", "Output3" };
                string [] c = { "Input1", "Input2", "Input3", "Input4" };
                if (value)
                    CreateNewNode(5,5, a);//,b,c);
                else
                    RemoveNewObjects();
            }
        }

        public Node CreateNewNode(int numFields,int num, string[] strings)//, string[] outputs, string[] inputs)
        {
            int input = Convert.ToInt32(strings[2]);
            int output = Convert.ToInt32(strings[1]);

            double xincrement = .1;
            double yincrement = .1;
            double xpoint = 194;
            double ypoint = 198;
            switch (numFields)
            {
                case 0:
                    ypoint = 29;
                    break;
                case 1:
                    ypoint = 59;
                    break;
                case 2:
                    ypoint = 95;
                    break;
                case 3:
                    ypoint = 129;
                    break;
                case 4:
                    ypoint = 163;
                    break;
                case 5:
                    ypoint = 198;
                    break;
                case 6:
                    ypoint = 233;
                    break;
                case 7:
                    ypoint = 268;
                    break;
                case 8:
                    ypoint = 303;
                    break;
                case 9:
                    ypoint = 338;
                    break;
                default:
                    ypoint = 373;
                    break;
            }
            xincrement = .9 / output;
            yincrement = .9 / input;



            var node = new Node()
            {
                Name = strings[0],
                //IsNew = true,

                Size = { Value = new Point(xpoint, ypoint) },
                ShortName = "N",
                Location = { Value = new Point(100, 100) },
                Color = Colors.AliceBlue

            };
            
            switch (num)
            {
                case 0:
                    break;
                case 1:
                    node.V1 = strings[3];
                    node.T1 = strings[4];
                    break;
                case 2:
                    node.V1 = strings[3];
                    node.T1 = strings[5];
                    node.V2 = strings[4];
                    node.T2 = strings[6];
                    break;
                case 3:
                    node.V1 = strings[3];
                    node.T1 = strings[6];
                    node.V2 = strings[4];
                    node.T2 = strings[7];
                    node.V3 = strings[5];
                    node.T3 = strings[8];
                    break;
                case 4:
                    node.V1 = strings[3];
                    node.T1 = strings[7];
                    node.V2 = strings[4];
                    node.T2 = strings[8];
                    node.V3 = strings[5];
                    node.T3 = strings[9];
                    node.V4 = strings[6];
                    node.T4 = strings[10];
                    break;
                case 5:
                    node.V1 = strings[3];
                    node.T1 = strings[8];
                    node.V2 = strings[4];
                    node.T2 = strings[9];
                    node.V3 = strings[5];
                    node.T3 = strings[10];
                    node.V4 = strings[6];
                    node.T4 = strings[11];
                    node.V5 = strings[7];
                    node.T5 = strings[12];
                    break;
                case 6:
                    node.V1 = strings[3];
                    node.T1 = strings[9];
                    node.V2 = strings[4];
                    node.T2 = strings[10];
                    node.V3 = strings[5];
                    node.T3 = strings[11];
                    node.V4 = strings[6];
                    node.T4 = strings[12];
                    node.V5 = strings[7];
                    node.T5 = strings[13];
                    node.V6 = strings[8];
                    node.T6 = strings[14];
                    break;
                case 7:
                    node.V1 = strings[3];
                    node.T1 = strings[10];
                    node.V2 = strings[4];
                    node.T2 = strings[11];
                    node.V3 = strings[5];
                    node.T3 = strings[12];
                    node.V4 = strings[6];
                    node.T4 = strings[13];
                    node.V5 = strings[7];
                    node.T5 = strings[14];
                    node.V6 = strings[8];
                    node.T6 = strings[15];
                    node.V7 = strings[9];
                    node.T7 = strings[16];
                    break;
                case 8:
                    node.V1 = strings[3];
                    node.T1 = strings[11];
                    node.V2 = strings[4];
                    node.T2 = strings[12];
                    node.V3 = strings[5];
                    node.T3 = strings[13];
                    node.V4 = strings[6];
                    node.T4 = strings[14];
                    node.V5 = strings[7];
                    node.T5 = strings[15];
                    node.V6 = strings[8];
                    node.T6 = strings[16];
                    node.V7 = strings[9];
                    node.T7 = strings[17];
                    node.V8 = strings[10];
                    node.T8 = strings[18];
                    break;
                case 9:
                    node.V1 = strings[3];
                    node.T1 = strings[12];
                    node.V2 = strings[4];
                    node.T2 = strings[13];
                    node.V3 = strings[5];
                    node.T3 = strings[14];
                    node.V4 = strings[6];
                    node.T4 = strings[15];
                    node.V5 = strings[7];
                    node.T5 = strings[16];
                    node.V6 = strings[8];
                    node.T6 = strings[17];
                    node.V7 = strings[9];
                    node.T7 = strings[18];
                    node.V8 = strings[10];
                    node.T8 = strings[19];
                    node.V9 = strings[11];
                    node.T9 = strings[20];
                    break;
                default:
                    node.V1 = strings[3];
                    node.T1 = strings[13];
                    node.V2 = strings[4];
                    node.T2 = strings[14];
                    node.V3 = strings[5];
                    node.T3 = strings[15];
                    node.V4 = strings[6];
                    node.T4 = strings[16];
                    node.V5 = strings[7];
                    node.T5 = strings[17];
                    node.V6 = strings[8];
                    node.T6 = strings[18];
                    node.V7 = strings[9];
                    node.T7 = strings[19];
                    node.V8 = strings[10];
                    node.T8 = strings[10];
                    node.V9 = strings[11];
                    node.T9 = strings[21];
                    node.V10 = strings[12];
                    node.T10 = strings[22];
                    break;
            }
            int total = strings.Count();
            int nodeCount = total- input - output;
            int inStop = (input + nodeCount - 1);


            string[] nodeCreationSting = new string[nodeCount];
            string[] stringOfInputs = new string[input];
            string[] stringOfOutputs = new string[output];

            for (int i = 0; i < nodeCount; i++)
            {
                nodeCreationSting[i] = strings[i];
            }

            for (int i = 0; i < input; i++)
            {
                stringOfInputs[i] = strings[i + nodeCount];

            }

            for (int i = 0; i < output; i++)
            {
                stringOfOutputs[i] = strings[i + inStop + 1];

            }
            addSnapPoints(node, input, output, yincrement, xincrement, stringOfInputs,stringOfOutputs);
            node.InputSnaps = input;
            node.OutputSnaps = output;
            node.Fields = numFields;

            //NodesThatExist.Add(node);
            //Nodes.Add(node);
            SelectedObject = node;
            return node;
        }
        public void viewNodes(Node n)
        {
            Nodes.Add(n);
            /*foreach( SnapSpot s in n.Snaps)
            {
                Snaps.Add(s);
            }*/
            foreach (var item in n.InSnaps)
            {
                Snaps.Add(item.Value);
            }
            foreach (var item in n.OutSnaps)
            {
                Snaps.Add(item.Value);
            }
        }


        public Node2 CreateNewNode2(int numFields, string[] strings)
        {
            int input = Convert.ToInt32(strings[2]);
            int output = Convert.ToInt32(strings[1]);
            //int input = Convert.ToInt32(strings[3]);
           // int output = Convert.ToInt32(strings[2]); 

            double xincrement = .1;
            double yincrement = .1;
            double xpoint = 194;
            double ypoint = 59;
            xincrement = .9 / output;
            yincrement = .9 / input;

            var node = new Node2()
            {
                Name = strings[0],
                //IsNew = true,

                Size = { Value = new Point(xpoint, ypoint) },
                ShortName = "N",
                Location = { Value = new Point(200, 100) },
                Color = Colors.AliceBlue

            };
            node.V1 = strings[3];
            node.T1 = strings[4];
        
            addSnapPoints2(node, input, output, yincrement, xincrement);
            node.Fields = numFields;

            Nodes2.Add(node);
            SelectedObject = node;
            return node;
        }



        public void addSnapPoints(Node node, int left, int right, double yincrement, double xincrement, string[] inputs, string[] outputs ) {

            double y = .1;
            for (int i = 0 ; i < left; i++)
            {
                
                SnapSpot s = new SnapSpot(node,null) { Offset = { X = 0, Y = y }, Angle = -90, Name = inputs[i], LockX = true, LockY = true };
                node.InSnaps.Add(inputs[i],s);
                //Snaps.Add(s);
                y = y + yincrement;

            }
            double x = .1;
            for (int j = 0; j < right; j++)
            {

                SnapSpot s = new SnapSpot(node,null) { Offset = { X = 1, Y = x }, Angle = 90, Name = outputs[j], LockX = true, LockY = true };
                node.OutSnaps.Add(outputs[j],s);
                //Snaps.Add(s);
                x = x + xincrement;

            }
            
        }

        public void addSnapPoints2(Node2 node, int left, int right, double yincrement, double xincrement)
        {
            double y = .1;
            for (int i = 0; i < left; i++)
            {

                SnapSpot s = new SnapSpot(null, node) { Offset = { X = 0, Y = y }, Angle = -90, Name = "InputSnap " + i, LockX = true, LockY = true };
                node.Snaps.Add(s);
                Snaps.Add(s);
                y = y + yincrement;

            }
            double x = .1;
            for (int j = 0; j < right; j++)
            {

                SnapSpot s = new SnapSpot(null, node) { Offset = { X = 1, Y = x }, Angle = 90, Name = "OutputSnap " + j, LockX = true, LockY = true };
                node.Snaps.Add(s);
                Snaps.Add(s);
                x = x + xincrement;

            }

        }

        public void RemoveNewObjects()
        {
            Nodes.Where(x => x.IsNew).ToList().ForEach(x => Nodes.Remove(x));
            Connectors.Where(x => x.IsNew).ToList().ForEach(x => Connectors.Remove(x));
        }

        private bool _creatingNewConnector;
        public bool CreatingNewConnector
        {
            get { return _creatingNewConnector; }
            set
            {
                _creatingNewConnector = value;
                OnPropertyChanged("CreatingNewConnector");

                if (value)
                    CreateNewConnector();
                else
                    RemoveNewObjects();
            }
        }

        public void CreateNewConnector()
        {
            var connector = new Connector()
                                {
                                    Name = "Connector" + (Connectors.Count + 1),
                                    IsNew = true,
                                };

            Connectors.Add(connector);
            SelectedObject = connector;
        }

        public void customConnectorToData(SnapSpot a, SnapSpot b)
        {

            bool make = true;
            
            if (a.IsConnected)
            {
                MessageBox.Show("Not output availability for " + a.Name);
                make = false;
            }
            if (make)
                {
                    var connector = new Connector()
                    {
                        Name = "Connector" + (Connectors.Count + 1),
                        //IsNew = true,
                        Start = a,
                        End = b,
                        Color = Colors.Red
                    };
                    connector.StartNode = a.Parent;
                    connector.EndNode2 = b.Parent2;
                    Connectors.Add(connector);
                    SelectedObject = connector;
                    a.IsConnected = true;
                    b.IsConnected = true;
                }

            
        }

        public void customConnectorFromData(SnapSpot a,SnapSpot b)
        {
            bool make = true;
            if (b.IsConnected)
            {
                MessageBox.Show("Not output availability for " + a.Name);
                make = false;
            }

            if (make)
            {
                var connector = new Connector()
                {
                    Name = "Connector" + (Connectors.Count + 1),
                    //IsNew = true,
                    Start = a,
                    End = b,
                    Color = Colors.Red
                };
                connector.StartNode2 = a.Parent2;
                connector.EndNode = b.Parent;
                Connectors.Add(connector);
                SelectedObject = connector;
                a.IsConnected = true;
                b.IsConnected = true;
            }


        }




        #endregion

        #region Delete Command

        private Command _deleteCommand;
        public Command DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand =new Command(Delete)); }
        }

        private void Delete()
        {
            if (SelectedObject is Connector)
                Connectors.Remove(SelectedObject as Connector);

            if (SelectedObject is Node)
            {
                var node = SelectedObject as Node;
                //Connectors.Where(x => x.Start == node || x.End == node).ToList().ForEach(x => Connectors.Remove(x));
                Nodes.Remove(node);
            }
        }

        #endregion

        #region Scrolling support

        private double _areaHeight = 10000;
        public double AreaHeight
        {
            get { return _areaHeight; }
            set
            {
                _areaHeight = value;
                OnPropertyChanged("AreaHeight");
            }
        }

        private double _areaWidth = 10000;
        public double AreaWidth
        {
            get { return _areaWidth; }
            set
            {
                _areaWidth = value;
                OnPropertyChanged("AreaWidth");
            }
        }

        #endregion

        public void compilePipeline()
        {
            List<string> branchStrings = new List<string>();
            string tempPipelinePath = conduitLocation + "\\data\\tempPipeline";
            if (Directory.Exists(tempPipelinePath))
                Directory.Delete(tempPipelinePath, true);
            Directory.CreateDirectory(tempPipelinePath);
            System.Threading.Thread.Sleep(100);
            Directory.CreateDirectory(tempPipelinePath + "\\parallel");
            Directory.CreateDirectory(tempPipelinePath + "\\pyscripts");
            string pipelinePath = "/homes/mgheffel/SDP";
            string dataParentDir = "/bulk/mgheffel/data/SDP";
            List<string> nodeNames = new List<string>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                nodeNames.Add(Nodes[i].Name);
                foreach (var item in Nodes[i].InSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        MessageBox.Show("Snap " + item.Key + " from node " + Nodes[i].Name + " is not connected");
                    }
                }
                foreach (var item in Nodes[i].OutSnaps)
                {
                    if (item.Value.IsConnected == false)
                    {
                        MessageBox.Show("Snap " + item.Key + " from node " + Nodes[i].Name + " is not connected");
                    }
                }
            }
            List<string> node2names = new List<string>();
            List<Node2> potentialHeadNodes = new List<Node2>();
            for (int i = 0; i < Nodes2.Count; i++)
            {
                node2names.Add(Nodes2[i].Name);
                if (Nodes2[i].Snaps[0].IsConnected == false)
                {
                    if (Nodes2[i].Snaps[1].IsConnected == false)
                    {
                        MessageBox.Show("Unconnected datanode: " + Nodes2[i].Name);
                    }
                    potentialHeadNodes.Add(Nodes2[i]);
                }
            }
            Node headNode = Nodes[0];
            for (int i = 0; i < Nodes.Count; i++)
            {
                bool headFlag = true;
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].EndNode == null)
                    {
                        continue;
                    }
                    if (Connectors[c].EndNode.Name == Nodes[i].Name)
                    {
                        if (Connectors[c].StartNode2.Snaps[0].IsConnected)
                        {
                            headFlag = false;
                        }
                    }
                }
                if (headFlag)
                {
                    headNode = Nodes[i];
                    break;
                }
            }
            //MessageBox.Show(headNode.Name);
            //MessageBox.Show(getAllFollowingNodes(headNode));
            List<Node> linearPipe = new List<Node>();
            linearPipe.Add(headNode);


            //get input and output dirs
            string inDirs = "";
            foreach (var item in headNode.InSnaps)
            {
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].EndNode == null)
                        continue;
                    if (Connectors[c].EndNode == headNode && Connectors[c].End.Name == item.Value.Name)
                    {
                        inDirs += item.Value.Name + ',' + Connectors[c].StartNode2.T1.ToString() + ';';
                        break;
                    }
                }
            }
            inDirs = inDirs.Substring(0, inDirs.Length - 1);
            string outDirs = "";
            foreach (var item in headNode.OutSnaps)
            {
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].StartNode == null)
                        continue;
                    if (Connectors[c].StartNode == headNode && Connectors[c].Start.Name == item.Value.Name)
                    {
                        outDirs += item.Value.Name + ',' + Connectors[c].EndNode2.T1.ToString() + ';';
                        break;
                    }
                }
            }
            outDirs = outDirs.Substring(0, outDirs.Length - 1);
            ScriptCreator sc = new ScriptCreator(headNode, inDirs, conduitLocation, pipelinePath, dataParentDir);
            sc.compileMasterScript(tempPipelinePath + "\\1a_" + headNode.Name.Split('-')[0] + "M.sh", outDirs);
            sc.compileParallelScript(tempPipelinePath + "\\parallel\\1a_" + headNode.Name.Split('-')[0] + "P.sh");

            //temp vars that will be imported later
            int runTime = 48;
            int mainStage = 1;
            string runallHeader = "#!/bin/bash\n#SBATCH --nodes=1\n#SBATCH --time=" + runTime.ToString() + ":00:00\n";
            string branchNum = "0";
            string runallFile = "while ( ! $alldoneflag )\n\tdo\n"; 
            runallHeader += "pipePath=" + pipelinePath + "\n";
            runallHeader += "dataParentDir=" + dataParentDir + "\n";
            runallHeader += "alldoneflag=false\n";
            //runallFile += "doneflag=false\nstepflag=false\nstages=(0)\nwhile ( ! $doneflag )\n\tdo\n";
            //add headnode to runall
            runallFile += "\tif [ ${stages["+branchNum+"]} == 1 ] ; then\n";
            runallFile += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
            //runallFile += "\t\t\tsbatch " + pipelinePath + "/1a_" + headNode.Name.Split('-')[0] + "M.sh\n";
            runallFile += "\t\t\ttouch " + dataParentDir + "/" + branchNum + '-' + mainStage.ToString() + ".done\n";
            runallFile += "\t\t\tstepflags["+branchNum+"]=true\n";
            runallFile += "\t\telif [ -e $dataParentDir/"+ branchNum + '-' + mainStage.ToString()+".done ] ; then\n";
            runallFile += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
            runallFile += "\t\t\tstepflags["+branchNum+"]=false\n";
            //remove next line
            runallFile += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
            runallFile += "\t\tfi\n";

            
            Node pastNode = headNode;
            //do the complex thing
            bool endPipelineFlag = false;
            while (!endPipelineFlag)
            {
                mainStage ++;
                List<Node> possibleCurNodes = new List<Node>();
                Node curNode = new Node();
                Node2 inDataNode = new Node2();
                SnapSpot pastOut = new SnapSpot(null, null);
                foreach (var item in pastNode.OutSnaps)
                {
                    pastOut = item.Value;
                }
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].StartNode == null)
                        continue;
                    if (Connectors[c].Start == pastOut)
                    {
                        inDataNode = Connectors[c].EndNode2;
                    }
                }
                SnapSpot dnOut = inDataNode.Snaps[1];
                if (dnOut.IsConnected == false)
                {
                    endPipelineFlag = true;
                    break;
                }
                //check fpr branching
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].EndNode == null)
                        continue;
                    if (Connectors[c].Start == dnOut)
                    {
                        possibleCurNodes.Add(Connectors[c].EndNode);
                        //curNode = Connectors[c].EndNode;
                    }
                }
                curNode = possibleCurNodes[0];
                if (possibleCurNodes.Count > 1)
                {
                    bool[] outflags = new bool[possibleCurNodes.Count];
                    for(int i = 0;i < possibleCurNodes.Count; i++)
                    {
                        Node n = possibleCurNodes[i];
                        string[] followNodes = getAllFollowingNodes(n).Split(',');
                        for (int i2=0;i2<possibleCurNodes.Count;i2++)
                        {
                            if (i == i2)
                                continue;
                            Node n2 = possibleCurNodes[i2];
                            if (followNodes.Contains(n2.Name))
                            {
                                outflags[i2] = true;
                            }

                        }
                    }
                    int nodesLeft = 0;
                    for (int i = 0; i < possibleCurNodes.Count; i++)
                    {
                        if (!outflags[i])
                        {
                            nodesLeft++;
                            //MessageBox.Show(possibleCurNodes[i].Name);
                            curNode = possibleCurNodes[i];
                        }
                    }
                    //successfully found branching
                    if (nodesLeft > 1)
                    {
                        MessageBox.Show("branch");
                        
                        List<Node> branches = new List<Node>();
                        for (int i = 0; i < possibleCurNodes.Count; i++)
                        {
                            if (!outflags[i])
                                branches.Add(possibleCurNodes[i]);
                        }

                        bool mergeFlag = false;
                        //check for branch merging
                        for (int i = 0; i < branches.Count; i++)
                        {
                            Node n = branches[i];
                            string[] followingNodes = getAllFollowingNodes(n).Split(',');
                            for (int i2 = 0; i2 < branches.Count; i2++)
                            {
                                if (i2 == i)
                                    continue;
                                Node n2 = branches[i2];
                                string[] followingNodes2 = getAllFollowingNodes(n2).Split(',');
                                foreach (string s in followingNodes)
                                {
                                    if (followingNodes2.Contains(s))
                                    {
                                        //successful merge detect
                                        MessageBox.Show("merge");
                                        mergeFlag = true;
                                    }
                                }

                            }
                        }
                        //handle branching
                        if (!mergeFlag)
                        {
                            //get main branch
                            int indexMain = 0;
                            int maxFollow = 0;
                            for (int b = 0; b < branches.Count; b++)
                            {
                                int followCount = getAllFollowingNodes(branches[b]).Split(',').Length;
                                if (followCount > maxFollow)
                                {
                                    indexMain = b;
                                    maxFollow = followCount;
                                }
                            }
                            curNode = branches[indexMain];
                            for (int b = 0; b < branches.Count; b++)
                            {
                                if (b != indexMain)
                                    branchStrings.Add(createBranch(branches[b],branchStrings,dataParentDir));
                            }
                        }
                    }
                }


                //MessageBox.Show(curNode.Name);
                //MessageBox.Show(getAllFollowingNodes(curNode));
                runallFile += "\telif [ ${stages[0]} == " + mainStage.ToString() + " ] ; then\n";

                /*runallFile += "\t\tif ( ! $stepflag ) ; then\n";
                runallFile += "\t\t\tsbatch " + mainStage.ToString() + curNode.Name + ".sh\n";
                runallFile += "\t\t\tstepflag=true\n";
                runallFile += "\t\telif [ -e " + mainStage.ToString() + ".done ] ; then\n";
                runallFile += "\t\t\tstepflag=false\n";
                runallFile += "\t\t\tstages[0]=${stages[0]}+1\n";
                runallFile += "\t\tfi\n";*/

                runallFile += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
                //runallFile += "\t\t\tsbatch " + pipelinePath + "/" +branchNum+'-'+ mainStage.ToString() + "_" + curNode.Name + "M.sh\n";
                runallFile += "\t\t\ttouch " + dataParentDir + "/" + branchNum + '-' + mainStage.ToString() + ".done\n";
                runallFile += "\t\t\tstepflags[" + branchNum + "]=true\n";
                runallFile += "\t\telif [ -e $dataParentDir/"+branchNum + '-' + mainStage.ToString() +".done ] ; then\n";
                runallFile += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
                runallFile += "\t\t\tstepflags[" + branchNum + "]=false\n";
                //remove next line
                runallFile += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
                runallFile += "\t\tfi\n";

                
                
                pastNode = curNode;
            }
            runallFile += "\telse\n";
            runallFile += "\t\tdoneflags[" + branchNum + "]=true\n";
            runallFile += "\tfi\n";
            foreach (string bString in branchStrings)
            {
                runallFile += bString;
            }
            runallFile += "\talldoneflag=true\n\tfor i in ${doneflags[@]}\n\t\tdo\n\t\tif [ \"$i\" == false ] ; then\n\t\t\talldoneflag=false\n\t\tfi\n\tdone\n";
            runallFile += "\tsleep 60\n";
            runallFile += "done";
            runallHeader += "doneflags=(";
            for (int i = 0; i <= branchStrings.Count; i++)
                runallHeader += "false ";
            runallHeader = runallHeader.Substring(0, runallHeader.Length - 1) + ")\n";
            runallHeader += "stepflags=(";
            for (int i = 0; i <= branchStrings.Count; i++)
                runallHeader += "false ";
            runallHeader = runallHeader.Substring(0, runallHeader.Length - 1) + ")\n";
            runallHeader += "stages=(";
            for (int i = 0; i <= branchStrings.Count; i++)
                runallHeader += "1 ";
            runallHeader = runallHeader.Substring(0, runallHeader.Length - 1) + ")\n";
            File.WriteAllText(tempPipelinePath + "\\0_runall.sh", runallHeader+runallFile);
        }

        public string createBranch(Node n, List<string> branchStrings, string dataParentDir)
        {
            string branchNum = (branchStrings.Count + 1).ToString();
            Node curNode = n;
            int mainStage = 1;
            string branchString = "\tif [ ${stages["+branchNum+"]} == 1 ] ; then\n";
            branchString += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
            //branchString += "\t\t\tsbatch " + branchNum + "-1" + curNode.Name + "\n";
            branchString += "\t\t\ttouch " + dataParentDir + "/" + branchNum + '-' + mainStage.ToString() + ".done\n";
            branchString += "\t\t\tstepflags[" + branchNum + "]=true\n";
            branchString += "\t\telif [ -e $dataParentDir/" + branchNum + "-1.done ] ; then\n";
            branchString += "\t\t\tstepflags[" + branchNum + "]=false\n";
            branchString += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
            //remove next line
            branchString += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
            branchString += "\t\tfi\n";
            


            //do the complex thing
            bool endPipelineFlag = false;
            while (!endPipelineFlag)
            {
                mainStage++;
                Node2 outDataNode = new Node2();
                SnapSpot curOut = new SnapSpot(null, null);
                foreach (var item in curNode.OutSnaps)
                {
                    curOut = item.Value;
                }
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].StartNode == null)
                        continue;
                    if (Connectors[c].Start == curOut)
                    {
                        outDataNode = Connectors[c].EndNode2;
                    }
                }
                SnapSpot dnOut = outDataNode.Snaps[1];
                if (dnOut.IsConnected == false)
                {
                    endPipelineFlag = true;
                    break;
                }
                //check fpr branching
                List<Node> possibleNextNodes = new List<Node>();
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].EndNode == null)
                        continue;
                    if (Connectors[c].Start == dnOut)
                    {
                        possibleNextNodes.Add(Connectors[c].EndNode);
                        //curNode = Connectors[c].EndNode;
                    }
                }
                Node nextNode = possibleNextNodes[0];
                if (possibleNextNodes.Count > 1)
                {
                    bool[] outflags = new bool[possibleNextNodes.Count];
                    for (int i = 0; i < possibleNextNodes.Count; i++)
                    {
                        Node n1 = possibleNextNodes[i];
                        string[] followNodes = getAllFollowingNodes(n1).Split(',');
                        for (int i2 = 0; i2 < possibleNextNodes.Count; i2++)
                        {
                            if (i == i2)
                                continue;
                            Node n2 = possibleNextNodes[i2];
                            if (followNodes.Contains(n2.Name))
                            {
                                outflags[i2] = true;
                            }

                        }
                    }
                    int nodesLeft = 0;
                    for (int i = 0; i < possibleNextNodes.Count; i++)
                    {
                        if (!outflags[i])
                        {
                            nodesLeft++;
                            //MessageBox.Show(possibleNextNodes[i].Name);
                            nextNode = possibleNextNodes[i];
                        }
                    }
                    //successfully found branching
                    if (nodesLeft > 1)
                    {
                        //MessageBox.Show("branch");

                        List<Node> branches = new List<Node>();
                        for (int i = 0; i < possibleNextNodes.Count; i++)
                        {
                            if (!outflags[i])
                                branches.Add(possibleNextNodes[i]);
                        }

                        bool mergeFlag = false;
                        //check for branch merging
                        for (int i = 0; i < branches.Count; i++)
                        {
                            Node n1 = branches[i];
                            string[] followingNodes = getAllFollowingNodes(n1).Split(',');
                            for (int i2 = 0; i2 < branches.Count; i2++)
                            {
                                if (i2 == i)
                                    continue;
                                Node n2 = branches[i2];
                                string[] followingNodes2 = getAllFollowingNodes(n2).Split(',');
                                foreach (string s in followingNodes)
                                {
                                    if (followingNodes2.Contains(s))
                                    {
                                        //successful merge detect
                                        MessageBox.Show("merge");
                                        mergeFlag = true;
                                    }
                                }

                            }
                        }
                        //handle branching
                        if (!mergeFlag)
                        {
                            //get main branch
                            int indexMain = 0;
                            int maxFollow = 0;
                            for (int b = 0; b < branches.Count; b++)
                            {
                                int followCount = getAllFollowingNodes(branches[b]).Split(',').Length;
                                if (followCount > maxFollow)
                                {
                                    indexMain = b;
                                    maxFollow = followCount;
                                }
                            }
                            nextNode = branches[indexMain];
                            for (int b = 0; b < branches.Count; b++)
                            {
                                if (b != indexMain)
                                    branchStrings.Add(createBranch(branches[b],branchStrings,dataParentDir));
                            }
                        }
                    }
                }


                branchString += "\telif [ ${stages[" + branchNum + "]} == "+mainStage.ToString()+" ] ; then\n";
                branchString += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
                //branchString += "\t\t\tsbatch " + branchNum + "-1" + curNode.Name + "\n";
                branchString += "\t\t\ttouch " + dataParentDir + "/" + branchNum + '-' + mainStage.ToString() + ".done\n";
                branchString += "\t\t\tstepflags[" + branchNum + "]=true\n";
                branchString += "\t\telif [ -e $dataParentDir/" + branchNum + "-1.done ] ; then\n";
                branchString += "\t\t\tstepflags[" + branchNum + "]=false\n";
                branchString += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
                //remove next line
                branchString += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
                branchString += "\t\tfi\n";


                MessageBox.Show(nextNode.Name);
                MessageBox.Show(getAllFollowingNodes(nextNode));
                /*branchString += "\telif (( ${stages[" + branchNum + "]} == " + mainStage.ToString() + " )) ; then\n";

                branchString += "\t\tif ( ! $stepflag ) ; then\n";
                branchString += "\t\t\tsbatch " + mainStage.ToString() + nextNode.Name + ".sh\n";
                branchString += "\t\t\tstepflag=true\n";
                branchString += "\t\telif [ -e " + mainStage.ToString() + ".done ] ; then\n";
                branchString += "\t\t\tstepflag=false\n";
                branchString += "\t\t\tstages[0]=${stages[0]}+1\n";
                branchString += "\t\tfi\n";*/

                mainStage += 1;
                
                curNode=nextNode;
            }
            branchString += "\telse\n";
            branchString += "\t\tdoneflags[" + branchNum + "]=true\n";
            branchString += "\tfi\n";
            return branchString;
        }
        public string getAllFollowingNodes(Node n)
        {
            string nodeNames = n.Name;
            foreach (var item in n.OutSnaps)
            {

                SnapSpot n2outSnap = new SnapSpot(null, null);
                foreach (Connector c in Connectors)
                {
                    if (c.Start == item.Value)
                    {
                        n2outSnap = c.EndNode2.Snaps[1];
                        foreach (Connector c2 in Connectors)
                        {
                            if (c2.Start == n2outSnap)
                            {
                                nodeNames += ',' + getAllFollowingNodes(c2.EndNode);
                            }
                        }
                    }

                }
            }
            return nodeNames;
        }
    }
}
