using System.Diagnostics.CodeAnalysis;

namespace Luveck.Service.Administration.Models.Dto
{
    [ExcludeFromCodeCoverage]
    public class ResponseModelDto<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Messages { get; set; }
        public T Result { get; set; }
    }
}
