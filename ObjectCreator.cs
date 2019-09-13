using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuildObjects.Enums;
using BuildObjects.MapObjects;
using BuildObjects.Utility;

namespace BuildObjects
{
    public static class ObjectCreator
    {
        /// <summary>
        /// creates objects without parameters in their constructors
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<dynamic> CreateAndInitializePropertiesWoParams(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateAndInitializePropertiesWoParams(fileName, anAssembly);
        }        

        /// <summary>
        /// creates objects without parameters in their constructors
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<dynamic> CreateAndInitializePropertiesWoParams(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateAndInitializeProperties(aMap, anAssembly, null);
        }

        private static List<dynamic> CreateAndInitializePropertiesWoParams(ViableMapAndData aMap, Assembly anAssembly)
        {
            return CreateAndInitializeProperties(aMap, anAssembly, null);
        }


        /// <summary>
        /// creates objects with parameters in their constructors
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        private static List<dynamic> CreateAndInitializeProperties(ViableMapAndData aMap, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateAndInitializeProperties(aMap, anAssembly);
        }

        /// <summary>
        /// creates objects with parameters in their constructors
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<dynamic> CreateAndInitializeProperties(string fileName, string assemblyLocation)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            
            return CreateAndInitializeProperties(aMap, assemblyLocation);
        }
                
        /// <summary>
        /// creates objects with or wihtout parameters in their constructors
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        private static List<dynamic> CreateAndInitializeProperties(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> results = new List<dynamic>();
            List<Object[]> parameterList = ParameterExtruder.BuildParameterArrayList(aMap, ".ctor");
            if (parameterList.Count > 0)
            {
                foreach (Object[] parameters in parameterList)
                    results.AddRange(CreateAndInitializeProperties(aMap, anAssembly, parameters));
            }
            else
                results.AddRange(CreateAndInitializeProperties(aMap, anAssembly, null));


            return results;
        }

        /// <summary>
        /// creates objects without parameters in their constructors
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        private static List<dynamic> CreateAndInitializeProperties(ViableMapAndData aMap, Assembly anAssembly, object[] parameters)
        {
            List<dynamic> result = new List<dynamic>();            
            
            var propertyStore = aMap.StoredData.Where(x => x.MapType.Equals(DataMapType.PropertyColumnMap)).FirstOrDefault();
            foreach (DataRow aRow in propertyStore.ActualData.Rows)
            {
                dynamic aObject = InI.CreateObjectFromAssembly(anAssembly, aMap.TheMap.ClassName, parameters);
                InI.InitializeProperties(aObject, aMap.TheMap, aRow);
                result.Add(aObject);
            }
            return result;
        }

