using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Renci.SshNet;

namespace Conduit
{

    public class MainViewModel : INotifyPropertyChanged
    {
        //stores conduit's location on the local device
        string conduitLocation;
        #region Collections

        //Observable Collections for Software Nodes, Data Nodes, and Connectors present on the screen as well as Software Nodes that have not been added

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

        //Shows and sets what item is selected so it can be deleted and identified
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

                //DeleteCommand.IsEnabled = value != null;

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
                    //Shows the name of the selected SnapSpot
                    MessageBox.Show(snap.Name);
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
            //Gets the current directory to be used when necessary
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

        /*Method to create a new Software Node. It takes a given node string array and uses that to create snapSnop 
         * and set the appropriate parameter labels and values as well as input and output snap names
         */
        public Node CreateNewNode(int numFields,int num, string[] strings)
        {
            int input = Convert.ToInt32(strings[2]);
            int output = Convert.ToInt32(strings[1]);

            //sets the size of the node so the appropriate number of parameters appear
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


            //creates the node
            var node = new Node()
            {
                Name = strings[0],
                Size = { Value = new Point(xpoint, ypoint) },
                ShortName = "N",
                Location = { Value = new Point(100, 100) },
                Color = Colors.AliceBlue

            };
            
            //sets the parameter names and values
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

            //Creates string[] of the inputSnap names and the outputSnap names and then uses that to add the snap points
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
            //adds the snapPoints 
            addSnapPoints(node, input, output, yincrement, xincrement, stringOfInputs,stringOfOutputs);
            //Sets the variables of the node to match the appropriate numbers of fields, input snaps, and output snaps
            node.InputSnaps = input;
            node.OutputSnaps = output;
            node.Fields = numFields;
            SelectedObject = node;
            //Returns the created Software Node
            return node;
        }

        //Method to add a software node to the screen
        public void viewNodes(Node n)
        {
            Nodes.Add(n);
            foreach (var item in n.InSnaps)
            {
                Snaps.Add(item.Value);
            }
            foreach (var item in n.OutSnaps)
            {
                Snaps.Add(item.Value);
            }
        }

