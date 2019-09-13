using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildObjects.MapObjects;
using BuildObjects.Utility;

namespace BuildObjects
{
    public static class ReadObject
    {
        public static ViableMapAndData LoadMap(string fileName)
        {
            ViableMapAndData loadedMap = null;
            FileSerializations.FileDeserialization(fileName, ref loadedMap);
            return loadedMap;
        }

        public static MapedObject LoadObjectOnly(string fileName)
        {
            MapedObject loadObject = null;
            FileSerializations.FileDeserialization(fileName, ref loadObject);
            return loadObject;
        }

        public static DataTable LoadDataOnly(string fileName)
        {
            DataTable data = null;
            FileSerializations.FileDeserialization(fileName, ref data);
            return data;
        }
    }
}
