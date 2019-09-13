using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BuildObjects.Enums;

namespace BuildObjects.MapObjects
{
    [Serializable]
    public class MapedObject
    {
        [DataMember]
        public String ClassName { set; get; }
        [DataMember]
        public List<PropertyColumnMap> PropMaps { set; get; }
        [DataMember]
        public List<FieldColumnMap> FieldMaps { set; get; }        
        [DataMember]
        public List<MethodParameterMap> MethodParameters{ set; get; }        
        public MapedObject()
        {
            PropMaps = new List<PropertyColumnMap>();
            FieldMaps = new List<FieldColumnMap>();            
            MethodParameters = new List<MethodParameterMap>();
        }    
    }

    [Serializable]
    public class MethodParameterMap
    {
        [DataMember]
        public String MethodName { set; get; }
        [DataMember]
        public List<ParameterMap> ParameterMaps { set; get; }
        [DataMember]
        public List<ParameterColumnMap> ColumnParameterMaps { set; get; }
        [DataMember]
        public int ParameterCount { set; get; }        

        public MethodParameterMap()
        {
            ColumnParameterMaps = new List<ParameterColumnMap>();
            ParameterMaps = new List<ParameterMap>();
        }
    }

    [Serializable]
    public class ParameterMap : InfoMap
    {
        [DataMember]
        public Guid ParameterID { set; get; }
        [DataMember]
        public String ParameterName { set; get; }
    }

    [Serializable]
    public class InfoMap
    {
        [DataMember]
        public String DataType { set; get; }
        [DataMember]
        public Boolean IsNullable { set; get; }
        [DataMember]
        public bool RuleBased { set; get; }
    }

    [Serializable]
    public class ColumnMap :InfoMap
    {
        [DataMember]
        public String ColumnName { set; get; }
    }

    [Serializable]
    public class PropertyColumnMap : ColumnMap
    {        
        [DataMember]
        public String PropertyName { set; get; }       
       
    }

    [Serializable]
    public class FieldColumnMap : ColumnMap
    {        
        [DataMember]
        public String FieldName { set; get; }    
     
    }

    [Serializable]
    public class ParameterColumnMap : ColumnMap
    {
        [DataMember]
        public String ParameterName { set; get; }        
    }

    [Serializable]
    public class DataStore
    {
        [DataMember]
        public String DataStoreName { set; get; }
        [DataMember]
        public DataMapType MapType { set; get; }
        [DataMember]
        public DataTable ActualData { set; get; }
    }

    [Serializable]
    public class ViableMapAndData
    {
        [DataMember]
        public Guid ID { set; get; }
        [DataMember]
        public MapedObject TheMap { set; get; }
        [DataMember]
        public List<DataStore> StoredData { set; get; } 
        
        public ViableMapAndData()
        {
            StoredData = new List<MapObjects.DataStore>();
        }
    }
}
