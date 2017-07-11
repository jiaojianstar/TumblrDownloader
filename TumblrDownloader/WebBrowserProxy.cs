using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TumblrDownloader
{
    static class WebBrowserProxy

    {

        struct Struct_INTERNET_PROXY_INFO

        {

            public int dwAccessType;

            public IntPtr proxy;

            public IntPtr proxyBypass;

        };



        [DllImport("wininet.dll", SetLastError = true)]

        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);



        public static void SetProxy(string strProxy)

        {

            const int INTERNET_OPTION_PROXY = 38;

            const int INTERNET_OPEN_TYPE_PROXY = 3;



            Struct_INTERNET_PROXY_INFO struct_IPI;



            // Filling in structure

            struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;

            struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);

            struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");



            // Allocating memory

            IntPtr intptrStruct = Marshal.AllocCoTaskMem(Marshal.SizeOf(struct_IPI));



            // Converting structure to IntPtr

            Marshal.StructureToPtr(struct_IPI, intptrStruct, true);

            bool iReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(struct_IPI));

        }

    }
}
