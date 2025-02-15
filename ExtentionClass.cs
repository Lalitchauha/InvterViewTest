using System.Data;
using System.Reflection;

namespace InvterViewTest
{
    public static class ExtentionClass
    {
        public static List<T> ToListFromDataTable<T>(this DataTable dataTable) where T : new()
        {
            var dataList = new List<T>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            //Read Attribute Names and Types
            var objFieldNames = typeof(T).GetProperties(flags).Cast<PropertyInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                }).ToList();

            //Read Datatable column names and types
            var dtlFieldNames = dataTable.Columns.Cast<DataColumn>().
                Select(item => new
                {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var classObj = new T();

                foreach (var dtField in dtlFieldNames)
                {
                    PropertyInfo propertyInfos = classObj.GetType().GetProperty(dtField.Name.ToLower());

                    var field = objFieldNames.Find(x => x.Name.ToLower() == dtField.Name.ToLower());

                    if (field != null)
                    {

                        if (propertyInfos.PropertyType == typeof(DateTime) || propertyInfos.PropertyType == typeof(DateTime?))
                        {
                            propertyInfos.SetValue
                            (classObj, convertToDateTime(dataRow[dtField.Name.ToLower()]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(int))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToInt(dataRow[dtField.Name.ToLower()]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(long))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToLong(dataRow[dtField.Name.ToLower()]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(double))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToDouble(dataRow[dtField.Name.ToLower()]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(decimal))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToDecimal(dataRow[dtField.Name.ToLower()]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(Boolean))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToBoolean(dataRow[dtField.Name.ToLower()]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(String))
                        {
                            if (dataRow[dtField.Name.ToLower()].GetType() == typeof(DateTime))
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToDateString(dataRow[dtField.Name.ToLower()]), null);
                            }
                            else
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToString(dataRow[dtField.Name.ToLower()]), null);
                            }
                        }
                    }
                }
                dataList.Add(classObj);
            }
            return dataList;
        }
        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return Convert.ToString((Convert.ToDateTime(date)));
        }

        private static string ConvertToString(object value)
        {
            return Convert.ToString(HelperFunctions.ReturnEmptyIfNull(value));
        }

        private static int ConvertToInt(object value)
        {
            return Convert.ToInt32(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static long ConvertToLong(object value)
        {
            return Convert.ToInt64(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static double ConvertToDouble(object value)
        {
            return Convert.ToDouble(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static decimal ConvertToDecimal(object value)
        {
            return Convert.ToDecimal(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static Boolean ConvertToBoolean(object value)
        {
            return Convert.ToBoolean(HelperFunctions.ReturnZeroIfNull(value));
        }

        private static DateTime convertToDateTime(object date)
        {
            return Convert.ToDateTime(HelperFunctions.ReturnDateTimeMinIfNull(date));
        }

    }
}
