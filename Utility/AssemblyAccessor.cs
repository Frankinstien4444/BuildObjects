using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BuildObjects.MapObjects;

namespace BuildObjects.Utility
{
    public static class AssemblyAccessor
    {
        public static void LoadAssembly(String assemblyLocation, ref Assembly SampleAssembly)
        {
            try
            {
                SampleAssembly = Assembly.LoadFrom(assemblyLocation);
            }
            catch(Exception e)
            {

            }
        }

        public static Type GetAssemblyType(ref Assembly SampleAssembly, string type)
        {
            Type foundOne = null;

            if (SampleAssembly != null)
            {
                foundOne = SampleAssembly.GetTypes().Where(x => x.FullName.Equals(type)).FirstOrDefault();
            }

            return foundOne;
        }

        public static List<FieldInfo> GetClassFields(Type anObject, List<FieldColumnMap> fields)
        {
            List<FieldInfo> fieldList = new List<FieldInfo>();
            foreach(FieldColumnMap aMap in fields)
            {
                fieldList.Add(anObject.GetField(aMap.FieldName));
            }

            return fieldList;
        }

        public static List<PropertyInfo> GetClassProperties(Type anObject, List<PropertyColumnMap> properties)
        {
            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            foreach (PropertyColumnMap aMap in properties)
            {
                propertyList.Add(anObject.GetProperty(aMap.PropertyName));
            }

            return propertyList;
        }
    }
}
