using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppCalendar.Models
{
    public class EmpModel
    {
        [Display(Name = "Enter Date")]
        public DateTime EnterDate { get; set; }
    }
}