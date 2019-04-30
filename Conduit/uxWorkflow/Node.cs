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

        private string t1 ="";
        private string t2 = "";
        private string t3 = "";
        private string t4 = "";
        private string t5 = "";
        private string t6 = "";
        private string t7 = "";
        private string t8 = "";
        private string t9 = "";
        private string t10 = "";

        private string v1 = "";
        private string v2 = "";
        private string v3 = "";
        private string v4 = "";
        private string v5 = "";
        private string v6 = "";
        private string v7 = "";
        private string v8 = "";
        private string v9 = "";
        private string v10 = "";
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
        public string T2
        {
            get { return t2; }
            set
            {
                if (t2 != value)
                {
                    t2 = value;
                    OnPropertyChanged("T2");
                }
            }
        }
        public string T3
        {
            get { return t3; }
            set
            {
                if (t3 != value)
                {
                    t3 = value;
                    OnPropertyChanged("T3");
                }
            }
        }
        public string T4
        {
            get { return t4; }
            set
            {
                if (t4 != value)
                {
                    t4 = value;
                    OnPropertyChanged("T4");
                }
            }
        }
        public string T5
        {
            get { return t5; }
            set
            {
                if (t5 != value)
                {
                    t5 = value;
                    OnPropertyChanged("T5");
                }
            }
        }
        public string T6
        {
            get { return t6; }
            set
            {
                if (t6 != value)
                {
                    t6 = value;
                    OnPropertyChanged("T6");
                }
            }
        }
        public string T7
        {
            get { return t7; }
            set
            {
                if (t7 != value)
                {
                    t7 = value;
                    OnPropertyChanged("T7");
                }
            }
        }
        public string T8
        {
            get { return t8; }
            set
            {
                if (t8 != value)
                {
                    t8 = value;
                    OnPropertyChanged("T8");
                }
            }
        }
        public string T9
        {
            get { return t9; }
            set
            {
                if (t9 != value)
                {
                    t9 = value;
                    OnPropertyChanged("T9");
                }
            }
        }
        public string T10
        {
            get { return t10; }
            set
            {
                if (t10 != value)
                {
                    t10 = value;
                    OnPropertyChanged("T10");
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
        public string V2
        {
            get { return v2; }
            set
            {
                if (v2 != value)
                {
                    v2 = value;
                    OnPropertyChanged("V2");
                }
            }
        }
        public string V3
        {
            get { return v3; }
            set
            {
                if (v3 != value)
                {
                    v3 = value;
                    OnPropertyChanged("V3");
                }
            }
        }
        public string V4
        {
            get { return v4; }
            set
            {
                if (v4 != value)
                {
                    v4 = value;
                    OnPropertyChanged("V4");
                }
            }
        }
        public string V5
        {
            get { return v5; }
            set
            {
                if (v5 != value)
                {
                    v5 = value;
                    OnPropertyChanged("V5");
                }
            }
        }
        public string V6
        {
            get { return v6; }
            set
            {
                if (v6 != value)
                {
                    v6 = value;
                    OnPropertyChanged("V6");
                }
            }
        }
        public string V7
        {
            get { return v7; }
            set
            {
                if (v7 != value)
                {
                    v7 = value;
                    OnPropertyChanged("V7");
                }
            }
        }
        public string V8
        {
            get { return v8; }
            set
            {
                if (v8 != value)
                {
                    v8 = value;
                    OnPropertyChanged("V8");
                }
            }
        }
        public string V9
        {
            get { return v9; }
            set
            {
                if (v9 != value)
                {
                    v9 = value;
                    OnPropertyChanged("V9");
                }
            }
        }
        public string V10
        {
            get { return v10; }
            set
            {
                if (v10 != value)
                {
                    v10 = value;
                    OnPropertyChanged("V10");
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