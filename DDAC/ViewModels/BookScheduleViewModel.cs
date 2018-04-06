using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDAC.Models;

namespace DDAC.ViewModels
{
    public class BookScheduleViewModel
    {
        public BookScheduleModel BookScheduleModels { get; set; }
        public ScheduleDetails ScheduleDetails { get; set; }
        public IEnumerable<CustomerModels> CustomerModels { get; set; }
        public ContainerModels Container { get; set; }

    }
}