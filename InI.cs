using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuildObjects.MapObjects;
using BuildObjects.Utility;

namespace BuildObjects
{
    /// <summary>
    /// Class Instantiates and Initializes through reflection
    /// </summary>
    public static class InI
    {
        public static dynamic CreateGeneric(string genericClass, string assemblyLocation, string type, object[] parameters)
        {
            dynamic result = null;
            Assembly SampleAssembly = null;
            AssemblyAccessor.LoadAssembly(assemblyLocation, ref SampleAssembly);
            
            Type foundOne = AssemblyAccessor.GetAssemblyType(ref SampleAssembly, type);
            if(foundOne != null)
            { 
               Type genericType = Type.GetType(genericClass);
               Type[] typeArgs = { foundOne };
               var createdType = genericType.MakeGenericType(typeArgs);
                if (parameters == null)
                    result = Activator.CreateInstance(createdType);
                else
                    result = Activator.CreateInstance(createdType, parameters);
            }

            return null;
        }

        public static dynamic CreateGeneric(string genericClass, string type, object[] parameters)
        {
            dynamic result = null;
            Type genericType = Type.GetType(genericClass);
            Type[] typeArgs = { Type.GetType(type)};
            var createdType = genericType.MakeGenericType(typeArgs);
            if(parameters == null)
                result = Activator.CreateInstance(createdType);
            else
                result = Activator.CreateInstance(createdType, parameters);

            return result;
        }

        public static dynamic CreateObjectFromAssembly(string assemblyLocation, string type, object[] parameters)
        {
            Assembly sampleAssembly = null;
            AssemblyAccessor.LoadAssembly(assemblyLocation, ref sampleAssembly);
            return CreateObjectFromAssembly(sampleAssembly, type, parameters);
        }

        public static dynamic CreateObjectFromAssembly(Assembly sampleAssembly, string type, object[] parameters)
        {
            dynamic result = null;
            if (sampleAssembly != null)
            {
                Type foundOne = sampleAssembly.GetTypes().Where(x => x.FullName.Equals(type)).FirstOrDefault();
                if (parameters == null)
                    result = Activator.CreateInstance(foundOne);
                else
                    result = Activator.CreateInstance(foundOne, parameters);
            }

            return result;
        }

        public static void InitializeFields(dynamic currentObject, MapedObject anObject, DataRow aRow)
        {
            List<FieldInfo> allFields = AssemblyAccessor.GetClassFields(currentObject.GetType(), anObject.FieldMaps);
            foreach(FieldColumnMap aMap in anObject.FieldMaps)
            {
                FieldInfo aField = allFields.Where(x => x.Name.Equals(aMap.FieldName)).FirstOrDefault();
                if(aField != null)
                {
                    aField.SetValue(currentObject, aRow.Field<object>(aMap.ColumnName));
                }
            }
        }

        public static void InitializeProperties(dynamic currentObject, MapedObject anObject, DataRow aRow)
        {
            List<PropertyInfo> allProps = AssemblyAccessor.GetClassProperties(currentObject.GetType(), anObject.PropMaps);
            foreach (PropertyColumnMap aMap in anObject.PropMaps)
            {
               PropertyInfo aProp = allProps.Where(x => x.Name.Equals(aMap.PropertyName)).FirstOrDefault();
               if (aProp != null)
               {
                   aProp.SetValue(currentObject, aRow.Field<object>(aMap.ColumnName), null);
               }
            }            
        }
    }
}
