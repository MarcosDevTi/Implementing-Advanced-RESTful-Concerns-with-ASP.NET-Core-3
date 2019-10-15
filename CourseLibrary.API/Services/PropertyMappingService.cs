using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private (string, PropertyMappingValue)[] _authorPropertyMapping =>
            new[]
            {
                Create("Id"),
                Create("MainCategory"),
                Create("DateOfBirth", true),
                Create("Name", false, "FirstName", "LastName"),
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            var fieldAfterSplit = fields.Split(',');

            foreach(var field in fieldAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if(!propertyMapping.Any(_ => _.main == propertyName))
                {
                    return false;
                }
            }
            return true;
        }
        private (string, PropertyMappingValue) Create(string mainPropertyName, bool revert = false, params string[] propertyNames)
        {
            var props = propertyNames.Length > 0 ? propertyNames.ToList() : new List<string>() { mainPropertyName };
            return (mainPropertyName, new PropertyMappingValue(props, revert));
        }
        public (string main, PropertyMappingValue)[] GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().Mappings;
            }
            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}>");
        }
    }
}
