using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;


namespace Saver
{
    public class Saver
    {








        public static void Save(byte[] jsonByte, string fullPath)
        {

            //Create Directory if it does not exist
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            Thread thread = new Thread(() =>
            {
                SaveFile(fullPath, jsonByte);
            });

            thread.Start();


        }

        //"/storage/emulated/0/Android/data/com.LSD.LSDMobile/files/"

        private static void SaveFile(string fullPath, byte[] content)
        {
            File.WriteAllBytes(fullPath, content);

        }

        public static string[] GetFilesFrom(string fullPath)
        {

            if (Directory.Exists(fullPath))
            {
                return Directory.GetFiles(fullPath);                
            }
            else
            {
                return null;
            }

        }

        public static string[] GetDirectoriesFrom(string fullPath)
        {

            if (Directory.Exists(fullPath))
            {
                return Directory.GetDirectories(fullPath);
            }
            else
            {
                return null;
            }

        }









        public class JsonSaves<T>
        {
            public List<T> array;


        }

    }

}
