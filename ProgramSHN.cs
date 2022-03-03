
using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.IO.Compression;

namespace OS_1
{
    class user
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public int Age { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите пункт : \n" +
                    "1. Вывести информацию о логических дисках. \n" +
                    "2. Работа с файлами. \n" +
                    "3. Работа с форматом JSON. \n" +
                    "4. Работа с форматом XML. \n" +
                    "5. Работа с zip архивом. \n" +
                    "6. Выход.");

                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 1)
                    First();
                else if (choice == 2)
                    Second();
                else if (choice == 3)
                    Third();
                else if (choice == 4)
                    Fourth();
                else if (choice == 5)
                    Fifth();
                else if (choice == 6)
                    Environment.Exit(0);

                else
                {
                    Console.WriteLine("Пожалуйста, выберите один из предложенных пунктов 1 - 5.\n");
                    continue;
                }
            }
        }
        static void First()
        {
            Console.WriteLine("1.");
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine("Название: {0}", drive.Name);
                Console.WriteLine("Тип диска: {0}", drive.DriveType);
                if (drive.IsReady)
                {
                    Console.WriteLine("Метка тома: {0}", drive.VolumeLabel);
                    Console.WriteLine("Ёмкость: {0} байт ({1:F} Гбайт)", drive.TotalSize, drive.TotalSize / 1073741824.0);
                    Console.WriteLine("Свободно: {0} байт ({1:F} Гбайт)", drive.TotalFreeSpace, drive.TotalFreeSpace / 1073741824.0);
                    Console.WriteLine("Доступно для текущего пользователя: {0} байт ({1:F} Гбайт)", drive.AvailableFreeSpace, drive.AvailableFreeSpace / 1073741824.0);
                    Console.WriteLine("Файловая система: {0}", drive.DriveFormat);
                }
                Console.WriteLine();
            }
        }
        static void Second()
        {
            Console.WriteLine("2.");
            string path = "TXTfile.txt";
            Console.WriteLine("Введите текст для записи в файл");
            string str = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(str);
            }
            Console.WriteLine("Текст из файла");
            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            FileInfo fileInf = new FileInfo(path);
            fileInf.Delete();
            Console.WriteLine();
        }
        static void Third()
        {
            string path = "JSONfile.json";

            user user1 = new user { Name = "Anatoly", Company = "Barbershop <<Natasha>>", Age = 27 };
            string json1 = JsonSerializer.Serialize<user>(user1);

            user user2 = new user { Name = "Ivan", Company = "Public Space <<Gnomes>>", Age = 19 };
            string json2 = JsonSerializer.Serialize<user>(user2);


            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(json1);
                sw.Write(json2);
            }

            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            File.Delete(path);
            Console.WriteLine();
        }
        static void Fourth()
        {
            string path = "CV.xml";

            XDocument xmlDoc = new XDocument();

            XElement customer = new XElement("Person");

            Console.Write("Введите Ваше имя: ");
            XAttribute userName = new XAttribute("Name", Console.ReadLine());


            Console.Write("Введите название компании: ");
            XElement userCompanyElem = new XElement("Company", Console.ReadLine());

            Console.Write("Введите Ваш возраст: ");
            XElement userAgeElem = new XElement("Age", Convert.ToInt32(Console.ReadLine()));

            customer.Add(userName);
            customer.Add(userCompanyElem);
            customer.Add(userAgeElem);
            xmlDoc.Add(customer);

            xmlDoc.Save("CV.xml");

            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            File.Delete(path);

        }
        static void Fifth()
        {
            string zipPath = "ZIPfile.zip";
            using (FileStream file = new FileStream(zipPath, FileMode.OpenOrCreate))
            {
                using ZipArchive zipfile = new ZipArchive(file, ZipArchiveMode.Update);

                string path = "";
                while (!File.Exists(path))
                {
                    Console.Write("Введите путь к файлу для добавление в архив: ");
                    path = Console.ReadLine();
                }

                zipfile.CreateEntryFromFile(path, path.Substring(path.LastIndexOf('\\') + 1));

                zipfile.ExtractToDirectory("fromZIPfile");

                path = "fromZIPfile\\" + path.Substring(path.LastIndexOf('\\') + 1);
                FileInfo fileInf = new FileInfo(path);
                Console.WriteLine("Имя файла: {0}\nВремя создания: {1:F}\nРазмер: {2} байт", fileInf.Name, fileInf.CreationTime, fileInf.Length);
            }




            Directory.Delete("fromZIPfile", true);
            File.Delete(zipPath);
            Console.WriteLine();
        }
    }
}