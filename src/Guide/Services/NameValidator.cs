using Guide.Core.Services;
using Guide.Extensions;

namespace Guide.Services
{
    public sealed class NameValidator : INameValidator
    {
        public bool IsValid(string name)
        {
            if(string.IsNullOrEmpty(name))
                return false;

            return name[0].IsAsciiPrintable();
        }
    }
}
