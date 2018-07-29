using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Moss.Hospital.Data.Extenstion
{
    public class ComboBoxItem
    {
        public object Value { get; set; }
        public object Text { get; set; }
    }

    public static class IListToComboBoxItem
    {
        public static IEnumerable<ComboBoxItem> ToComboBoxItem<T>(this IEnumerable<T> source, string value, string text)
        {
            if (source!=null)
            {
                PropertyInfo propertyValue = typeof(T).GetProperty(value);
                PropertyInfo propertyText = typeof(T).GetProperty(text);
                if (propertyValue != null && propertyText != null)
                {
                    List<ComboBoxItem> items = new List<ComboBoxItem>();
                    foreach (T item in source)
                    {
                        ComboBoxItem comboItem = new ComboBoxItem();
                        comboItem.Value = propertyValue.GetValue(item);
                        comboItem.Text = propertyText.GetValue(item);
                        items.Add(comboItem);
                    }
                    return items;
                }
                else
                {
                    if (propertyValue == null && propertyText == null)
                    {
                        throw new Exception(string.Format("Thuộc tính \"{0}\" và \"{1}\" không tồn tại trong đối tượng \"{2}\"", propertyValue.Name, propertyText.Name, typeof(T).Name));
                    }
                    else
                    {
                        throw new Exception(string.Format("Thuộc tính \"{0}\" không tồn tại trong đối tượng \"{2}\"", propertyValue != null ? propertyValue.Name : propertyText.Name, typeof(T).Name));
                    }
                }
            }
            else
            {
                throw new Exception("DataSource không được NULL");
            }
            
        }
    }
}
