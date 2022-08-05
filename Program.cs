using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {
        // course class of objects
        class course
        {
            public string courseid;
            public string title;
            public string code;
            public string subject;
            public string location;
            public string instructor;
        }

        // instructor class of objects
        class instructor
        {
            public string name;
            public string email;
            public string phone;
          
        }
        static void Main(string[] args)
        {

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            String cpath =path+"/Courses.csv";
            String ipath =  path+"/Instructors.csv";
            var courses = new course[1];
            int lines = 0;
            List<instructor> instructorList = new List<instructor>();
            
            if (File.Exists(cpath))
            {  //count how many lines in the csv file
                using (var textReader = new StreamReader(cpath))
                {
                    while (textReader.ReadLine() != null)
                    {
                        lines++;
                    }
                    
                }



                // reading the file again
                    using (var textReader = new StreamReader(cpath))
                { //using System.IO  
                    char _Delimiter = ','; // .csv comma separate values 
                    string line;
                    int i = 0;
                    // skipping the first line
                    line = textReader.ReadLine();
                    Array.Resize(ref courses, lines - 1);
                    //looping througn the lines
                    while ((line = textReader.ReadLine()) != null)
                    {

                        //assigning column values to a an array of strings 
                        string[] columns = line.Split(_Delimiter);

                        // creating course objects related to individual lines and adding them to the course array of objects
                        courses[i] = new course { subject = columns[0].Substring(0, 3), code = columns[0].Substring(4), title = columns[1], courseid = columns[2], instructor = columns[3], location = columns[7] };
                        i++;


                    }
                    // first query
                    IEnumerable<course> query1 = from c in courses where c.subject == "IEE" && Int32.Parse(c.code) >= 300 orderby c.instructor select new course { code = c.code, title = c.title, instructor = c.instructor };
                 
                    // seconde query
                    var query2 = from c in courses group c by c.subject into subjectGroup select new { key = subjectGroup.Key, count = subjectGroup.Count(), codeGroupe = from c in subjectGroup group c by c.code into codeGroupe select codeGroupe };
                    Console.WriteLine("Question 1.2-a Reatrieving IEE courses with codes >=300 Please press any key \n ");
                    Console.ReadKey();
                    foreach (course c in query1)
                    {
                       
                        Console.WriteLine(c.code + "-----------------------------------" + c.title + "--------------------  " + c.instructor);
                    }

                    Console.WriteLine("\n Question 1.2-b Retrieve and deliver courses in groups \n ");
                    Console.ReadKey();
                    foreach (var c in query2)
                    {
                        Console.WriteLine(c.key);

                        foreach (var cg in c.codeGroupe)
                        {
                            if (cg.Count() >= 2)
                            {
                                Console.WriteLine("----" + cg.Key);
                                foreach (var crs in cg)
                                {

                                    Console.WriteLine("-----------" + crs.title);
                                }
                            }

                        }
                    }
                }




            }

            if (File.Exists(ipath))
            {    // same concept as the previous question

                using (var textReader = new StreamReader(ipath))
                { //using System.IO  
                    char _Delimiter = ','; // .csv comma separate values 
                    string line = textReader.ReadLine();
                    line = textReader.ReadLine();
                    while (line != null)
                    {

                        string[] columns = line.Split(_Delimiter);
                        line = textReader.ReadLine();
                        instructorList.Add(new instructor {name=columns[0], email=columns[1], phone=columns[2] });
                    
                    }
                    Console.WriteLine("-------------------------------------------------------"+"\n");

                   
                    
                }
            }
            // third query

            var query3 = from course in courses join inst in instructorList on course.instructor equals inst.name orderby course.code select new { subject = course.subject,code= course.code, email=inst.email };
            Console.WriteLine("Question 1.5 query to find the course  and instructor’s email address for each course.Please enter any key \n");
            Console.ReadKey();
            foreach (var courseAndEmail in query3)
            {
                if(Int32.Parse(courseAndEmail.code) >=200 && Int32.Parse(courseAndEmail.code) <= 300)
                Console.WriteLine(courseAndEmail.subject+"----"+courseAndEmail.code + "----" + courseAndEmail.email);
            }
        }


    }
}
