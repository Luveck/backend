using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Luveck.Service.Administration.Models.Dto
{
    /// <summary>
    /// Class BlobDto.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BlobDto
    {
        /// <summary>
        /// Gets or sets the contenido base64.
        /// </summary>
        /// <value>The contenido base64.</value>
        [Required]
        public string ContentsBase64 { get; set; }

        /// <summary>
        /// Nombre del contenedor que debe almacenar el archivo
        /// </summary>
        /// <value>The contenedor.</value>
        [Required]
        public string Container { get; set; }

        /// <summary>
        /// Gets or sets the identifier requerimiento.
        /// </summary>
        /// <value>The identifier requerimiento.</value>
        [Required]
        public string PathBlob { get; set; }

        /// <summary>
        /// Gets or sets the name of the BLOB.
        /// </summary>
        /// <value>The name of the BLOB.</value>
        public string BlobName { get; set; }

        public string ContentType { get; set; }
    }
}
