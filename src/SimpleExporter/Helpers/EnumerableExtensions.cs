using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleExporter.Helpers
{
    public static class EnumerableExtensions
    {
        public static Type GetItemType(this IEnumerable source)
        {
            // If the list implements IEnumerable<T> get the type from the generic
            // declaration so that we don't query the a query that could take
            // some time to connect
            var itemType = (from i in source.GetType().GetInterfaces()
                where i.IsGenericType &&
                      i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                select i.GetGenericArguments().Single()).SingleOrDefault();


            // If the itemType is Object, we assume they are using an anonymous type
            // and need to get the type of the first item, instead of the IEnumerable
            if (itemType != null && itemType != typeof(object)) return itemType;

            var firstItem = source.OfType<object>().FirstOrDefault();
            return firstItem?.GetType();
        }
    }
}