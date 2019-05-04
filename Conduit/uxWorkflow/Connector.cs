using System.Windows;
namespace Conduit
{
    public class Connector: DiagramObject
    {
        public override BindablePoint Location
        {
            get { return new BindablePoint(); }
        }
    	  
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