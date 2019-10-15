using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public class PropertyMapping<TSource, TDestination>: IPropertyMapping
    {
        public (string, PropertyMappingValue)[] Mappings { get; private set; }
        public PropertyMapping((string, PropertyMappingValue)[] mappings)
        {
            Mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
        }
    }
}
