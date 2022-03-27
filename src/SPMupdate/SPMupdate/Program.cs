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
                        if (System.IO.File.Exists("C:\\SPM.zip"))
                        {
                            System.IO.File.Delete("C:\\SPM.zip");
                        }
                        //Console.WriteLine("Downloading versions info...");
                        tagdl.DownloadFile("http://repo.bultek.com.ua/SPM-BINARY/SPM-" + man.Branch + ".zip", "C:\\SPM.zip");
                        // Param1 = Link of file
                        // Param2 = Path to save
                    }
                    Console.WriteLine("====================================");
                    Console.WriteLine("Warning: Please ensure that there are no running SPM instances!");
                    Console.WriteLine("Do you want to update now? (Y/n)");
                    string answer = Console.ReadLine();
                    if (answer.ToLower().StartsWith("n"))
                    {
                        Console.WriteLine("====================================");
                        Console.WriteLine("Update aborted!");
                        Console.WriteLine("====================================");
                        System.Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("====================================");
                        Console.WriteLine("Extracting the update...");
                        Console.WriteLine("====================================");
                    }
                    if (System.IO.Directory.Exists("C:\\SPM\\futureversion")) System.IO.Directory.Delete("C:\\SPM\\futureversion", true);
                    if (!System.IO.Directory.Exists("C:\\SPM\\futureversion"))
                    {
                        if (System.IO.Directory.Exists(@"C:\temp\SPM")) System.IO.Directory.Delete(@"C:\temp\", true);
                        ZipFile.ExtractToDirectory("C:\\SPM.zip", "C:\\temp\\");
                    }
                    Console.WriteLine("====================================");
                    Console.WriteLine("Update extracted!");
                    Console.WriteLine("====================================");
                    Console.WriteLine("====================================");
                    Console.WriteLine("Updating...");
                    Console.WriteLine("====================================");

                    if (System.IO.Directory.Exists("C:\\temp\\config.old")) System.IO.Directory.Delete("C:\\temp\\config.old", true);
                    System.IO.Directory.Move("C:\\SPM\\config", "C:\\temp\\config.old");
                    System.IO.Directory.Delete(@"C:\SPM", true);
                    System.IO.Directory.Move("C:\\temp\\SPM", "C:\\SPM");
                    System.IO.Directory.Delete(@"C:\SPM\config", true);
                    System.IO.Directory.Move("C:\\temp\\config.old", "C:\\SPM\\config");
                    Console.WriteLine("If this was an API breaking update (major update) it may break your configs");
                    Console.WriteLine("====================================");
                    Console.WriteLine("Update complete!");
                    Console.WriteLine("====================================");
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