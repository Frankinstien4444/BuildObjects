using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildObjects.Enums;
using BuildObjects.MapObjects;

namespace BuildObjects.Utility
{
    public static class ParameterExtruder
    {
        public const string Constructor = ".ctor";

        public static List<Object[]> BuildParameterArrayList(ViableMapAndData aMap, string methodName)
        {
            List<Object[]> parameterList = new List<object[]>();            
            var methodStores = aMap.StoredData.Where(x => x.MapType.Equals(DataMapType.MethodParameterMap));
            if (methodStores.Any())
            {
                var parameterStores = methodStores.Where(x => x.DataStoreName.Equals(methodName));
                if (parameterStores != null)
                {
                    foreach (DataStore aStore in parameterStores)
                    {
                        if (aStore.ActualData != null)
                        {
                            MethodParameterMap aParamMap = ParameterExtruder.GetParameterMap(methodName, aMap.TheMap, aStore.ActualData.Columns.Count);
                            foreach (DataRow aRow in aStore.ActualData.Rows)
                            {
                                object[] parameters = ParameterExtruder.ExtractParameters(aParamMap, aRow);
                                parameterList.Add(parameters);
                            }
                        }
                        else
                            parameterList.Add(null);
                    }
                }                
            }

            return parameterList;
        }
        public static Object[] ExtractParameters(MethodParameterMap paramMap, DataRow aRow)
        {
             
            Object[] parameters = new Object[paramMap.ParameterCount];
            
            if (paramMap.ColumnParameterMaps.Count> 0)
            {
                ExtractColumnParameters(parameters, paramMap.ColumnParameterMaps, aRow);
            }
            else if(paramMap.ParameterMaps.Count > 0)
            {
                ExtractColumnParameters(parameters, paramMap.ParameterMaps, aRow);
            }
            else
                parameters = null;

            return parameters;
        }

        public static MethodParameterMap GetParameterMap(String methodName, MapedObject aMap, int paramterCount)
        {
            return aMap.MethodParameters.Where(x => x.MethodName.Equals(methodName) && x.ParameterCount.Equals(paramterCount)).FirstOrDefault();
        }        

        private static object[] ExtractColumnParameters(Object[] parameters, List<ParameterMap> paramMaps, DataRow aRow)
        {
            int count = -1;
            foreach (ParameterMap paired in paramMaps)
            {
                count++;
                if (paired.IsNullable)
                {
                    parameters[count] = NullableTypes(paired, aRow.Field<object>(paired.ParameterName));
                }
                else
                    parameters[count] = aRow.Field<object>(paired.ParameterName);
            }

            return parameters;
        }

        private static object[] ExtractColumnParameters(Object[] parameters, List<ParameterColumnMap> paramMaps, DataRow aRow)
        {
            int count = -1;
            foreach (ParameterColumnMap paired in paramMaps)
            {
                count++;
                if (paired.IsNullable)
                {
                    parameters[count] = NullableTypes(paired, aRow.Field<object>(paired.ColumnName));
                }
                else
                    parameters[count] = aRow.Field<object>(paired.ColumnName);
            }

            return parameters;
        }

        private static Object NullableTypes(InfoMap paired, Object dataPoint)
        {
            Object result = null;
            switch (paired.DataType)
            {
                case "System.Double":
                    Double? newDouble = new Double?((double)dataPoint);
                    result = newDouble;
                    break;
                case "System.Integer":
                    int? newInteger = new int?((int)dataPoint);
                    result = newInteger;
                    break;
                case "System.Long":
                    long? newLong = new long?((long)dataPoint);
                    result = newLong;
                    break;
                case "System.Single":
                    Single? newSingle = new Single?((Single)dataPoint);
                    result = newSingle;
                    break;
                case "System.Float":
                    float? newFloat = new float?((float)dataPoint);
                    result = newFloat;
                    break;
                case "System.Decimal":
                    Decimal? newDecimal = new Decimal?((decimal)dataPoint);
                    result = newDecimal;
                    break;

                case "System.DateTime":
                    DateTime? newDate = new DateTime?((DateTime)dataPoint);
                    result = newDate;
                    break;
            }

            return result;
        }
    }
}
