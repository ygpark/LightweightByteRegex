﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ByteRegexPInvoke
{
    public class ByteRegex
    {
        #region PInvoke
        [DllImport("ByteRegex.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr byteregex_init();

        [DllImport("ByteRegex.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr byteregex_init_by_pattern(byte[] pattern, int size);

        [DllImport("ByteRegex.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int byteregex_free(IntPtr handle);

        [DllImport("ByteRegex.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void byteregex_compile(IntPtr handle, byte[] pattern, int size);

        [DllImport("ByteRegex.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int byteregex_matches(IntPtr handle, byte[] input, int size);
        
        [DllImport("ByteRegex.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int byteregex_get_matches(IntPtr handle, int[] output, out int size);

        [DllImport("ByteRegex.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int byteregex_get_matches_count(IntPtr handle);

        #endregion

        #region 멤버변수
        IntPtr _handle;
        #endregion

        public ByteRegex()
        {
            _handle = byteregex_init();
        }

        public ByteRegex(string pattern)
        {
            byte[] buf = Encoding.GetEncoding("Latin1").GetBytes(pattern);
            _handle = byteregex_init_by_pattern(buf, buf.Length);
        }

        ~ByteRegex()
        {
            byteregex_free(_handle);
        }

        public void Compile(string pattern)
        {
            byte[] buf = Encoding.GetEncoding("Latin1").GetBytes(pattern);
            byteregex_compile(_handle, buf, buf.Length);
        }


        public int[] Matches(byte[] buffer)
        {
            int count;

            count = byteregex_matches(_handle, buffer, buffer.Length);
            int[] output = new int[count];
            output = new int[count];
            int err = byteregex_get_matches(_handle, output, out count);
            if (err == -1)// need more buffer
            {
                output = new int[count];
                err = byteregex_get_matches(_handle, output, out _);
                if (err != 0)
                {
                    throw new Exception("알 수 없는 오류 발생");
                }
            }

            return output;
        }
    }
}
