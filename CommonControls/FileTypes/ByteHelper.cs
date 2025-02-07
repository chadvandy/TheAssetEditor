﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonControls.FileTypes
{
    public class ByteHelper
    {
        public static T ByteArrayToStructure<T>(byte[] bytes, int offset) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            var objectSize = GetSize<T>();
            if (offset + objectSize > bytes.Length)
                throw new Exception($"Object {typeof(T)} does not fit into the remaining buffer [offset{offset} + Size{objectSize} => byteBuffer{bytes.Length}]");

            try
            {
                var p = handle.AddrOfPinnedObject() + offset;
                return (T)Marshal.PtrToStructure(p, typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        public static byte[] GetBytes<T>(T data) where T : struct
        {
            int size = Marshal.SizeOf(data);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        static public byte[] CreateFixLengthString(string str, int maxLength)
        {
            byte[] output = new byte[maxLength];
            var byteValues = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byteValues.Length && i < maxLength; i++)
                output[i] = byteValues[i];
            return output;
        }

        public static int GetSize(Type type)
        {
            return Marshal.SizeOf(type);
        }

        public static int GetSize<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }
    }
}