        //Method to Create a Data Node which similarily takes a node string of the input names, output names, parameter names and values
        public Node2 CreateNewNode2(int numFields, string[] strings)
        {
            int input = Convert.ToInt32(strings[2]);
            int output = Convert.ToInt32(strings[1]);

            //creates the appropriately sized node
            double xincrement = .1;
            double yincrement = .1;
            double xpoint = 194;
            double ypoint = 59;
            xincrement = .9 / output;
            yincrement = .9 / input;

            //Creates the data node
            var node = new Node2()
            {
                Name = strings[0],

                Size = { Value = new Point(xpoint, ypoint) },
                ShortName = "N",
                Location = { Value = new Point(200, 100) },
                Color = Colors.AliceBlue

            };

            //sets the parameter and value
            node.V1 = strings[3];
            node.T1 = strings[4];
             //adds snapPoints to the data node
            addSnapPoints2(node, input, output, yincrement, xincrement);
            node.Fields = numFields;
            //Adds the DataNode to the screen
            Nodes2.Add(node);
            SelectedObject = node;
            //returns the DataNode just added
            return node;
        }

       
        //Adds snapPoints to the Software Node that are evenly spaced along the appropriate side of the node
        public void addSnapPoints(Node node, int left, int right, double yincrement, double xincrement, string[] inputs, string[] outputs ) {

            //input snaps
            double y = .1;
            for (int i = 0 ; i < left; i++)
            {
                SnapSpot s = new SnapSpot(node,null) { Offset = { X = 0, Y = y }, Angle = -90, Name = inputs[i], LockX = true, LockY = true };
                node.InSnaps.Add(inputs[i],s);
                y = y + yincrement;

            }
            //output snaps
            double x = .1;
            for (int j = 0; j < right; j++)
            {

                SnapSpot s = new SnapSpot(node,null) { Offset = { X = 1, Y = x }, Angle = 90, Name = outputs[j], LockX = true, LockY = true };
                node.OutSnaps.Add(outputs[j],s);
                x = x + xincrement;

            }
            
        }
        //Adds snaps for Data Nodes
        public void addSnapPoints2(Node2 node, int left, int right, double yincrement, double xincrement)
        {
            //input snaps
            double y = .1;
            for (int i = 0; i < left; i++)
            {

                SnapSpot s = new SnapSpot(null, node) { Offset = { X = 0, Y = y }, Angle = -90, Name = "InputSnap " + i, LockX = true, LockY = true };
                node.Snaps.Add(s);
                Snaps.Add(s);
                y = y + yincrement;

            }
            //output snaps
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
        //Creates a connector from a software node to a data node
        public void customConnectorToData(SnapSpot a, SnapSpot b)
        {

            bool make = true;
            //checks to make sure the software node snap is not connected
            if (a.IsConnected)
            {
                MessageBox.Show("Not output availability for " + a.Name);
                make = false;
            }
            if (make)
            {
                //creates connector
                var connector = new Connector()
                {
                    Name = "Connector" + (Connectors.Count + 1),
                    Start = a,
                    End = b,
                    Color = Colors.Red
                };
                //sets appropriate properties of the connector to their respective values
                connector.StartNode = a.Parent;
                connector.EndNode2 = b.Parent2;
                Connectors.Add(connector);
                SelectedObject = connector;
                a.IsConnected = true;
                b.IsConnected = true;
            }
        }
        //Creates a connector from a data node to a software node
        public void customConnectorFromData(SnapSpot a,SnapSpot b)
        {
            bool make = true;
            //Ensures that the Software snap is not already connected
            if (b.IsConnected)
            {
                MessageBox.Show("Not output availability for " + a.Name);
                make = false;
            }

            if (make)
            {
                // makes the connector
                var connector = new Connector()
                {
                    Name = "Connector" + (Connectors.Count + 1),
                    Start = a,
                    End = b,
                    Color = Colors.Red
                };
                //sets appropriate properties of the connector to their respective values
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

        //turns user interface into a graph that is parsed into a software pipeline
        public void compilePipeline(string pipelinePath,string dataParentDir, bool deleteWhenDone)
        {
            List<string> branchStrings = new List<string>();
            //temporary locatino to build pipeline
            string tempPipelinePath = conduitLocation + "\\data\\tempPipeline";
            //every master script uses chain to indicate when the step is done
            string chain=File.ReadAllText(conduitLocation+"\\data\\skeletons\\chain.sh");
            //remove previously created pipeline
            if (Directory.Exists(tempPipelinePath))
                Directory.Delete(tempPipelinePath, true);
            Directory.CreateDirectory(tempPipelinePath);
            tempPipelinePath += '\\' + Path.GetFileName(pipelinePath);
            
            System.Threading.Thread.Sleep(100);
            //set up new pipeline structure
            Directory.CreateDirectory(tempPipelinePath);
            System.Threading.Thread.Sleep(100);
            Directory.CreateDirectory(tempPipelinePath + "\\parallel");
            Directory.CreateDirectory(tempPipelinePath + "\\pyscripts");
            Directory.CreateDirectory(tempPipelinePath + "\\dependencies");
            System.Threading.Thread.Sleep(100);
            File.WriteAllText(tempPipelinePath+"\\parallel\\chain.sh", chain.Replace("\r\n", "\n"));
            //check for fatal errors that would currupt pipeline assembly
            for (int i = 0; i < Nodes.Count; i++)
            {
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
            //find first node in pipeline
            List<Node2> potentialHeadNodes = new List<Node2>();
            for (int i = 0; i < Nodes2.Count; i++)
            {
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



            //get input and output dirs of headnode
            string inDirs = getInDirs(headNode, headNode.InSnaps);
            string outDirs = getOutDirs(headNode, headNode.OutSnaps);


            //default runall time
            int runTime = 48;
            int mainStage = 1;
            //set up runall script
            string runallHeader = "#!/bin/bash\n#SBATCH --nodes=1\n#SBATCH --time=" + runTime.ToString() + ":00:00\n";
            string branchNum = "0";
            //build linear pipeline on main branch
            string basename = branchNum + '-' + mainStage.ToString();
            ScriptCreator sc = new ScriptCreator(headNode, inDirs, conduitLocation, pipelinePath, dataParentDir, basename);
            //sc.compileMasterScript(tempPipelinePath + "\\"+ branchNum + '-' + mainStage.ToString() + "_" + "M.sh", outDirs);
            //sc.compileParallelScript(tempPipelinePath + "\\parallel\\"+ branchNum + '-' + mainStage.ToString() + "_"  + "P.sh");
            sc.compileScripts(tempPipelinePath, basename, outDirs);

            //string appended to that is eventually writted as the runall script
            string runallFile = "while ( ! $alldoneflag )\n\tdo\n"; 
            runallHeader += "pipePath=" + pipelinePath + "\n";
            runallHeader += "dataParentDir=" + dataParentDir + "\n";
            runallHeader += "alldoneflag=false\n";
            //add headnode to runall
            runallFile += "\tif [ ${stages["+branchNum+"]} == 1 ] ; then\n";
            runallFile += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
            runallFile += "\t\t\tsbatch " + pipelinePath + "/" + branchNum + '-' + mainStage.ToString() +"_" + "M.sh " + branchNum + '-' + mainStage.ToString() + ".done\n";
            runallFile += "\t\t\tstepflags["+branchNum+"]=true\n";
            runallFile += "\t\telif [ -e $dataParentDir/"+ branchNum + '-' + mainStage.ToString()+".done ] ; then\n";
            runallFile += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
            runallFile += "\t\t\tstepflags["+branchNum+"]=false\n";
            runallFile += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
            runallFile += "\t\tfi\n";

            
            Node pastNode = headNode;
            bool endPipelineFlag = false;
            //iterate through nodes on linear main branch
            while (!endPipelineFlag)
            {
                List<int> newBranchIndicies = new List<int>();
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
                    //detect branching
                    if (nodesLeft > 1)
                    {
                        //MessageBox.Show("branch");
                        
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
                                        MessageBox.Show("Merging pipelines not supported in current version");
                                        mergeFlag = true;
                                        return;
                                    }
                                }

                            }
                        }
                        //handle branching without merge
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
                            //fork nessecary branches
                            for (int b = 0; b < branches.Count; b++)
                            {
                                if (b != indexMain)
                                {
                                    branchStrings.Add(createBranch(branches[b], branchStrings, dataParentDir, pipelinePath, tempPipelinePath));
                                    newBranchIndicies.Add(branchStrings.Count);
                                }
                                    
                            }
                        }
                    }
                }

                //get input and outputs of current node
                inDirs = getInDirs(curNode, curNode.InSnaps);
                outDirs = getOutDirs(curNode, curNode.OutSnaps);

                basename = branchNum + '-' + mainStage.ToString();
                sc = new ScriptCreator(curNode, inDirs, conduitLocation, pipelinePath, dataParentDir, basename);
                sc.compileScripts(tempPipelinePath, basename, outDirs);

                //add current node to script
                runallFile += "\telif [ ${stages[0]} == " + mainStage.ToString() + " ] ; then\n";
                runallFile += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
                runallFile += "\t\t\tsbatch " + pipelinePath + "/" + branchNum + '-' + mainStage.ToString() + "_" + "M.sh " + branchNum + '-' + mainStage.ToString() + ".done\n";
                runallFile += "\t\t\tstepflags[" + branchNum + "]=true\n";
                runallFile += "\t\telif [ -e $dataParentDir/"+branchNum + '-' + mainStage.ToString() +".done ] ; then\n";
                runallFile += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
                //add branches to runall script
                foreach (int b in newBranchIndicies)
                {
                    runallFile+= "\t\t\tstages[" + b + "]=1\n";
                }
                runallFile += "\t\t\tstepflags[" + branchNum + "]=false\n";
                //remove next line
                runallFile += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
                runallFile += "\t\tfi\n";

                
                
                pastNode = curNode;
            }
            //finish runall script
            runallFile += "\telse\n";
            runallFile += "\t\tdoneflags[" + branchNum + "]=true\n";
            runallFile += "\tfi\n";
            foreach (string bString in branchStrings)
            {
                runallFile += bString;
            }
            runallFile += "\talldoneflag=true\n\tfor i in ${doneflags[@]}\n\t\tdo\n\t\tif [ \"$i\" == false ] ; then\n\t\t\talldoneflag=false\n\t\tfi\n\tdone\n";
            runallFile += "\tsleep 60\n";
            runallFile += "done\n";
            runallFile += "touch $dataParentDir/all.done";
            runallHeader += "doneflags=(";
            for (int i = 0; i <= branchStrings.Count; i++)
                runallHeader += "false ";
            runallHeader = runallHeader.Substring(0, runallHeader.Length - 1) + ")\n";
            runallHeader += "stepflags=(";
            for (int i = 0; i <= branchStrings.Count; i++)
                runallHeader += "false ";
            runallHeader = runallHeader.Substring(0, runallHeader.Length - 1) + ")\n";
            runallHeader += "stages=(1 ";
            for (int i = 1; i <= branchStrings.Count; i++)
                runallHeader += "0 ";
            runallHeader = runallHeader.Substring(0, runallHeader.Length - 1) + ")\n";
            File.WriteAllText(tempPipelinePath + "\\runall.sh", runallHeader+runallFile);
            SendPipeline(pipelinePath, conduitLocation + "\\data\\tempPipeline", dataParentDir);
        }