        /// <summary>
        /// intitializes objects with properties and fields from data stores.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<dynamic> CreateAndIntializePropertiesAndFieldsWoParams(String fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateAndIntializePropertiesAndFieldsWoParams(fileName, anAssembly);
        }

        /// <summary>
        /// intitializes objects with properties and fields from data stores.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        public static List<dynamic> CreateAndIntializePropertiesAndFieldsWoParams(String fileName, Assembly anAssembly)
        {           
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateAndIntializePropertiesAndFieldsWoParams(aMap, anAssembly);
        }

        /// <summary>
        /// intitializes objects with properties and fields from data stores.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        public static List<dynamic> CreateAndIntializePropertiesAndFieldsWoParams(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> results = new List<dynamic>();
            results.AddRange(CreateAndInitializePropertiesWoParams(aMap, anAssembly));
            InitializeFields(aMap, results);
            return results;
        }
        /// <summary>
        /// intitializes objects with properties and fields from data stores.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        public static List<dynamic> CreateAndIntializePropertiesAndFields(String fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateAndIntializePropertiesAndFields(aMap, anAssembly);
        }

        /// <summary>
        /// intitializes objects with properties and fields from data stores.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        public static List<dynamic> CreateAndIntializePropertiesAndFields(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> results = new List<dynamic>();
            
            results.AddRange(CreateAndInitializeProperties(aMap, anAssembly));
            InitializeFields(aMap, results);
            return results;
        }

        /// <summary>
        /// intitializes objects fields from data stores.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        private static void InitializeFields(ViableMapAndData aMap, List<dynamic> createdObjects)
        {
            var FieldStore = aMap.StoredData.Where(x => x.MapType.Equals(DataMapType.FieldColumnMap)).FirstOrDefault();
            int count = -1;
            foreach(dynamic anObject in createdObjects)
            {
                InI.InitializeFields(anObject, aMap.TheMap, FieldStore.ActualData.Rows[count]);
            }
        }

        /// <summary>
        /// creates objects and initializes fields wiht no paramters in constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<dynamic> CreateAndInitializeFieldsWoParams(string fileName, String assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateAndInitializeFieldsWoParams(fileName, anAssembly);
        }

        /// <summary>
        /// creates objects and initializes fields wiht no paramters in constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        public static List<dynamic> CreateAndInitializeFieldsWoParams(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateAndInitializeFields(aMap, anAssembly, null);
        }

        /// <summary>
        /// creates objects and initializes fields wiht paramters in constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<dynamic> CreateAndInitializeFields(string fileName, String assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateAndInitializeFields(fileName, anAssembly);
        }

        //creates objects and initializes fields wiht paramters in constructor
        public static List<dynamic> CreateAndInitializeFields(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateAndInitializeFields(aMap, anAssembly);
        }
        /// <summary>
        /// intitializes objects with fields from data stores.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        private static List<dynamic> CreateAndInitializeFields(ViableMapAndData aMap, Assembly anAssembly, object[] parameters)
        {
            List<dynamic> result = new List<dynamic>();            
            var fieldStore = aMap.StoredData.Where(x => x.MapType.Equals(DataMapType.FieldColumnMap)).FirstOrDefault();
            foreach (DataRow aRow in fieldStore.ActualData.Rows)
            {
                dynamic anObject = InI.CreateObjectFromAssembly(anAssembly, aMap.TheMap.ClassName, parameters);
                InI.InitializeFields(anObject, aMap.TheMap,aRow);
 
                 result.Add(anObject);
            }
            return result;
        }

        /// <summary>
        /// creates objects with parameters in their constructors
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        private static List<dynamic> CreateAndInitializeFields(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> result = new List<dynamic>();            
            List<Object[]> parameterList = ParameterExtruder.BuildParameterArrayList(aMap, ".ctor");
            if (parameterList.Count > 0)
            {
                foreach (Object[] parameters in parameterList)
                    result.AddRange(CreateAndInitializeFields(aMap, anAssembly, parameters));
            }
            else
                result.AddRange(CreateAndInitializeFields(aMap, anAssembly, null));
            return result;
        }


        //ViableMapAndData aMap = ReadObject.LoadMap(fileName);
        //Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);

        public static List<Tuple<String, String, object>> CreateInitializeFieldsAndRunMethods(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateInitializeFieldsAndRunMethods(fileName, anAssembly);
        }

        public static List<Tuple<String, String, object>> CreateInitializeFieldsAndRunMethods(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateInitializeFieldsAndRunMethods(aMap, anAssembly);
        }
        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<Tuple<String, String, object>> CreateInitializeFieldsAndRunMethodsWoParams(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateInitializeFieldsAndRunMethodsWoParams(fileName, anAssembly);
        }
        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        public static List<Tuple<String, String, object>> CreateInitializeFieldsAndRunMethodsWoParams(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateInitializeFieldsAndRunMethodsWoParams(aMap, anAssembly);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aMap"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        private static List<Tuple<String, String, object>> CreateInitializeFieldsAndRunMethods(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> injectedObjects = new List<dynamic>();            
            injectedObjects.AddRange(CreateAndInitializeFields(aMap, anAssembly));
            return RunMethods(aMap, injectedObjects);
        }

        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="aMap"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        private static List<Tuple<String, String, object>> CreateInitializeFieldsAndRunMethodsWoParams(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> injectedObjects = new List<dynamic>();
            injectedObjects.AddRange(CreateAndInitializeFields(aMap, anAssembly, null));
            return RunMethods(aMap, injectedObjects);          
        }

        ////////////////////////////////////////////////////////


        public static List<Tuple<String, String, object>> CreateInitializePropertiesAndRunMethods(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateInitializePropertiesAndRunMethods(fileName, anAssembly);
        }

        public static List<Tuple<String, String, object>> CreateInitializePropertiesAndRunMethods(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateInitializePropertiesAndRunMethods(aMap, anAssembly);
        }
        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<Tuple<String, String, object>> CreateInitializePropertiesAndRunMethodsWoParams(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateInitializePropertiesAndRunMethodsWoParams(fileName, anAssembly);
        }
        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        public static List<Tuple<String, String, object>> CreateInitializePropertiesAndRunMethodsWoParams(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateInitializePropertiesAndRunMethodsWoParams(aMap, anAssembly);
        }

        private static List<Tuple<String, String, object>> CreateInitializePropertiesAndRunMethods(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> injectedObjects = new List<dynamic>();
            injectedObjects.AddRange(CreateAndInitializeProperties(aMap, anAssembly));
            return RunMethods(aMap, injectedObjects);
        }

        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="aMap"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        private static List<Tuple<String, String, object>> CreateInitializePropertiesAndRunMethodsWoParams(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> injectedObjects = new List<dynamic>();
            injectedObjects.AddRange(CreateAndInitializeProperties(aMap, anAssembly, null));
            return RunMethods(aMap, injectedObjects);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        public static List<Tuple<String, String, object>> CreateInitializePropertiesFieldsAndRunMethods(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateInitializePropertiesFieldsAndRunMethods(fileName, anAssembly);
        }
        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<Tuple<String, String, object>> CreateInitializePropertiesFieldsAndRunMethodsWoParams(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateInitializePropertiesFieldsAndRunMethodsWoParams(fileName, anAssembly);
        }

        public static List<Tuple<String, String, object>> CreateInitializePropertiesFieldsAndRunMethods(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateInitializePropertiesFieldsAndRunMethods(aMap, anAssembly);
        }

        /// <summary>
        /// No paramters in constuctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        private static List<Tuple<String, String, object>> CreateInitializePropertiesFieldsAndRunMethodsWoParams(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateInitializePropertiesFieldsAndRunMethodsWoParams(aMap, anAssembly);
        }

        private static List<Tuple<String, String, object>> CreateInitializePropertiesFieldsAndRunMethods(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> injectedObjects = new List<dynamic>();
            injectedObjects.AddRange(CreateAndIntializePropertiesAndFields(aMap, anAssembly));
            return RunMethods(aMap, injectedObjects);
        }

        /// <summary>
        /// No parameters in Constructor
        /// </summary>
        /// <param name="aMap"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        private static List<Tuple<String, String, object>> CreateInitializePropertiesFieldsAndRunMethodsWoParams(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<dynamic> injectedObjects = new List<dynamic>();
            injectedObjects.AddRange(CreateAndIntializePropertiesAndFieldsWoParams(aMap, anAssembly));
            return RunMethods(aMap, injectedObjects);
        }



        public static List<Tuple<String, String, object>> CreateObjectInitializeFieldsAndRunMethods(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateObjectInitializeFieldsAndRunMethods(fileName, anAssembly);
        }

        public static List<Tuple<String, String, object>> CreateObjectInitializeFieldsAndRunMethods(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateObjectInitializeFieldsAndRunMethods(aMap, anAssembly);
        }

        private static List<Tuple<String, String, object>> CreateObjectInitializeFieldsAndRunMethods(ViableMapAndData aMap, Assembly anAssembly)
        {
            List<Tuple<String, String, object>> results = new List<Tuple<String, String, object>>();
            List<dynamic> injectedObjects = new List<dynamic>();
            List<Object[]> constructorParams = ParameterExtruder.BuildParameterArrayList(aMap, ParameterExtruder.Constructor);

            foreach(Object[] ctorParameters in constructorParams)
                injectedObjects.Add(CreateAndInitializeFields(aMap, anAssembly, ctorParameters));

            if (injectedObjects.Any())
                results = RunMethods(aMap, injectedObjects);
            
            

            return results;
        }

        /// <summary>
        /// No parameters in constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assemblyLocation"></param>
        /// <returns></returns>
        public static List<Tuple<String, String, object>> CreateObjectAndRunMethodsNoParams(string fileName, string assemblyLocation)
        {
            Assembly anAssembly = Assembly.LoadFrom(assemblyLocation);
            return CreateObjectAndRunMethodsNoParams(fileName, anAssembly);
        }

        /// <summary>
        /// No parameters in constructor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="anAssembly"></param>
        /// <returns></returns>
        public static List<Tuple<String, String, object>> CreateObjectAndRunMethodsNoParams(string fileName, Assembly anAssembly)
        {
            ViableMapAndData aMap = ReadObject.LoadMap(fileName);
            return CreateObjectAndRunMethodsNoParams(aMap, anAssembly, null);
        }

        /// <summary>
        /// No parameters in constructor
        /// </summary>
        /// <param name="aMap"></param>
        /// <param name="anAssembly"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<Tuple<String, String, object>> CreateObjectAndRunMethodsNoParams(ViableMapAndData aMap, Assembly anAssembly, object[] parameters)
        {
            List<dynamic> injectedObjects = new List<dynamic>();
            dynamic anObject = InI.CreateObjectFromAssembly(anAssembly, aMap.TheMap.ClassName, parameters);
            injectedObjects.Add(anObject);
            return RunMethods(aMap, injectedObjects);
        }

        private static List<Tuple<String, String, object>> RunMethods(ViableMapAndData aMap, List<dynamic> objects)
        {
            List<Tuple<String, String, object>> results = new List<Tuple<String, String, object>>();
            var methodStore = aMap.StoredData.Where(x => x.MapType.Equals(DataMapType.MethodParameterMap) && !x.DataStoreName.Equals(ParameterExtruder.Constructor));
            if (methodStore.Any())
            {                
                foreach (DataStore aStore in methodStore)
                {                    
                    foreach (DataRow aRow in aStore.ActualData.Rows)
                    {
                        foreach (dynamic anObject in objects)
                        {
                            MethodInfo method = anObject.GetType().GetMethod(aStore.DataStoreName);

                            List<object[]> paramList = ParameterExtruder.BuildParameterArrayList(aMap, method.Name);
                            if (paramList.Count > 0)
                            {
                                foreach (Object[] parameters in paramList)
                                {
                                    results.Add(InvokeMethod(method, anObject, parameters, aStore.DataStoreName));
                                }
                            }
                            else
                            {
                                results.Add(InvokeMethod(method, anObject, null, aStore.DataStoreName));
                            }
                         }
                    }
                }               
            }

            return results;
        }

        private static Tuple<String, String, object> InvokeMethod(MethodInfo method, dynamic anObject, Object[] parameters, string methodName)
        {
            object methodResult = method.Invoke(anObject, parameters);

            Tuple<String, String, object> oneResult = new Tuple<string, string, object>(methodName, ParameterNames.GetParameterValues(parameters), methodResult);
            return oneResult;
        }
    }
}
