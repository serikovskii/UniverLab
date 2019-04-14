using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniverLab.DataAccess;
using UniverLab.Models;

namespace UniverLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var student = new StudentTable();
            student.Add(new Student
            {
                FullName = "Azat Mukushev", GroupId = 1
            });

            var professor = new ProfessorTable();
            professor.Add(new Professor
            {
                FullName = "Oleg Skidan"
            });
        }
    }
}
