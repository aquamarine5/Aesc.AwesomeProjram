using System.IO;
using System;
using Aesc.AwesomeKits;
using Microsoft.Win32;
using System.Diagnostics;

namespace Aesc.AwesomeProjram
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                FileInfo file = new FileInfo("./AwesomeProgram.launch.json");
                var stream = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader streamReader = new StreamReader(stream);
                string s = streamReader.ReadToEnd();
                if (s == "" || s == "n")
                {
                    RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(@"directory\shell", true);
                    if (registryKey == null) registryKey = Registry.ClassesRoot.CreateSubKey(@"directory\shell");
                    RegistryKey rightCommandKey = registryKey.CreateSubKey("发送文件夹...(S)");
                    RegistryKey associatedKey = rightCommandKey.CreateSubKey("command");
                    associatedKey.SetValue("", $"\"{Process.GetCurrentProcess().MainModule.FileName}\" -directory \"%1\"");
                    ///////
                    registryKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
                    if (registryKey == null) registryKey = Registry.ClassesRoot.CreateSubKey(@"*\shell");
                    rightCommandKey = registryKey.CreateSubKey("发送...(S)");
                    associatedKey = rightCommandKey.CreateSubKey("command");
                    associatedKey.SetValue("", $"\"{Process.GetCurrentProcess().MainModule.FileName}\" -file \"%1\"");
                    ////////////
                    registryKey = Registry.ClassesRoot.OpenSubKey(@"pngfile\shell", true);
                    if (registryKey == null) registryKey = Registry.ClassesRoot.CreateSubKey(@"pngfile\shell");
                    rightCommandKey = registryKey.CreateSubKey("发送图片...(S)");
                    associatedKey = rightCommandKey.CreateSubKey("command");
                    associatedKey.SetValue("", $"\"{Process.GetCurrentProcess().MainModule.FileName}\" -image \"%1\"");
                    //////
                    registryKey = Registry.ClassesRoot.OpenSubKey(@"jpegfile\shell", true);
                    if (registryKey == null) registryKey = Registry.ClassesRoot.CreateSubKey(@"jpegfile\shell");
                    rightCommandKey = registryKey.CreateSubKey("发送图片...(S)");
                    associatedKey = rightCommandKey.CreateSubKey("command");
                    associatedKey.SetValue("", $"\"{Process.GetCurrentProcess().MainModule.FileName}\" -image \"%1\"");
                }
            }
            else
            {
                Console.WriteLine(args[1]);
                var qiniuNetdisk = new QiniuNetdisk("S5QEg5KyY9vZ__hm7UPTqAU9wSmBPnGx_9z9RGNU", "JGukVifLvx--kmdTihJGFyKbvop89EqKzZUIl1wS");
                string c = "D:\\Program Source\\AwesomeProjram";
                string b = $"{c}\\BackupFiles";
                Directory.CreateDirectory(b);
                string t=DateTime.Now.ToString("MMddHHmmss");
                Directory.CreateDirectory(c);
                string k = args[0];
                string p = args[1];
                if (k == "-directory")
                {
                    string f = c + $"\\{DateTime.Now:MMddHHmmss}.zip";
                    if (File.Exists(f)) File.Delete(f);
                    Console.WriteLine(f);
                    System.IO.Compression.ZipFile.CreateFromDirectory(p, f);
                    var ff = new FileInfo(f);
                    if (ff.Length < 1048576L * 50L)
                    {
                        qiniuNetdisk.Upload(f);
                    }
                }
                else if (k == "-file")
                {
                    var ff = new FileInfo(p);
                    ff.CopyTo($"{b}\\{t}.{Path.GetExtension(p)}");
                    if (ff.Length < 1048576L * 50L)
                        qiniuNetdisk.Upload(p);
                }
                else if (k == "-image")
                {
                    new SMMSImageHost("awesomehhhhh", "070304syz").Upload(p);
                }
            }
        }
    }
}
