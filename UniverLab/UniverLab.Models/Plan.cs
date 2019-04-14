using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverLab.Models
{
    public class Plan
    {
        public string Id { get; set; }
        public int SubjectId { get; set; }
        public string ThemeSubject { get; set; }
        public int Hours { get; set; }
    }
}