        //recursively create branches in runall script as needed by pipeline graph
        //follows logic of above linear branch assembly
        public string createBranch(Node n, List<string> branchStrings, string dataParentDir, string pipelinePath, string tempPipelinePath)
        {
            string branchNum = (branchStrings.Count + 1).ToString();
            Node curNode = n;
            int mainStage = 1;

            string inDirs = getInDirs(curNode, curNode.InSnaps);
            string outDirs = getOutDirs(curNode, curNode.OutSnaps);


            string basename = branchNum + '-' + mainStage.ToString();
            ScriptCreator sc = new ScriptCreator(curNode, inDirs, conduitLocation, pipelinePath, dataParentDir, basename);
            sc.compileScripts(tempPipelinePath, basename, outDirs);

            string branchString = "\tif [ ${stages["+branchNum+"]} == 1 ] ; then\n";
            branchString += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
            branchString += "\t\t\tsbatch " + pipelinePath + "/" + branchNum + '-' + mainStage.ToString() + "_" + "M.sh "+ branchNum + '-' + mainStage.ToString() + ".done\n";
            branchString += "\t\t\tstepflags[" + branchNum + "]=true\n";
            branchString += "\t\telif [ -e $dataParentDir/" + branchNum + "-1.done ] ; then\n";
            branchString += "\t\t\tstepflags[" + branchNum + "]=false\n";
            branchString += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
            branchString += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
            branchString += "\t\tfi\n";
            


            //recursive branch creation
            bool endPipelineFlag = false;
            while (!endPipelineFlag)
            {
                List<int> newBranchIndicies = new List<int>();
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
                            nextNode = possibleNextNodes[i];
                        }
                    }
                    if (nodesLeft > 1)
                    {

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
                                        MessageBox.Show("Merging pipelines not supported by current version");
                                        mergeFlag = true;
                                        return "";
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
                                {
                                    branchStrings.Add(createBranch(branches[b], branchStrings, dataParentDir, pipelinePath, tempPipelinePath));
                                    newBranchIndicies.Add(branchStrings.Count);
                                }
                                    
                            }
                        }
                    }
                }

                inDirs = getInDirs(curNode, curNode.InSnaps);
                outDirs = getOutDirs(curNode, curNode.OutSnaps);


                basename = branchNum + '-' + mainStage.ToString();
                sc = new ScriptCreator(curNode, inDirs, conduitLocation, pipelinePath, dataParentDir, basename);
                sc.compileScripts(tempPipelinePath, basename, outDirs);

                branchString += "\telif [ ${stages[" + branchNum + "]} == "+mainStage.ToString()+" ] ; then\n";
                branchString += "\t\tif [ ${stepflags[" + branchNum + "]} == false ] ; then\n";
                branchString += "\t\t\tsbatch " + pipelinePath + "/" + branchNum + '-' + mainStage.ToString() + "_" + "M.sh " + branchNum + '-' + mainStage.ToString() + ".done\n";
                branchString += "\t\t\tstepflags[" + branchNum + "]=true\n";
                branchString += "\t\telif [ -e $dataParentDir/" + branchNum + "-1.done ] ; then\n";
                branchString += "\t\t\tstepflags[" + branchNum + "]=false\n";
                branchString += "\t\t\tstages[" + branchNum + "]=$((${stages[" + branchNum + "]}+1))\n";
                foreach (int b in newBranchIndicies)
                {
                    branchString += "\t\t\tstages[" + b + "]=1\n";
                }
                branchString += "\t\t\techo \"" + branchNum + '-' + mainStage.ToString() + " done\"\n";
                branchString += "\t\tfi\n";
                mainStage += 1;
                
                curNode=nextNode;
            }
            branchString += "\telse\n";
            branchString += "\t\tdoneflags[" + branchNum + "]=true\n";
            branchString += "\tfi\n";
            return branchString;
        }

        //returns a comma delimited list of all nodes following the passed node
        //used for branch and merge detection
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
        //set up SFTP connection
        public static ConnectionInfo getSftpConnection(string host, string username, int port, string password)
        {
            return new PasswordConnectionInfo(host, port, username, password);
        }
        public string user;
        public string pass;
        //zips temp pipeline location, sends it to beocat, unzips at desired location and submits runall script
        public void SendPipeline(string beocatPath, string localPath, string parentDir)
        {
            if (File.Exists(conduitLocation + "\\data\\tempPipe.zip"))
            {
                File.Delete(conduitLocation + "\\data\\tempPipe.zip");
                System.Threading.Thread.Sleep(100);
            }
            ZipFile.CreateFromDirectory(localPath, conduitLocation+"\\data\\tempPipe.zip");

            string filename = conduitLocation + "\\data\\tempPipe.zip";
            string justName = "";


            justName = Path.GetFileNameWithoutExtension(filename);
            //MessageBox.Show("Create client Object");
            using (SftpClient sftpClient = new SftpClient(getSftpConnection("headnode.beocat.ksu.edu", user, 22, pass)))
            {
                //MessageBox.Show("Connect to server");
                sftpClient.Connect();
                //MessageBox.Show("Creating FileStream object to stream a file");
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    sftpClient.BufferSize = 1024;
                    sftpClient.UploadFile(fs, Path.GetFileName(filename));
                }

                sftpClient.Dispose();
            }
            

            SshClient ssh = new SshClient("headnode.beocat.ksu.edu", user, pass);
            if (ssh != null)
            {
                using (ssh)
                {
                    ssh.Connect();
                    string file2upzip = "unzip " + justName + ".zip";
                    var command = ssh.CreateCommand("unzip " + justName + ".zip");
                    command.Execute();
                    command = ssh.CreateCommand("cd " + parentDir);
                    command.Execute();
                    command=ssh.CreateCommand("sbatch " + beocatPath + "/runall.sh");
                    command.Execute();
                }
                //transferred zip to beocat but threw error here
                //ssh.Disconnect();
            }
            MessageBox.Show("Pipeline submitted");
        }
        //gets nodes input directories from input snaps
        public string getInDirs(Node curNode, Dictionary<string, SnapSpot> snaps)
        {
            string inDirs = "";
            foreach (var item in snaps)
            {
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].EndNode == null)
                        continue;
                    if (Connectors[c].EndNode == curNode && Connectors[c].End.Name == item.Value.Name)
                    {
                        inDirs += item.Value.Name + ',' + Connectors[c].StartNode2.T1.ToString() + ';';
                        break;
                    }
                }
            }
            inDirs = inDirs.Substring(0, inDirs.Length - 1);
            return inDirs;
        }
        //gets nodes output directories from output snaps
        public string getOutDirs(Node curNode, Dictionary<string,SnapSpot> snaps)
        {
            string outDirs = "";
            foreach (var item in snaps)
            {
                for (int c = 0; c < Connectors.Count; c++)
                {
                    if (Connectors[c].StartNode == null)
                        continue;
                    if (Connectors[c].StartNode == curNode && Connectors[c].Start.Name == item.Value.Name)
                    {
                        outDirs += item.Value.Name + ',' + Connectors[c].EndNode2.T1.ToString() + ';';
                        break;
                    }
                }
            }
            outDirs = outDirs.Substring(0, outDirs.Length - 1);
            return outDirs;
        }
    }
}
