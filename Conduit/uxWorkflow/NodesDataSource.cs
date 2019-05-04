using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Conduit
{
    public static class NodesDataSource
    {
        public static Random random = new Random();

        public static List<Color> Colors; 

        public static Node GetRandomNode()
        {
            var nodenumber = random.Next(0, 100);
            var nodename = "Node" + nodenumber;
            var node = new Node
                {
                    Size = {Value = new Point(150,120)},
                    Name = nodename,
                    ShortName = "N" + nodenumber,
                    Location = { Value = new Point(random.Next(0, 500), random.Next(0, 500)) },
                    Color = Colors[random.Next(0, Colors.Count - 1)],
                };


            node.Snaps.AddRange(new[]
                                    {
                                        new SnapSpot(node,null) {Offset = {X = 0, Y = .5}, Angle = -90, Name = "LeftSnap" + nodenumber, LockX = true},
                                        new SnapSpot(node,null) {Offset = {X = .5, Y = 0}, Angle = 0, Name = "TopSnap" + nodenumber, LockY = true},
                                        new SnapSpot(node,null) {Offset = {X = 1, Y = .5}, Angle = 90, Name = "RightSnap" + nodenumber, LockX = true},
                                        new SnapSpot(node,null) {Offset = {X = .5, Y = 1}, Angle = 180, Name = "BottomSnap" + nodenumber, LockY = true},
                                    });
            return node;
        }

        public static IEnumerable<Node> GetRandomNodes()
        {
            Colors = typeof(Colors).GetProperties().Select(x => (Color)x.GetValue(null, null)).ToList();

            return Enumerable.Range(5, random.Next(6, 10)).Select(x => GetRandomNode());
        }

        public static IEnumerable<Connector> GetRandomConnectors(List<Node> nodes)
        {
            var result = new List<Connector>();
            for (int i = 0; i < nodes.Count() - 1; i++)
            {
                result.Add(new Connector() 
                {
                    Start = nodes[i].Snaps[random.Next(0,3)], 
                    End = nodes[i + 1].Snaps[random.Next(0,3)],
                    Name = "Connector" + random.Next(1, 100).ToString(),
                    Color = Colors[random.Next(0, Colors.Count - 1)],
                });
            }
            return result;
        }
    }
}