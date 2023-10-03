namespace Luveck.Service.Administration.DTO
{
    public class FileRequestDto
    {
        public int productId { get; set; }
        public string Name { get; set; }
        public string FileBase64 { get; set; }
        public string TypeFile { get; set; }
    }
}
