using McCrypt;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace McEncryptor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- McEncryptor --");

            string runningInFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string keysDbFile = Path.Combine(runningInFolder, "keys.db");
            Directory.SetCurrentDirectory(runningInFolder);

            if (File.Exists(keysDbFile))
            {
                Console.WriteLine("Parsing Key Cache File. (keys.db)");
                Keys.ReadKeysDb(keysDbFile);
            }

            Console.WriteLine("Path to pack file: ");
            string packPath = Console.ReadLine();

            string uuid = Manifest.ReadUUID(Path.Combine(packPath, "manifest.json"));

            byte[] ckey = Keys.LookupKey(uuid);
            string contentKey = "s5s5ejuDru4uchuF2drUFuthaspAbepE";
            if (ckey != null)
                contentKey = Encoding.UTF8.GetString(ckey);

            Console.WriteLine("uuid: " + uuid);
            Manifest.SignManifest(packPath);
            Marketplace.EncryptContents(packPath, uuid, contentKey);


        }
    }
}
