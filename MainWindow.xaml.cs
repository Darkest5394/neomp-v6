using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace ZFN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> EpicServices;
        const string REDIRECT_TV = "slurpydirpcum.dll";
     
        static MainWindow()
        {
            MainWindow.EpicServices = new List<string>()
            {
                "EpicGamesLauncher",
                "EpicWebHelper",
                "CrashReportClient",
                "EpicOnlineServicesHost",
                "EpicOnlineServicesUserHelper"
            };
        }
        public MainWindow()
        {
            InitializeComponent();
           
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Home.Margin = new Thickness(1000, 1000, 1000, 1000);
            Settings.Margin = new Thickness(60,0,-60,0);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Settings.Margin = new Thickness(1000, 1000, 1000, 1000);
            Home.Margin = new Thickness(60, 0, -60, 0);
        }
        async void LaunchFortnite()
        {
            string username = Username.Text;
            string password = Password.Text;
            string FortntiteEXE = Path.Combine((string)DirectoryT.Text, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe");
            string FortniteEAC = Path.Combine((string)DirectoryT.Text, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_EAC.exe");
            string FortLauncher = Path.Combine((string)DirectoryT.Text, "FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe");
            string Argz = string.Concat($"-epicapp=Fortnite -epicenv=Prod -epicportal -AUTH_TYPE=epic -AUTH_LOGIN={username} -AUTH_PASSWORD={password} -epiclocale=en-us -fltoken=7a848a93a74ba68876c36C1c -fromfl=none -noeac -nobe -skippatchcheck");
            Process Fortnite = new Process()
            {
                StartInfo = new ProcessStartInfo(
                    FortntiteEXE,
                    Argz


                )
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                }


            };
            Process FortniteL = new Process()
            {
                StartInfo = new ProcessStartInfo(
                   FortLauncher


               )
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                }
            };

            Process FortEAC = new Process()
            {
                StartInfo = new ProcessStartInfo(
                 FortniteEAC


             )
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                }
            };
            byte[] dllBytes;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(REDIRECT_TV))
                FortniteL.Start();
            foreach (ProcessThread thread in FortniteL.Threads) Util32.SuspendThread(Util32.OpenThread(2, false, thread.Id));
            FortEAC.Start();
            foreach (ProcessThread thread in FortEAC.Threads) Util32.SuspendThread(Util32.OpenThread(2, false, thread.Id));
            Fortnite.Start();
            Stream memoryStream = new MemoryStream(Slurp.Properties.Resources.slurpydirpcum);
            //   Stream memoryStreamDev = new MemoryStream(Properties.Resources.AresNoFiddlerRedirect);
            if (memoryStream == null)
            {
                Console.WriteLine("The attempt to redirect to our backend has failed.");
                return;
            }
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string dllredirectstr = Path.Combine(directory, "slurpydirpcum.dll");
            //   string dllredirectstrdev = Path.Combine(Path.GetTempPath(), "AresNoFiddlerRedirect.dll");
            //string consolestr = Path.Combine(Path.GetTempPath(), "Console.dll");

            using (FileStream fileStream = new FileStream(dllredirectstr, FileMode.Create))
            {
                memoryStream.CopyTo(fileStream);
                Injector.Inject(Fortnite.Id, dllredirectstr);
              //  MessageBox.Show("Mods hit the twin towers");
            }
            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string directoryPath = DirectoryT.Text;

            if (!string.IsNullOrEmpty(directoryPath))
            {
                if (Directory.Exists(directoryPath))
                {
                    string fortniteGamePath = Path.Combine(directoryPath, "FortniteGame");
                    if (Directory.Exists(fortniteGamePath))
                    {
                        LaunchFortnite();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Path.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Directory.");
                }
            }
            else
            {
                MessageBox.Show("Invalid Directory.");
            }
            
        }

        private void Border_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Home.Margin = new Thickness(1000, 1000, 1000, 1000);
            Settings.Margin = new Thickness(60, 0, -60, 0);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string url = "https://discord.gg/neomp";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
