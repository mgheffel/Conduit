using System.Windows;
namespace Conduit
{
    public class Connector: DiagramObject
    {
        public override BindablePoint Location
        {
            get { return new BindablePoint(); }
        }
    	//initial snapspot of connectore  
        private SnapSpot _start;
        public SnapSpot Start
        {
            get { return _start; }
            set
            {
                _start = value;
                OnPropertyChanged("Start");
            }
        }
        //final SnapSpot of connector
        private SnapSpot _end;
        public SnapSpot End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged("End");
                MidPoint.Value = new Point(((End.Location.X + Start.Location.X) / 2), 
                                     ((End.Location.Y + Start.Location.Y) / 2));
            }
        }
        //Node that contains the start SnapSpot for software nodes
        private Node startNode;
        public Node StartNode
        {
            get { return startNode; }
            set
            {
                if (startNode != value)
                {
                    startNode = value;
                    OnPropertyChanged("StartNode");
                }
            }
        }
        //Node2 that contains the start SnapSpot for data nodes
        private Node2 startNode2;
        public Node2 StartNode2
        {
            get { return startNode2; }
            set
            {
                if (startNode2 != value)
                {
                    startNode2 = value;
                    OnPropertyChanged("StartNode2");
                }
            }
        }
        //Node that contains the end SnapSpot for software nodes
        private Node endNode;
        public Node EndNode
        {
            get { return endNode; }
            set
            {
                if (endNode != value)
                {
                    endNode = value;
                    OnPropertyChanged("EndNode");
                }
            }
        }
        //Node2 that contains the end SnapSpot for data nodes
        private Node2 endNode2;
        public Node2 EndNode2
        {
            get { return endNode2; }
            set
            {
                if (endNode2 != value)
                {
                    endNode2 = value;
                    OnPropertyChanged("EndNode2");
                }
            }
        }

        private BindablePoint _midPoint;
        public BindablePoint MidPoint
        {
            get { return _midPoint ?? (_midPoint = new BindablePoint()); }
        }
    }
}