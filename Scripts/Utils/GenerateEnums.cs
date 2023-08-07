
using System.IO;
using Localization.ScriptableObjects;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Utils
{
    public class GenerateEnum
    {
        public static void Generate(string enumName, LocaEntry[] entries)
        {
#if UNITY_EDITOR
            string filePathAndName = "Assets/Scripts/Enums/" + enumName + ".cs"; //The folder Scripts/Enums/ is expected to exist

            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("[System.Serializable]");
                streamWriter.WriteLine("public enum " + enumName);
                streamWriter.WriteLine("{");
                for (int i = 0; i < entries.Length; i++)
                {
                    streamWriter.WriteLine("\t" + entries[i].ID + " = " + entries[i].ValId + ",");
                }
                streamWriter.WriteLine("}");
            }
            AssetDatabase.Refresh();
#endif
        }
    }
}