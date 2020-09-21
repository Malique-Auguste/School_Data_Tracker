using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolData;

namespace SchoolDataTracker
{
    class Program
    {
        static School school;
        static Subject Math = new Subject("Mathematics", 3, 1);
        static Subject Physics = new Subject("Physics", 3, 1);
        static Subject Business = new Subject("Business", 2, 1);
        static Subject English = new Subject("English", 2, 1);
        static Subject Art = new Subject("Art", 1, 1);
        static Subject Music = new Subject("Music", 1, 1);

        static void Initialise_School()
        {
            List<Teacher> teachers_ = new List<Teacher>(){ new Teacher(false, "Emily", "Smith", Math, 2, new List<int>(){1,2,3}), new Teacher(true, "Roger", "Ramoutar", Physics, 3, new List<int>(){1,2,4,5}), 
                                    new Teacher(false, "Claire", "Prince", Business, 2, new List<int>(){1,3,4,5}),new Teacher(false, "Samantha", "Ali", English, 1, new List<int>(){1,2,5,6}), 
                                    new Teacher(true, "Roger", "Brown", Art, 2, new List<int>(){1, 2, 3, 4}), new Teacher(true, "James", "Parris", Music, 3, new List<int>(){1,2,3,4})};
            List<Student> students_ = new List<Student>(){ new Student("Joshua", 1, new List<Subject>(){ Math, English, Music }), new Student("Ari", 1, new List<Subject>(){ Math, Physics, Art }),
                                    new Student("Katelyn", 1, new List<Subject>(){ English, English, Music }), new Student("Richard", 1, new List<Subject>(){ Physics, Math, Business }),
                                    new Student("Jabari", 1, new List<Subject>(){ Art, English, Music }), new Student("Zion", 1, new List<Subject>(){ Physics, Business, Music }),
                                    new Student("Anna", 1, new List<Subject>(){ Physics, English, Music }), new Student("Gregory", 1, new List<Subject>(){ English, Art, Business })};
            
            school = new School("1st Achiever's Secondary School", teachers_, students_);
            Console.WriteLine("School Initialisation Complete");
        }

        static Time_Table Initialise_Time_Table()
        {
            Time_Table time_table = new Time_Table(5, 5, 30, 30, new List<Subject>(){ Math, Physics, Business, English, Art, Music });
            for (int i = 0; i < time_table.subjects.Count; i++)
            {
                time_table.subjects[i].SetPopularity(school.Get_Subject_Popularity(time_table.subjects[i]));
            }
            Console.WriteLine("Time Table Initialisation Complete");
            return time_table;
        }

        static void Main(string[] args)
        {
            Initialise_School();
            Time_Table time_table = Initialise_Time_Table();
            time_table.Generate_Time_Table_Data(school.teachers.Where(x => x.years.Contains(1)).ToList());
            time_table.Print_Table();
            school.time_table.Add(time_table);
            Console.WriteLine();
        }
    }
}