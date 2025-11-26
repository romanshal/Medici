using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Medici.Behaviors.Validation")]
namespace Medici.Extensions
{
    internal static class TypeExtensions
    {
        internal static bool IsRealisation(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        internal static IEnumerable<Type> GetImplementableInterfaces(this Type targetType, Type templateType) =>
            GetImplementableInterfacesCore(targetType, templateType).Distinct();

        internal static bool IsCastTo(this Type targetType, Type templateType)
        {
            if (targetType == null || templateType == null) return false;

            if (targetType == templateType) return true;

            return templateType.IsAssignableFrom(targetType);
        }

        private static IEnumerable<Type> GetImplementableInterfacesCore(Type pluggedType, Type templateType)
        {
            if (pluggedType == null) yield break;

            if (!pluggedType.IsRealisation()) yield break;

            if (templateType.IsInterface)
            {
                foreach (
                    var interfaceType in pluggedType.GetInterfaces()
                        .Where(type => type.IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.BaseType!.IsGenericType && (pluggedType.BaseType!.GetGenericTypeDefinition() == templateType))
            {
                yield return pluggedType.BaseType!;
            }

            if (pluggedType.BaseType == typeof(object)) yield break;

            foreach (var interfaceType in GetImplementableInterfacesCore(pluggedType.BaseType!, templateType))
            {
                yield return interfaceType;
            }
        }
    }
}
