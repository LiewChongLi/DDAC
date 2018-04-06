using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DDAC.Models
{
    public class ShipDetails
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Ship Name")]
        public string ShipName { get; set; }

        [Required]
        [Range(10, 1000)]
        [Display(Name = "Bay Size")]
        public int BaySize { get; set; }

        [Required]
        [Display(Name = "Origin")]
        public string Origin { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }    
        
        [Required]
        public bool Availability { get; set; }

        [Required]
        [Display(Name = "Remaining Bay Size")]
        public int RemainingBaySize { get; set; }

    }

    public class ScheduleDetails
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Origin")]
        public string Origin { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Required]
        public int ShipDetailsId { get; set; }

        public ShipDetails ShipDetails { get; set; }
    }
    
    public class Location
    {
        public int Id { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

    }




}