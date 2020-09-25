﻿using System;
using System.Collections.Generic;
using System.Linq;
using ExternalDataManager;
using System.Text;
using SchoolData;

namespace SchoolDataTracker
{
    class Program
    {
        static School school;
        static List<Subject> subjects;
        static List<Teacher> teachers;

        /*
        static Subject Math = new Subject("Mathematics", 3, 1);
        static Subject Physics = new Subject("Physics", 3, 1);
        static Subject Biology = new Subject("Biology", 3, 1);
        static Subject Chemistry = new Subject("Chemistry", 3, 1);
        static Subject Business = new Subject("Business", 2, 1);
        static Subject Accounts = new Subject("Accounts", 2, 1);
        static Subject IT = new Subject("I.T", 2, 1);
        static Subject Geography = new Subject("Geography", 2, 1);
        static Subject English = new Subject("English", 2, 1);
        static Subject Art = new Subject("Art", 1, 1);
        static Subject Music = new Subject("Music", 1, 1);
        static Subject Drama = new Subject("Drama", 1, 1);
        */

        static void Initialise_School()
        {
            /*
            List<Teacher> teachers_ = new List<Teacher>(){ new Teacher(false, "Emily", "Smith", Math, 2, new List<int>(){1,2,3}), new Teacher(true, "Roger", "Ramoutar", Physics, 3, new List<int>(){1,2,4,5}), 
                                    new Teacher(false, "Claire", "Prince", Business, 2, new List<int>(){1,3,4,5}),new Teacher(false, "Samantha", "Ali", English, 1, new List<int>(){1,2,5,6}), 
                                    new Teacher(true, "Roger", "Brown", Art, 2, new List<int>(){1, 2, 3, 4}), new Teacher(true, "James", "Parris", Music, 3, new List<int>(){1,2,3,4}),
                                    new Teacher(false, "Kim", "Parris", Drama, 1, new List<int>(){1, 2, 3, 4}), new Teacher(true, "Jonathan", "Ali", IT, 3, new List<int>(){1,2,3,4}),
                                    new Teacher(true, "Allan", "Nimble", Biology, 3, new List<int>(){1, 2, 3, 4}), new Teacher(true, "Ryker", "Dennis", Chemistry, 2, new List<int>(){1,2,3,4}),
                                    new Teacher(false, "Jenny", "Walay", Accounts, 1, new List<int>(){1, 2, 3, 4}), new Teacher(false, "Aasia", "Vanderpool", Geography, 1, new List<int>(){1, 2, 3, 4})};
            List<Student> students_ = new List<Student>(){ new Student("Joshua", 1, new List<Subject>(){ Math, Business, Accounts, English, Music, Art }), 
                                    new Student("Ari", 1, new List<Subject>(){ Math, Physics, Chemistry, English, Art, IT}),
                                    new Student("Katelyn", 1, new List<Subject>(){ English, Music, Art, Drama, Math, Biology}), 
                                    new Student("Richard", 1, new List<Subject>(){ Physics, Math, Geography,Business, English, }),
                                    new Student("Jabari", 1, new List<Subject>(){ Art, English, Music, Math, IT, Accounts}), 
                                    new Student("Zion", 1, new List<Subject>(){ Physics, Math, Drama, Business, Music }),
                                    new Student("Anna", 1, new List<Subject>(){ Accounts, IT, Math,English, Business, Geography}), 
                                    new Student("Gregory", 1, new List<Subject>(){ English, Art, Drama, IT, Math, Business }),
                                    new Student("Nicholas", 1, new List<Subject>(){ Math, Geography, Biology, Chemistry, Physics, English }),
                                    new Student("Lindsay", 1, new List<Subject>(){ Biology, Chemistry, Math, IT, Art, Accounts})};
            */
            teachers = DataManager.Load_Data<List<Teacher>>("Teachers");
            school = new School("1st Achiever's Secondary School", teachers, DataManager.Load_Data<List<Student>>("Students"));
            Console.WriteLine("School Initialisation Complete");
        }

        static Time_Table Initialise_Time_Table()
        {
            subjects = DataManager.Load_Data<List<Subject>>("Subjects");
            Time_Table time_table = new Time_Table(5, 5, 30, 30, subjects);
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
            school.time_table.Add(Initialise_Time_Table());
            school.time_table[0].Generate_Time_Table_Data(school.teachers.Where(x => x.years.Contains(1)).ToList());
            
            while(true)
            {
                Console.Write("\nWhat would you like to do?\n1.Add Teacher\n2.Add Student\n3.View Time Table\nOption(Type 'exit' at any time to quit): ");
                string option_str = Console.ReadLine();
                int option;
                if(int.TryParse(option_str, out option))
                {
                    Console.WriteLine();
                    if(option == 1)
                    {
                        //if the return of the function is true, recreate the teacher.
                        while(DataManager.Add_Teacher(teachers, subjects) == true)
                        {

                        }
                    }
                }
                else if(option_str.Contains("exit"))
                {
                    return;
                }
                else
                {
                    Console.WriteLine("The option selected was not a number.");
                }
            }

            /*
            while (true)
            {
                Console.WriteLine("\nEnter the student whose time table you would like to view");
                int i = int.Parse(Console.ReadLine());
                Console.WriteLine("\n"+school.students[i]+" time table:");
                Console.WriteLine(school.time_table[0].Generate_Table_String(school.students[i]));
                Console.WriteLine();
            }
            */
        }
    }
}