using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ZFN
{
    internal class Injector
    {
        public const int PROCESS_CREATE_THREAD = 2;

        public const int PROCESS_VM_OPERATION = 8;

        public const int PROCESS_VM_WRITE = 32;

        public const int PROCESS_VM_READ = 16;

        public const int PROCESS_QUERY_INFORMATION = 1024;

        public const uint PAGE_READWRITE = 4;

        public const uint MEM_COMMIT = 4096;

        public const uint MEM_RESERVE = 8192;

        public Injector()
        {
        }

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        public static void Inject(int processId, string path)
        {
            UIntPtr uIntPtr;
            IntPtr intPtr = Injector.OpenProcess(1082, false, processId);
            IntPtr procAddress = Injector.GetProcAddress(Injector.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            uint length = (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char)));
            IntPtr intPtr1 = Injector.VirtualAllocEx(intPtr, IntPtr.Zero, length, 12288, 4);
            Injector.WriteProcessMemory(intPtr, intPtr1, Encoding.Default.GetBytes(path), length, out uIntPtr);
            Injector.CreateRemoteThread(intPtr, IntPtr.Zero, 0, procAddress, intPtr1, 0, IntPtr.Zero);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        public static extern bool SetConsoleCtrlHandler(Injector.HandlerRoutine HandlerRoutine, bool Add);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        public delegate bool HandlerRoutine(int dwCtrlType);
    }
}