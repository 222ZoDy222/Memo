using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saver;
using System.IO;
using System.Text;
using UnityEngine.Android;

namespace Saver
{
    public class SaveManager : MonoBehaviour
    {

        public static SaveManager instance;



        private void Awake()
        {
            if (instance != null) Destroy(instance);
            instance = this;
        }


        public static string pathFolder
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Application.persistentDataPath;
#else
                return Application.dataPath;
#endif
            }
        }



        // Start is called before the first frame update
        void Start()
        {



        }


        
        public static string[] GetFiles(string folderName)
        {
            string fullPath = Path.Combine(pathFolder, folderName);
            if (Directory.Exists(fullPath))
            {
                return Directory.GetFiles(fullPath);
            }
            return null;
        }

        public static void DeleteFile(string fullPath)
        {
            if (!File.Exists(fullPath)) return;

            File.Delete(fullPath);

        }

        public static void UpdateSave<T>(string fileName, string fromPath, string fullPath, T obj)
        {
            if (obj == null) return;

            if(fromPath != null)
            {
                Directory.Move(fromPath, fullPath);
            }

            fileName += ".json";
            if (fullPath != null) fullPath = Path.Combine(fullPath, fileName);
            else Path.Combine(pathFolder, fileName);


            //Convert To Json then to bytes
            string jsonData = JsonUtility.ToJson(obj, true);
            if (jsonData == null) return;

            byte[] jsonByte = Encoding.UTF8.GetBytes(jsonData);

            // If haven't permission to Write data to system
            if (!HavePermission())
            {

                // start Gettings permission to Write and Read
                instance.dataToSaves.Enqueue(new DataToSave()
                {
                    fullPath = fullPath,
                    jsonByte = jsonByte,
                });

                if (instance.permissionRequestRoutine != null)
                {
                    instance.StopCoroutine(instance.permissionRequestRoutine);
                }
                instance.permissionRequestRoutine = instance.StartCoroutine(instance.SaveDataCycle());
            }
            else
            {
                Saver.Save(jsonByte, fullPath);
            }

        }

        public static void Save<T>(string fileName, string fullPath, T obj)
        {
            if (obj == null) return;
            
            fileName += ".json";
            if (fullPath != null) fullPath = Path.Combine(fullPath, fileName);
            else Path.Combine(pathFolder, fileName);


            //Convert To Json then to bytes
            string jsonData = JsonUtility.ToJson(obj, true);
            if (jsonData == null) return;
            
            byte[] jsonByte = Encoding.UTF8.GetBytes(jsonData);

            // If haven't permission to Write data to system
            if (!HavePermission())
            {

                // start Gettings permission to Write and Read
                instance.dataToSaves.Enqueue(new DataToSave()
                {
                    fullPath = fullPath,
                    jsonByte = jsonByte,
                });

                if (instance.permissionRequestRoutine != null)
                {
                    instance.StopCoroutine(instance.permissionRequestRoutine);
                }
                instance.permissionRequestRoutine = instance.StartCoroutine(instance.SaveDataCycle());
            }
            else
            {
                Saver.Save(jsonByte, fullPath);
            }

        }

        public static T Load<T>(string fullPath)
        {
            if (!File.Exists(fullPath)) return default(T);
            T saved;
            try
            {
                saved = JsonUtility.FromJson<T>(File.ReadAllText(fullPath));
            }
            catch
            {
                saved = default;
            }


            return saved;
        }


        public static void Delete(string fullPath)
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                //Directory.Delete(fullPath);
            }
        }

        public static bool HavePermission()
        {
            //return PermissionChecker.FileReadWrite;
            return (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) && Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite));
        }


        private Coroutine permissionRequestRoutine;

        IEnumerator SaveDataCycle()
        {

            while (dataToSaves.Count != 0)
            {
                if (HavePermission())
                {
                    DataToSave data = dataToSaves.Dequeue();
                    Saver.Save(data.jsonByte, data.fullPath);
                }
                else
                {
                    //PermissionChecker.RequestFilePermissions();
                    Permission.RequestUserPermissions(PermissionsToReadWrite);
                }
                yield return new WaitForSeconds(3f);



            }

        }

        private readonly string[] PermissionsToReadWrite = new string[2]
        {
        Permission.ExternalStorageRead,
        Permission.ExternalStorageWrite,
        };

        private Queue<DataToSave> dataToSaves = new Queue<DataToSave>();

        private struct DataToSave
        {
            public byte[] jsonByte;
            public string fullPath;
        }

    }
}

