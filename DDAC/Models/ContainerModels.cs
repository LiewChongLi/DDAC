using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DDAC.Models
{
    public class ContainerModels
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Container Type")]
        public string ContainerType { get; set; }

        [Required]
        [Range(10, 1000)]
        [Display(Name = "Number of Ship Bay Used")]
        public int NumberOfBayUsed { get; set; }


        
    }
}