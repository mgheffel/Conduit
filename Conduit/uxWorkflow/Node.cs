using System.Collections.Generic;
using System.Windows;
namespace Conduit
{
    public class Node: DiagramObject
    {
        public Node()
        {
            Size.ValueChanged = RecalculateSnaps;
            Location.ValueChanged = RecalculateSnaps;
        }

        private void RecalculateSnaps()
        {
            Snaps.ForEach(x => x.Recalculate());
        }

        private List<SnapSpot> _snaps;
        public List<SnapSpot> Snaps
        {
            get { return _snaps ?? (_snaps = new List<SnapSpot>()); }
        }

        private Point originalSize;
        private BindablePoint _size;
        public BindablePoint Size
        {
            get { return _size ?? (_size = new BindablePoint()); }
        }

        private bool _isCollapsed;
        public bool IsCollapsed
        {
            get { return _isCollapsed; }
            set
            {
                if (value)
                {
                    originalSize = Size.Value;
                    Size.Value = new Point { X = 50, Y = 50 };
                }
                else
                    Size.Value = originalSize;

                _isCollapsed = value;
                OnPropertyChanged("IsCollapsed");
                RecalculateSnaps();
            }
        }

        private string _shortName;
        public string ShortName
        {
            get { return _shortName; }
            set
            {
                _shortName = value;
                OnPropertyChanged("ShortName");
            }
        }
    }
}