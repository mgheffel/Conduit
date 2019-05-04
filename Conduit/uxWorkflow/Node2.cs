using System.Collections.Generic;
using System.Windows;

namespace Conduit
{
    public class Node2 : DiagramObject
    {
        public Node2()
        {
            Size.ValueChanged = RecalculateSnaps;
            Location.ValueChanged = RecalculateSnaps;

        }

        private void RecalculateSnaps()
        {
            Snaps.ForEach(x => x.Recalculate2());
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

        private string t1 = "";
        private string v1 = "";
        
        public string T1
        {
            get { return t1; }
            set
            {
                if (t1 != value)
                {
                    t1 = value;
                    OnPropertyChanged("T1");
                }
            }
        }
        public string V1
        {
            get { return v1; }
            set
            {
                if (v1 != value)
                {
                    v1 = value;
                    OnPropertyChanged("V1");
                }
            }
        }

        public string ShortName
        {
            get { return _shortName; }
            set
            {
                _shortName = value;
                OnPropertyChanged("ShortName");
            }
        }
        private int inputSnaps;
        public int InputSnaps
        {
            get { return inputSnaps; }
            set
            {
                inputSnaps = value;
                OnPropertyChanged("InputSnaps");
            }
        }
        private int outputSnaps;
        public int OutputSnaps
        {
            get { return outputSnaps; }
            set
            {
                outputSnaps = value;
                OnPropertyChanged("OutputSnaps");
            }
        }

        private int fields;
        public int Fields
        {
            get { return fields; }
            set
            {
                fields = value;
                OnPropertyChanged("Fields");
            }
        }
    }
}
