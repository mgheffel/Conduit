using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System;

namespace Conduit
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Collections

        private ObservableCollection<Node> _nodes;
        public ObservableCollection<Node> Nodes
        {
            get { return _nodes ?? (_nodes = new ObservableCollection<Node>()); }
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
                if (value)
                    CreateNewNode(5, a);
                else
                    RemoveNewObjects();
            }
        }

        public Node CreateNewNode(int numFields, string[] strings)
        {
            int input = Convert.ToInt32(strings[2]);
            int output = Convert.ToInt32(strings[1]);

            double xincrement = .1;
            double yincrement = .1;
            double xpoint = 306;
            double ypoint = 198;
            switch (numFields)
            {
                case 0:
                    xpoint = 104;
                    ypoint = 29;
                    break;
                case 1:
                    xpoint = 194;
                    ypoint = 59;
                    break;
                case 2:
                    xpoint = 384;
                    ypoint = 59;
                    break;
                case 3:
                    xpoint = 576;
                    ypoint = 59;
                    break;
                case 4:
                    xpoint = 384;
                    ypoint = 95;
                    break;
                case 5:
                    xpoint = 576;
                    ypoint = 95;
                    break;
                case 6:
                    xpoint = 384;
                    ypoint = 129;
                    break;
                case 7:
                    xpoint = 384;
                    ypoint = 163;
                    break;
                case 8:
                    xpoint = 576;
                    ypoint = 129;
                    break;
                case 9:
                    xpoint = 576;
                    ypoint = 163;
                    break;
                default:
                    xpoint = 576;
                    ypoint = 198;
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
            switch (numFields)
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
                    node.V4 = strings[5];
                    node.T4 = strings[9];
                    node.V5 = strings[6];
                    node.T5 = strings[10];
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
                    node.V4 = strings[5];
                    node.T4 = strings[11];
                    node.V5 = strings[6];
                    node.T5 = strings[12];
                    node.V6 = strings[7];
                    node.T6 = strings[13];
                    node.V7 = strings[8];
                    node.T7 = strings[14];
                    break;
                case 7:
                    node.V1 = strings[3];
                    node.T1 = strings[10];
                    node.V2 = strings[4];
                    node.T2 = strings[11];
                    node.V4 = strings[5];
                    node.T4 = strings[12];
                    node.V5 = strings[6];
                    node.T5 = strings[13];
                    node.V6 = strings[7];
                    node.T6 = strings[14];
                    node.V7 = strings[8];
                    node.T7 = strings[15];
                    node.V9 = strings[9];
                    node.T9 = strings[16];
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
                    node.V10 = strings[12];
                    node.T10 = strings[21];
                    break;
            }
            addSnapPoints(node, input, output, yincrement, xincrement);
            node.InputSnaps = input;
            node.OutputSnaps = output;
            node.Fields = numFields;

            Nodes.Add(node);
            SelectedObject = node;
            return node;
        }

        public Node2 CreateNewNode2(int numFields, string[] strings)
        {
            int input = Convert.ToInt32(strings[3]);
            int output = Convert.ToInt32(strings[2]); 

            double xincrement = .1;
            double yincrement = .1;
            double xpoint = 194;
            double ypoint = 59;
            xincrement = .9 / output;
            yincrement = .9 / input;

            var node = new Node2()
            {
                Name = strings[4],
                //IsNew = true,

                Size = { Value = new Point(xpoint, ypoint) },
                ShortName = "N",
                Location = { Value = new Point(200, 100) },
                Color = Colors.AliceBlue

            };
            node.V1 = strings[1];
            node.T1 = strings[0];
        
            addSnapPoints2(node, input, output, yincrement, xincrement);
            node.InputSnaps = input;
            node.OutputSnaps = output;
            node.Fields = numFields;

            Nodes2.Add(node);
            SelectedObject = node;
            return node;
        }



        public void addSnapPoints(Node node, int left, int right, double yincrement, double xincrement ) {
            double y = .1;
            for (int i = 0 ; i < left; i++)
            {
                
                SnapSpot s = new SnapSpot(node,null) { Offset = { X = 0, Y = y }, Angle = -90, Name = "InputSnap " + i, LockX = true };
                node.Snaps.Add(s);
                Snaps.Add(s);
                y = y + yincrement;

            }
            double x = .1;
            for (int j = 0; j < right; j++)
            {

                SnapSpot s = new SnapSpot(node,null) { Offset = { X = 1, Y = x }, Angle = -90, Name = "OutputSnap " + j, LockX = true };
                node.Snaps.Add(s);
                Snaps.Add(s);
                x = x + xincrement;

            }
            
        }

        public void addSnapPoints2(Node2 node, int left, int right, double yincrement, double xincrement)
        {
            double y = .1;
            for (int i = 0; i < left; i++)
            {

                SnapSpot s = new SnapSpot(null,node) { Offset = { X = 0, Y = y }, Angle = -90, Name = "InputSnap " + i, LockX = true };
                node.Snaps.Add(s);
                Snaps.Add(s);
                y = y + yincrement;

            }
            double x = .1;
            for (int j = 0; j < right; j++)
            {

                SnapSpot s = new SnapSpot(null,node) { Offset = { X = 1, Y = x }, Angle = -90, Name = "OutputSnap " + j, LockX = true };
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

        public void customConnectorToData(Node a, int d, Node2 b, int c)
        {
            int inputa = a.InputSnaps;
            int inputb = b.InputSnaps;
            int outputa = a.OutputSnaps;
            int outputb = b.OutputSnaps;
            
            int x = a.Snaps.Count;
            int y = b.Snaps.Count;
            int input = 0;
            int output = 0;

            bool make = true;
            if (inputb == 0)
            {

                MessageBox.Show("Not input availability for " + b.Name);
            }
            else
            {
                for (int i = inputa; i < x; i++)
                {
                    if (a.Snaps[i].IsConnected == false)
                    {
                        input = i;
                        break;
                    }
                }
                if (a.Snaps[0].IsConnected == true && input == 0)
                {
                    make = false;
                    MessageBox.Show("Not output availability for " + a.Name);
                }

                for (int k = 0; k < inputb; k++)
                {
                    if (b.Snaps[k].IsConnected == false)
                    {
                        output = k;
                        break;
                    }
                }
                if (b.Snaps[0].IsConnected == true && output == 0)
                {
                    make = false;
                    MessageBox.Show("Not input availability for " + b.Name);
                }

                if (make)
                {
                    var connector = new Connector()
                    {
                        Name = "Connector" + (Connectors.Count + 1),
                        //IsNew = true,
                        Start = a.Snaps[input],
                        End = b.Snaps[output],
                        Color = Colors.Red
                    };
                    connector.StartNode = a;
                    connector.EndNode2 = b;
                    Connectors.Add(connector);
                    SelectedObject = connector;
                    a.Snaps[input].IsConnected = true;
                    b.Snaps[output].IsConnected = true;
                }
            }

            
        }

        public void customConnectorFromData(Node2 a,int d, Node b, int c)
        {
            int inputa = a.InputSnaps;
            int inputb = b.InputSnaps;
            int outputa = a.OutputSnaps;
            int outputb = b.OutputSnaps;
            int x = a.Snaps.Count;
            //int y = b.Snaps.Count;
            int input = 0;
            int output = 0;

            bool make = true;
            for (int i = inputa; i < x; i++)
            {
                if (a.Snaps[i].IsConnected == false)
                {
                    input = i;
                    break;
                }
            }
            if (a.Snaps[0].IsConnected == true && input == 0)
            {
                make = false;
                MessageBox.Show("Not output availability for " + a.Name);
            }

            for (int k = 0; k < inputb; k++)
            {
                if (b.Snaps[k].IsConnected == false)
                {
                    output = k;
                    break;
                }
            }
            if (b.Snaps[0].IsConnected == true && output == 0)
            {
                make = false;
                MessageBox.Show("Not input availability for " + b.Name);
            }
            
            if (make)
            {
                var connector = new Connector()
                {
                    Name = "Connector" + (Connectors.Count + 1),
                    //IsNew = true,
                    Start = a.Snaps[input],
                    End = b.Snaps[output],
                    Color = Colors.Red
                };
                connector.StartNode2 = a;
                connector.EndNode = b;
                Connectors.Add(connector);
                SelectedObject = connector;
                a.Snaps[input].IsConnected = true;
                b.Snaps[output].IsConnected = true;
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

        private double _areaHeight = 500;
        public double AreaHeight
        {
            get { return _areaHeight; }
            set
            {
                _areaHeight = value;
                OnPropertyChanged("AreaHeight");
            }
        }

        private double _areaWidth = 500;
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
    }
}
