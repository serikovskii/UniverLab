using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverLab.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public int ProfessorId { get; set; }
        public int LectureHallId { get; set; }
        public DateTime Time { get; set; }
        public DayOfWeek Day { get; set; }
    }
}
