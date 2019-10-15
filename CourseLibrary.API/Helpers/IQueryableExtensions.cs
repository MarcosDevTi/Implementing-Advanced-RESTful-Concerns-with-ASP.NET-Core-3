using CourseLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CourseLibrary.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, (string main, PropertyMappingValue)[] mappings)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (mappings == null)
                throw new ArgumentNullException(nameof(mappings));
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            var orderByAfterSplit = orderBy.Split(",");

            foreach(var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();

                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if(!mappings.Any(_ => _.main == propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                (string main, PropertyMappingValue)? propertyMappingValue = mappings.FirstOrDefault(_ => _.main == propertyName);

                if (propertyMappingValue == null)
                    throw new ArgumentNullException("propertyMappingValue");

                foreach (var destinationProperty in propertyMappingValue.Value.Item2.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Value.Item2.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }
    }
}
