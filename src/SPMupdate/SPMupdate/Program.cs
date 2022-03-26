using System.IO.Compression;
using System.Net;
namespace SPMupdateManager
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("Sharp Package Manager update utility");
            Console.WriteLine("Gathering Data...");
            Console.WriteLine("====================================");
            Manager man = new Manager();
            if (args.Length == 2)
            {
                man.Branch = args[0];
                man.CurrentVersion = int.Parse(args[1]);
                if (man.IsUpdateAvailable)
                {
                    Console.WriteLine("====================================");
                    Console.WriteLine("Update available!");
                    Console.WriteLine("Downloading update...");
                    Console.WriteLine("====================================");
                    using (WebClient tagdl = new WebClient())
                    {
                        //Console.WriteLine("Downloading versions info...");
                        tagdl.DownloadFile("http://repo.bultek.com.ua/SPM-BINARY/SPM-" + man.Branch + ".zip", "C:\\SPM.zip");
                        // Param1 = Link of file
                        // Param2 = Path to save
                    }
                    Console.WriteLine("Extracting the update...");
                    if (System.IO.Directory.Exists("C:\\SPM\\futureversion")) System.IO.Directory.Delete("C:\\SPM\\futureversion", true);
                    if (!System.IO.Directory.Exists("C:\\SPM\\futureversion"))
                    {
                        System.IO.Directory.CreateDirectory("C:\\SPM\\futureversion");
                        ZipFile.ExtractToDirectory("C:\\SPM.zip", "C:\\SPM\\futureversion");
                    }
                    Console.WriteLine("====================================");
                    Console.WriteLine("Update extracted!");
                    Console.WriteLine("====================================");
                    Console.WriteLine("Updating...");
                    if (System.IO.Directory.Exists("C:\\SPM\\config.old")) System.IO.Directory.Delete("C:\\SPM\\config.old", true);
                    System.IO.Directory.Move("C:\\SPM\\config", "C:\\SPM\\config.old");
                    String[] files = Directory.GetFiles(@"C:\SPM\futureversion", "*", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        System.IO.File.Copy(file, file.Replace("C:\\SPM\\futureversion", "C:\\"), true);
                    }
                    System.IO.Directory.Delete("C:\\SPM\\config", true);
                    System.IO.Directory.Move("C:\\SPM\\config.old", "C:\\SPM\\config");
                    Console.WriteLine("If this was a API breaking update (major update) it may break your configs");
                    Console.WriteLine("====================================");
                    Console.WriteLine("Update complete!");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("===================");
                    Console.WriteLine("No Update Required!");
                    Console.WriteLine("===================");
                }
            }
            else
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("!!!!!!!!!!NOT ENOUGH ARGUMENTS!!!!!!!!!");
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.ReadKey();
            }
        }
    }
    public class Manager
    {
        public string Branch { get; set; }
        public int CurrentVersion { get; set; }
        public int LatestVersion
        {

            get
            {
                if (System.IO.File.Exists("C:\\temp\\latestversioninfo.spmvi")) System.IO.File.Delete("C:\\temp\\latestversioninfo.spmvi");
                if (System.IO.File.Exists("C:\\temp\\latestversiontag.spmvi")) System.IO.File.Delete("C:\\temp\\latestversiontag.spmsvi");
                using (WebClient tagdl = new WebClient())
                {
                    tagdl.DownloadFile("https://gitlab.com/bultekdev/spm-projects/SharpPackageManager/-/raw/versioncontrol/" + Branch + ".spmvi", "C:\\temp\\latestversioninfo.spmvi");
                    // Param1 = Link of file
                    // Param2 = Path to save
                }
                // Read latest version info
                using (StreamReader file = new StreamReader("C:\\temp\\latestversioninfo.spmvi"))
                {
                    int latestversion = int.Parse(file.ReadLine());
                    return latestversion;
                }
            }
        }
        public bool IsUpdateAvailable
        {
            get
            {
                if (LatestVersion > CurrentVersion) return true;
                else return false;
            }
        }
    }
}