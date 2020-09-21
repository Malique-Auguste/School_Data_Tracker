using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SchoolData
{
    class School
    {
        public string name; //name of the school
        public List<Teacher> teachers; //teachers at the school
        public List<Student> students; //students at the school
        public List<Time_Table> time_table = new List<Time_Table>(); //time table for each year arranged in asceding order (year 1 first)

        public School(string name, List<Teacher> teachers, List<Student> students)
        {
            //Constructor
            this.name = name;
            this.teachers = teachers;
            this.students = students;
        }

        public int Get_Subject_Popularity(Subject subject_)
        {
            //Returns the number of students taking a subject
            int popularity = 0;
            for (int i = 0; i < students.Count; i++)
            {
                for (int j = 0; j < students[i].subjects.Count; j++)
                {
                    if (students[i].subjects[j] == subject_)
                    {
                        popularity++;
                    }
                }
            }

            return popularity;
        }

        public override string ToString()
        {
            return String.Format("{0} has {1} teachers and {2} students.", name, teachers.Count, students.Count);
        }
    }

    class Teacher
    {
        public bool male; //false=female
        public string fname; //first name 
        public string lname; //last name
        public Subject subject; //subject that the teacher teaches
        public int experience; //1-3, 3 being a very experienced teacher
        public List<int> years; //years that the teacher teaches
        public int identification_number=-1;

        public Teacher(bool male, string fname, string lname, Subject subject, int experience, List<int> years)
        {
            //Constructor
            this.male = male;
            this.fname = fname;
            this.lname = lname;
            this.subject = subject;
            this.experience = experience;
            this.years = years;
        }

        public string Get_Short_Name()
        {
            string name = "Mr.";
            if(!male)
            {
                name = "Ms.";
            }
            return name + " " + fname[0] + "." + lname[0];
        }

        public override string ToString()
        {
            string name = "Mr.";
            if(!male)
            {
                name = "Ms.";
            }
            return name + " " + fname + " " + lname;
        }
    }

    class Student
    {
        public string name; //name of the student
        public int year; //year that the student is in
        public List<Subject> subjects; //subjects that the student studies
        public int identification_number=-1;

        public Student(string name, int year, List<Subject> subjects)
        {
            //Constructor
            this.name = name;
            this.year = year;
            this.subjects = subjects;
        }
        
        public override string ToString()
        {
            return name;
        }
    }

    class Time_Table
    {
        public int number_of_days; //days that the table spans over
        public float day_start; //time that the school day starts at
        public float period_length; //in minutes
        public int number_of_periods; //per day
        public List<Teacher> teachers;
        public List<Subject> subjects; //subjects that are in the time table
        public List<Subject> time_table_data = new List<Subject>(); //the order that the subjects are in the time table
        private string table_string = ""; //the time table in  string yearat

        public Time_Table(int number_of_days, int number_of_periods, float day_start, float period_length, List<Subject> subjects)
        { 
            //Constructor
            if(subjects.Count > number_of_days * number_of_periods)
            {
                //if there are not enough periods for there to be a class fo reach subject atleast once
                //an error is thrown and the softwares crashes
                throw new SystemException("too much subjects for time table size.");
            }

            this.number_of_days = number_of_days;
            this.number_of_periods = number_of_periods;
            this.day_start = day_start;
            this.period_length = period_length;
            this.subjects = subjects;
        }
        
        public List<int> Generate_Number_Of_Subject_Periods()
        {
            //this generates the number of periods that a subject will have in the time table
            subjects.OrderBy(x => x.importance);
           
            int total_num_of_periods = number_of_periods * number_of_days;
            int best_amount_of_periods = subjects.Sum(x => x.importance);
            List<int> periods = new List<int>();
            for (int i = 0; i < subjects.Count; i++)
            {
                //The importance of a subject is multiplied by the ratio of available periods to the ideal amount of periods
                //There is a minimum of 1
                int temp = (int)(subjects[i].importance * ((float)total_num_of_periods / best_amount_of_periods));
                if(temp == 0)
                {
                    temp = 1;
                }
                periods.Add(temp);
            }

            return periods;
        }

        public List<T> Randomise_List<T>(List<T> list_)
        {
            //Randomly orders list
            Random rnd = new Random();
            for (int i = 0; i < list_.Count; i++)
			{
                int rand_int = rnd.Next(0, list_.Count);
                T temp = list_[rand_int];
                list_[rand_int] = list_[i];
                list_[i] = temp;
			}

            return list_;
        }
        
        public void Print_Table()
        {
            if(table_string == "")
            {
                //Determines the number of letters in each column s that all the coloumns are spaced out well
                List<int> number_of_letters = new List<int>();
                Console.WriteLine();
                for (int j = 0; j < number_of_periods; j++)
                {
                    number_of_letters.Add(0);
                    for (int i = 0; i < number_of_days; i++)
                    {
                        int index = (number_of_periods * i) + j;
                        if(time_table_data[index].ToString().Length - 1> number_of_letters[j])
                        {
                            number_of_letters[j] = time_table_data[index].ToString().Length - 1;
                        }
                    }
                }

                //creates string for table
                StringBuilder builder = new StringBuilder();
                int k = 0;
                for (int i = 0; i < number_of_days; i++)
                {
                    if(i > 0)
                    {
                        builder.Append("\n\n");
                    }
                    builder.Append(i + 1);
                    for (int j = 0; j < number_of_periods; j++)
                    {
                        int number_of_spaces = number_of_letters[j] - (time_table_data[k].ToString().Length - 1);
                        builder.Append(" | ");
                        builder.Append(time_table_data[k]);
                        builder.Append(new string(' ',number_of_spaces));
                        k++;
                    }
                    builder.Append(" |");
                }
                table_string = builder.ToString();
            }
            
            Console.WriteLine(table_string);
        }
        
        public void Generate_Time_Table_Data(List<Teacher> teachers)
        {
            //generate the time table
            List<int> periods = Generate_Number_Of_Subject_Periods();

            for (int i = 0; i < subjects.Count; i++)
            {
                List<Teacher> possible_teachers = teachers.Where(x => x.subject.name == subjects[i].name).ToList();
                subjects[i].teacher = possible_teachers.OrderBy(x => x.experience).ToList()[0];
            }
            
            for (int i = 0; i < subjects.Count; i++)
			{
                for (int j = 0; j < periods[i]; j++)
			    {
                    time_table_data.Add(subjects[i]);
			    }
			}

            Subject Free_Period = new Subject("Free Period", 0, 0);
            while(time_table_data.Count < number_of_days * number_of_periods)
            {
                time_table_data.Add(Free_Period);
            }
            
            time_table_data = Randomise_List(time_table_data);
        }
    }

    class Subject
    {
        public string name; //name of subject
        private int importance_; //on a scale of 1-3, 3 is the most important
        public int importance
        {
            get { return importance_; }
            set
            {
                if (value < 1) { importance_ = 1; }
                else if (value > 3) { importance_ = 3; }
                else { importance_ = value; }
            }
        }
        private int year;
        public Teacher teacher;

        private static List<string> subject_types = new List<string>(); //static list conaining all of the subject names
        private static List<int> subject_popularities = new List<int>(); //static list containing the subjects importance

        public Subject(string name, int importance, int year)
        {
            //Costructor
            this.name = name;
            this.importance = importance;
            this.year = year;

            //If the subject is not already present in the subject types list it is added to the list
            if(!subject_types.Contains(name))
            {
                subject_types.Add(name + year);
                subject_popularities.Add(0);
            }
        }

        public void SetPopularity(int num)
        {
            //sets the popularity for a certain subject from the static lists
            subject_popularities[subject_types.IndexOf(this.name + year)] += num;
        }
        public int Get_Popularity()
        {
            //returns the popularity for a certain subject from the static lists
            return subject_popularities[subject_types.IndexOf(this.name + year)];
        }

        
        public override string ToString()
        {
            if(name == "Free Period")
            {
                return name;
            }
            return name + " (" + teacher.Get_Short_Name() + ")";
        }

        public override bool Equals(object obj)
        {
            return obj is Subject subject &&
                   name == subject.name &&
                   importance == subject.importance &&
                   year == subject.year;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(name, importance_, importance, year);
        }
    }
}
