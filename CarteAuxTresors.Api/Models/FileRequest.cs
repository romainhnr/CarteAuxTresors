using System.ComponentModel.DataAnnotations;

namespace CarteAuxTresors.Api.Models
{
    /// <summary>
    /// FileRequest
    /// </summary>
    public class FileRequest
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        [Required]
        public required IFormFile File { get; set; }
    }
}
