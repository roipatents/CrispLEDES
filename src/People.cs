using System.Collections.ObjectModel;

namespace FreshbooksLedesConverter;

public class People() : KeyedCollection<string, Person>(StringComparer.InvariantCultureIgnoreCase)
{
    protected override string GetKeyForItem(Person item)
    {
        return item.Name;
    }
}
