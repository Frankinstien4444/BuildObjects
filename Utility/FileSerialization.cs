using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BuildObjects.Utility
{
    public static class FileSerializations
    {
        public static void FileDeserialization<T>(String filePath, ref T loadedFile)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                loadedFile = (T)bFormatter.Deserialize(stream);
                stream.Flush();
            }
        }

        public static void FileSerialization(String filePath, Object dataObject)
        {
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                var bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, dataObject);
                stream.Flush();
            }
        }

        public static void WriteBinaryFile(String filePath, Byte[] fileCode)
        {
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                BinaryWriter writeFile = new BinaryWriter(stream);
                writeFile.Write(fileCode);
                writeFile.Flush();
            }
        }
    }
}
