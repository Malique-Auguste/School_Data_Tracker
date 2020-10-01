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

            string subject;
            while(true)
            {
                Console.Write("Subject Name: ");
                subject = Console.ReadLine();
                if(subject.Contains("exit"))
                {
                    return false;
                }
                Subject subject_ = subjects.FirstOrDefault(x => x.name == subject);
                if(subject_ != null)
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
                else
                {
                    Console.WriteLine("Experience was not in correct format.");
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
                catch{}
                Console.WriteLine("Years was not in correct format.");
                
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
                    Console.WriteLine("\nRedo entry.");
                    return true;
                }
                else if(decision.Contains("exit"))
                {
                    return false;
                }
                Console.WriteLine("The entered value is not y or n");
            }
            
            teachers.Add(new_teacher);
            Save_Data("Teachers", teachers);
            Console.WriteLine(new_teacher.Get_Short_Name()+" has been added to the directory.");
            return false;
        }
        
        public static bool Remove_Teacher(List<Teacher> teachers)
        {
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
            
            List<Teacher> temp_teach_list = teachers.Where(x => x.male == male && x.fname == f_name && x.lname == l_name).ToList();
            if(temp_teach_list.Count == 0)
            {
                if(male)
                {
                    Console.Write("A male teacher by the name of "+f_name+" "+l_name+" could not be found.");
                }
                else
                {
                    Console.Write("A female teacher by the name of "+f_name+" "+l_name+" could not be found.");
                }

                while(true)
                {
                    Console.Write("Would you like to re-enter the data(y/n): ");
                    string decision = Console.ReadLine();
                    if(decision == "y")
                    {
                        return true;
                    }
                    else if(decision == "n")
                    {
                        Console.WriteLine("\nRedo entry.");
                        return false;
                    }
                    else if(decision.Contains("exit"))
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("The entered value is not y or n");
                    }
                }
            }

            teachers.RemoveAll(x => x.male == male && x.fname == f_name && x.lname == l_name);
            Save_Data("Teachers", teachers);

            Console.WriteLine("Deletion complete");
            return false;
        }
    
        public static bool Add_Student(List<Student> students, List<Subject> all_subjects)
        {
            //returns true if the fucntion has to be recalled
            Console.WriteLine("Enter the student's personal information below('exit' can be typed at any time to return to the menu):");
            
            Console.Write("Student's Name: ");
            string name = Console.ReadLine();
            if(name.Contains("exit"))
            {
                return false;
            }

            int year;
            while(true)
            {
                Console.Write("Year: ");
                string year_str = Console.ReadLine();
                if(int.TryParse(year_str, out year))
                {
                    break;
                }
                else if(year_str.Contains("exit"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Year was not in correct format.");
                }
            }
        
            List<Subject> subjects;
            while(true)
            {
                Console.Write("Subjects(comma separated list): ");
                string subjects_str = Console.ReadLine();
                if(subjects_str.Contains("exit"))
                {
                    return false;
                }
                List<string> subjects_str_list = subjects_str.Split(",").ToList();
                try
                {
                    subjects = all_subjects.Where(x => subjects_str_list.Contains(x.name)).ToList();
                    if(subjects.Count == subjects_str_list.Count)
                    {
                        break;
                    }
                    else
                    {
                        List<string> all_subjects_strings = all_subjects.Select(x => x.name).ToList();
                        string subjects_not_found = string.Join(", ",subjects_str_list.Where(x => !all_subjects_strings.Contains(x)).ToList());
                        Console.WriteLine("The subject(s) " + subjects_not_found + " could not be found");
                    }
                }
                catch
                {}
                Console.WriteLine("Subjects was not in correct format.");
                
            }
        
            Student new_student = new Student(name, year, subjects);
            while(true)
            {
                Console.Write("Are you sure that you would like to save the student to the data base(y/n): ");
                string decision = Console.ReadLine();
                if(decision == "y")
                {
                    break;
                }
                else if(decision == "n")
                {
                    Console.WriteLine("\nRedo entry.");
                    return true;
                }
                else if(decision.Contains("exit"))
                {
                    return false;
                }
                Console.WriteLine("The entered value is not y or n");
            }
            
            students.Add(new_student);
            Save_Data("Students", students);
            Console.WriteLine(new_student+" has been added to the database.");
            return false;
        }
        
        public static bool Remove_Student(List<Student> students, List<Subject> all_subjects)
        {
            //returns true if the fucntion has to be recalled
            Console.WriteLine("Enter the student's personal information below('exit' can be typed at any time to return to the menu):");
            
            Console.Write("Student's Name: ");
            string name = Console.ReadLine();
            if(name.Contains("exit"))
            {
                return false;
            }

            int year;
            while(true)
            {
                Console.Write("Year: ");
                string year_str = Console.ReadLine();
                if(int.TryParse(year_str, out year))
                {
                    break;
                }
                else if(year_str.Contains("exit"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Year was not in correct format.");
                }
            }
        
            while(true)
            {
                if(students.Where(x => x.name == name && x.year == year ).ToList().Count == 0)
                {
                    Console.WriteLine("That student could not be found in the database.");
                    while(true)
                    {
                        Console.Write("Would you like to re-enter the data(y/n): ");
                        string decision_ = Console.ReadLine();
                        if(decision_ == "y")
                        {
                            return true;
                        }
                        else if(decision_ == "n")
                        {
                            Console.WriteLine("\nRedo entry.");
                            return false;
                        }
                        else if(decision_.Contains("exit"))
                        {
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("The entered value is not y or n");
                        }
                    }
                }

                Console.Write("Are you sure that you would like to remove the student from the data base(y/n): ");
                string decision = Console.ReadLine();
                if(decision == "y")
                {
                    break;
                }
                else if(decision == "n")
                {
                    Console.WriteLine("\nRedo entry.");
                    return true;
                }
                else if(decision.Contains("exit"))
                {
                    return false;
                }
                Console.WriteLine("The entered value is not y or n");
            }
            
            students.RemoveAll(x => x.name == name && x.year == year );
            Save_Data("Students", students);
            Console.WriteLine(name +" has been removed from the database.");
            return false;
        }
        
    }
}