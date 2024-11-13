using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.RepositoryMS.Api.Models.PostgreSQL
{
    public class PipelineExecution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid RepositoryId { get; set; }

        public Guid PipelineId { get; set; }

        public string Status { get; set; }

    }
}
