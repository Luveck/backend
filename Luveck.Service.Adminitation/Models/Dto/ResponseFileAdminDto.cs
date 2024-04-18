using System.Diagnostics.CodeAnalysis;
using System;

namespace Luveck.Service.Administration.Models.Dto
{
    /// <summary>
    /// Class ResponseFileAdminDto.
    /// </summary>
    /// <remarks>Erick Tijera</remarks>
    [ExcludeFromCodeCoverage]
    public class ResponseFileAdminDto
    {
        /// <summary>
        /// Gets or sets the IsSuccess.
        /// </summary>
        /// <value>The IsSuccess.</value>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the Error exception.
        /// </summary>
        /// <value>The Error exception.</value>
        public Exception Error { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        /// <value>The Message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        /// <value>The Data.</value>
        public BlobDto Data { get; set; }
    }
}
