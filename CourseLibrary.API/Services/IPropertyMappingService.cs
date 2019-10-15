namespace CourseLibrary.API.Services
{
    public interface IPropertyMappingService
    {
        (string main, PropertyMappingValue)[] GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}