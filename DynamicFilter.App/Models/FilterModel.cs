using System;

namespace DynamicFilter.App.Models
{
    public class FilterModel
    {
        public FilterModel(string propertyname, Type valueType, object value, string methodName)
        {
            PropertyName = propertyname;
            ValueType = valueType;
            Value = value;
            MethodName = methodName;
        }


        public string PropertyName { get; set; }
        public Type ValueType { get; set; }
        public object Value { get; set; }
        public string MethodName { get; set; }
    }
}


//public class Person
//{
//    ["Caption"]
//    public string Name { get; set; }
//    ["Contains"]
//    public string LastName { get; set; }
//    ["equal"]
//    public int Age { get; set; }

//    Person p = new Person();
//}

