using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DDAC.Models
{
    public class BookScheduleModel
    {
        public int Id { get; set; }

        public bool IsDelivered { get; set; }

        [Required]
        public int ScheduleDetailsId { get; set; }
        public ScheduleDetails ScheduleDetails { get; set; }

        public int totalBayUsed { get; set; }

        [Required]
        public int CustomerModelsId { get; set; }
        public CustomerModels CustomerModels { get; set; }

        public ICollection<ContainerModels> ContainerModels { get; set; }
    }
}