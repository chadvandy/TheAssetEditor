﻿using System.Runtime.InteropServices;

namespace AssetManagement.GenericFormats.Unmanaged
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BoneKey
    {
        public XMFLOAT3 translation;
        public XMFLOAT4 quaternion;
        public double timeStampe;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexInfluence
    {
        public uint boneIndex;
        public float weight;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct PackedCommonVertex
    {
        public XMFLOAT4 Position;
        public XMFLOAT3 Normal;
        public XMFLOAT3 BiNormal;
        public XMFLOAT3 Tangent;
        public XMFLOAT2 Uv;
        public XMFLOAT4 Color;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 4)]
        public VertexInfluence[]? influences; // fixed array length 4        

        public int WeightCount;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct VertexWeight
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string boneName;
        public int vertexIndex;
        public float vertexWeight;
    }

}
