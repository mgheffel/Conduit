using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace Conduit
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region Collections

        private ObservableCollection<Node> _nodes;
        public ObservableCollection<Node> Nodes
        {
            get { return _nodes ?? (_nodes = new ObservableCollection<Node>()); }
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

                if (value)
                    CreateNewNode(4,3,5);
                else
                    RemoveNewObjects();
            }
        }

        public Node CreateNewNode(int input, int output, int numFields)
        {
            double xincrement = .1;
            double yincrement = .1;
            double xpoint = 102;
            double ypoint = 148;
            switch (numFields)
            {
                case 1:
                    xpoint = 52;
                    ypoint = 49;
                    break;
                case 2:
                    xpoint = 102;
                    ypoint = 49;
                    break;
                case 3:
                    xpoint = 153;
                    ypoint = 49;
                    break;
                case 4:
                    xpoint = 102;
                    ypoint = 75;
                    break;
                case 5:
                    xpoint = 153;
                    ypoint = 75;
                    break;
                case 6:
                    xpoint = 102;
                    ypoint = 99;
                    break;
                case 7:
                    xpoint = 102;
                    ypoint = 123;
                    break;
                case 8:
                    xpoint = 153;
                    ypoint = 99;
                    break;
                case 9:
                    xpoint = 153;
                    ypoint = 123;
                    break;
                default:
                    xpoint = 153;
                    ypoint = 148; 
                    break;
            }
            xincrement = .9/output;
            yincrement = .9/input;

            var newnode = new Node()
            {
                Name = "Node" + (Nodes.Count + 1),
                IsNew = true,

                                    Size = { Value = new Point( xpoint, ypoint) },
                                    ShortName = "N",
                                    Location = { Value = new Point(256, 100) },
                                    Color = Colors.AliceBlue
            };
            addSnapPoints(newnode, input, output, yincrement, xincrement);
            
            Nodes.Add(newnode);
            SelectedObject = newnode;
            return newnode;
        }

        public void addSnapPoints(Node node, int left, int right, double yincrement, double xincrement ) {
            double y = .1;
            for (int i = 0 ; i < left; i++)
            {
                
                SnapSpot s = new SnapSpot(node) { Offset = { X = 0, Y = y }, Angle = -90, Name = "InputSnap " + i, LockX = true };
                node.Snaps.Add(s);
                Snaps.Add(s);
                y = y + yincrement;

            }
            double x = .1;
            for (int j = 0; j < right; j++)
            {

                SnapSpot s = new SnapSpot(node) { Offset = { X = 1, Y = x }, Angle = -90, Name = "OutputSnap " + j, LockX = true };
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

        public void customConnector(Node a, Node b)
        {
            int x = a.Snaps.Count;
            int y = b.Snaps.Count;
            int input = 0;
            int output = 0;

            bool make = true;
            for(int i = x/2 +1; i<x; i++)
            {
              if (a.Snaps[i].IsConnected == false)
                {
                    input = i;
                    break;
                }
            }
            if (input == 0)
            {
                make = false;
                MessageBox.Show("Not output availability for "+ a.Name);
            }
           
                for (int k = 0; k <= y / 2; k++)
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
                    IsNew = true,
                    Start = a.Snaps[input],
                    End = b.Snaps[output],
                    Color = Colors.Red
                };
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
