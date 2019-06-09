using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// Taken from Sewer56/Heroes-Collision-Library
namespace Reloaded.Memory.Shared.Structs
{
    public struct Quadnode
    {
        private static Random _random = new Random();

        public ushort NodeIndex;
        public ushort NodeParent;
        public ushort NodeChild;
        public ushort RightNodeNeighbour;
        public ushort LeftNodeNeighbour;
        public ushort BottomNodeNeighbour;
        public ushort TopNodeNeighbour;
        public ushort NumberOfTriangles;
        public uint OffsetTriangleList;
        public ushort PositioningOffsetValueLR;
        public ushort PositioningOffsetValueTB;
        public byte NodeDepthLevel;
        public byte Null1;
        public ushort Null2;
        public uint Null3;

        public static Quadnode BuildRandomStruct()
        {
            Quadnode quadNode;
            quadNode.NodeIndex = (ushort) _random.Next();
            quadNode.NodeParent = (ushort) _random.Next();
            quadNode.NodeChild = (ushort) _random.Next();
            quadNode.RightNodeNeighbour = (ushort) _random.Next();
            quadNode.LeftNodeNeighbour = (ushort) _random.Next();
            quadNode.BottomNodeNeighbour = (ushort) _random.Next();
            quadNode.TopNodeNeighbour = (ushort) _random.Next();
            quadNode.NumberOfTriangles = (ushort) _random.Next();
            quadNode.OffsetTriangleList = (uint) _random.Next();
            quadNode.PositioningOffsetValueLR = (ushort) _random.Next();
            quadNode.PositioningOffsetValueTB = (ushort) _random.Next();
            quadNode.NodeDepthLevel = (byte) _random.Next();
            quadNode.Null1 = 0;
            quadNode.Null2 = 0;
            quadNode.Null3 = 0;
            return quadNode;
        }
    }
}
