using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;

namespace SchoolDataTracker
{
    class Program
    {
        static School school;
        static Subject Math = new Subject("Mathematics", 3);
        static Subject Physics = new Subject("Physics", 3);
        static Subject Chemistry = new Subject("Chemistry", 3);
        static Subject Business = new Subject("Business", 2);
        static Subject English = new Subject("English", 2);
        static Subject Art = new Subject("Art", 1);
        static Subject Music = new Subject("Music", 1);

        static void Initialise_School()
        {
            Teacher[] teachers_ = { new Teacher("Ms. Smith", Math), new Teacher("Mr. Ramoutar", Physics), new Teacher("Mrs. Prince", Business),
                                    new Teacher("Ms. Ali", English), new Teacher("Mr. Brown", Art), new Teacher("Mr. Parris", Music)};
            Student[] students_ = { new Student("Joshua", 1, new Subject[] { Math, English, Music }), new Student("Ari", 1, new Subject[] { Math, Physics, Art }),
                                    new Student("Katelyn", 1, new Subject[] { Chemistry, English, Music }), new Student("Richard", 1, new Subject[] { Physics, Math, Business }),
                                    new Student("Jabari", 1, new Subject[] { Art, English, Music }), new Student("Zion", 1, new Subject[] { Physics, Chemistry, Music }),
                                    new Student("Anna", 1, new Subject[] { Physics, English, Music }), new Student("Gregory", 1, new Subject[] { English, Art, Business })};
            school = new School("1st Achiever's Secondary School", teachers_, students_);
            Console.WriteLine("School Initialisation Complete");
        }

        static Time_Table Initialise_Time_Table()
        {
            Time_Table time_table = new Time_Table(5, 5, 30, 30, new Subject[] { Math, Physics, Chemistry, Business, English, Art, Music });
            for (int i = 0; i < time_table.subjects.Length; i++)
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
            time_table.Generate_Time_Table_Data();
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}