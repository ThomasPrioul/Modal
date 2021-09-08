using System.Collections.Generic;

namespace Blazored.Modal
{
    public class ModalParameters : Dictionary<string, object>
    {
        public T Get<T>(string parameterName)
        {
            if (TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }
            
            throw new KeyNotFoundException($"{parameterName} does not exist in modal parameters");
        }

        public T TryGet<T>(string parameterName)
        {
            if (TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }

            return default;
        }
    }
}
