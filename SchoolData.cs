using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Data
{
    class School
    {
        public string name;
        public Teacher[] teachers;
        public Student[] students;
        public Time_Table time_table;

        public School(string name, Teacher[] teachers, Student[] students)
        {
            this.name = name;
            this.teachers = teachers;
            this.students = students;
        }

        public int Get_Subject_Popularity(Subject subject_)
        {
            int popularity = 0;
            for (int i = 0; i < students.Length; i++)
            {
                for (int j = 0; j < students[i].subjects.Length; j++)
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
            return String.Format("{0} has {1} teachers and {2} students.", name, teachers.Length, students.Length);
        }
    }

    class Teacher
    {
        public string name;
        public Subject subject;

        public Teacher(string name, Subject subject)
        {
            this.name = name;
            this.subject = subject;
        }

        public override string ToString()
        {
            return String.Format("{0} teaches {1}.", name, subject);
        }
    }

    class Student
    {
        public string name;
        public int form;
        public Subject[] subjects;

        public Student(string name, int form, Subject[] subjects)
        {
            this.name = name;
            this.form = form;
            this.subjects = subjects;
        }
        public override string ToString()
        {
            return String.Format("{0} studies {1} subjects.", name, subjects.Length);
        }
    }

    class Time_Table
    {
        public int number_of_days; //per week
        public float day_start;
        public float period_length; //in minutes
        public int number_of_periods; //per day
        public Subject[] subjects;
        public List<string> time_table_data = new List<string>();
        private string table_string = "";

        public Time_Table(int number_of_days, int number_of_periods, float day_start, float period_length, Subject[] subjects)
        { 
            if(subjects.Length > number_of_days * number_of_periods)
            {
                Console.WriteLine("Error Encountered: too much subjects for time table size.");
                Console.Read();
                System.Environment.Exit(1);
            }

            this.number_of_days = number_of_days;
            this.number_of_periods = number_of_periods;
            this.day_start = day_start;
            this.period_length = period_length;
            this.subjects = subjects;
        }
        
        public List<int> Generate_Number_Of_Subject_Periods()
        {
            subjects.OrderBy(x => x.importance);
           
            int total_num_of_periods = number_of_periods * number_of_days;
            int best_amount_of_periods = subjects.Sum(x => x.importance);
            List<int> periods = new List<int>();
            for (int i = 0; i < subjects.Length; i++)
            {
                int temp = (int)(subjects[i].importance * ((float)total_num_of_periods / best_amount_of_periods));
                if(temp == 0)
                {
                    temp = 1;
                }
                periods.Add(temp);
            }

            /*
            int j = 0;
            while(total_num_of_periods - periods.Sum() > 0 && j != periods.Count)
            {
                if(subjects[j].Get_Popularity() > Subject.average_popularity)
                {
                    periods[j]++;
                }
                j++;
            }
            */

            return periods;
        }

        public List<T> Randomise_List<T>(List<T> list_)
        {
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
                List<int> number_of_letters = new List<int>();
                Console.WriteLine();
                for (int j = 0; j < number_of_periods; j++)
                {
                    number_of_letters.Add(0);
                    for (int i = 0; i < number_of_days; i++)
                    {
                        if(time_table_data[(number_of_periods * i) + j].Length - 1> number_of_letters[j])
                        {
                            number_of_letters[j] = time_table_data[(number_of_periods * i) + j].Length - 1;
                        }
                    }
                }

                StringBuilder builder = new StringBuilder();
                int k = 0;
                for (int i = 0; i < number_of_days; i++)
                {
                    builder.Append(i + 1);
                    builder.Append(" ");
                    for (int j = 0; j < number_of_periods; j++)
                    {
                        int number_of_spaces = number_of_letters[j] - (time_table_data[k].Length - 1);
                        builder.Append("|");
                        builder.Append(time_table_data[k]);
                        builder.Append(new string(' ',number_of_spaces));
                        k++;
                    }
                    builder.Append("|");
                    builder.Append("\n\n");
                }
                table_string = builder.ToString();
            }

            Console.WriteLine(table_string);
        }
        
        public void Generate_Time_Table_Data()
        {
            List<int> periods = Generate_Number_Of_Subject_Periods();
            
            for (int i = 0; i < subjects.Length; i++)
			{
                for (int j = 0; j < periods[i]; j++)
			    {
                    time_table_data.Add(subjects[i].name + " ");
			    }
			}

            Subject Free_Period = new Subject("Free Period", 0);
            while(time_table_data.Count < number_of_days * number_of_periods)
            {
                time_table_data.Add(Free_Period.name);
            }
            
            time_table_data = Randomise_List(time_table_data);
        }
    }

    class Subject
    {
        public string name;
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

        private static List<string> subject_types = new List<string>();
        private static List<int> subject_popularities = new List<int>();
        public static float average_popularity = 1;

        public Subject(string name, int importance)
        {
            this.name = name;
            this.importance = importance;

            if(!subject_types.Contains(name))
            {
                subject_types.Add(name);
                subject_popularities.Add(0);
            }
        }

        public void SetPopularity(int num)
        {
            subject_popularities[subject_types.IndexOf(this.name)] += num;
            average_popularity = (float)subject_popularities.Sum() / subject_popularities.Count;
        }

        public int Get_Popularity()
        {
            return subject_popularities[subject_types.IndexOf(this.name)];
        }

        public override string ToString()
        {
            return name;
        }
    }
}
