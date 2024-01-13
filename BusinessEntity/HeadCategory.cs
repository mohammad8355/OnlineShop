using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity;

public class HeadCategory
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string IdentifierName { get; set; }
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

     //navigation property
     public ICollection<Category> Categories { get; set; }

}
