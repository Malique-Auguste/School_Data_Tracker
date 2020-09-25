using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using SchoolData;

namespace ExternalDataManager
{
    static class DataManager
    {
        public static void Save_Data<T>(string file_name, T obj)
        {
            string data = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText("saved_data/"+file_name+".txt", data);
        }

        public static T Load_Data<T>(string file_name)
        {
            string file = File.ReadAllText("saved_data/"+file_name+".txt");
            return JsonConvert.DeserializeObject<T>(file);
        }

        public static bool Add_Teacher(List<Teacher> teachers, List<Subject> subjects)
        {
            //returns true if the fucntion has to be recalled
            Console.WriteLine("Enter the teacher's personal information below('exit' can be typed at any time to return to the menu):");
            bool male;
            while(true)
            {
                Console.Write("Gender(m/f): ");
                string gender_str = Console.ReadLine();
                if(gender_str == "m" || gender_str =="f")
                {
                    if(gender_str == "m")
                    {
                        male = true;
                    }
                    else
                    {
                        male = false;
                    }
                    break;
                }
                else if(gender_str.Contains("exit"))
                {
                    return false;
                }
                Console.WriteLine("Gender was not in correct format.");
            }

            Console.Write("First Name: ");
            string f_name = Console.ReadLine();
            if(f_name.Contains("exit"))
            {
                return false;
            }
            Console.Write("Last Name: ");
            string l_name = Console.ReadLine();
            if(l_name.Contains("exit"))
            {
                return false;
            }

            Subject subject;
            while(true)
            {
                Console.Write("Subject Name: ");
                string subject_str = Console.ReadLine();
                if(subject_str.Contains("exit"))
                {
                    return false;
                }
                subject = subjects.FirstOrDefault(x => x.name == subject_str);
                if(subject != null)
                {
                    break;
                }
                Console.WriteLine("The subject could not be found.");
            }

            int experience;
            while(true)
            {
                Console.Write("Experience(1-3): ");
                string experience_str = Console.ReadLine();
                if(int.TryParse(experience_str, out experience))
                {
                    if(experience > 3 || experience < 1)
                    {
                        Console.WriteLine("Experience was out of range.");
                    }
                    else
                    {
                        break;
                    }
                }
                else if(experience_str.Contains("exit"))
                {
                    return false;
                }
            }
        
            List<int> years;
            while(true)
            {
                Console.Write("Years(comma separated list): ");
                string years_str = Console.ReadLine();
                if(years_str.Contains("exit"))
                {
                    return false;
                }
                List<string> years_str_list = years_str.Split(",").ToList();
                try
                {
                    years = years_str_list.Select(x => int.Parse(x)).ToList();
                    break;
                }
                catch
                {
                    Console.WriteLine("Years was not in correct format.");
                }
            }
        
            Teacher new_teacher = new Teacher(male, f_name, l_name, subject, experience, years);
            while(true)
            {
                Console.Write("Are you sure that you would like to save the teacher to the data base(y/n): ");
                string decision = Console.ReadLine();
                if(decision == "y")
                {
                    break;
                }
                else if(decision == "n")
                {
                    Console.WriteLine("Redo entry.");
                    return true;
                }
                else if(decision.Contains("exit"))
                {
                    return false;
                }
            }
            Console.WriteLine(new_teacher.Get_Short_Name());
            teachers.Add(new_teacher);
            Console.WriteLine(new_teacher.Get_Short_Name());
            Save_Data("Teachers", teachers);
            Console.WriteLine(new_teacher.Get_Short_Name()+" has been added to the directory.");
            return false;
        }
        
    }
}