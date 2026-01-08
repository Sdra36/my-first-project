using System;

namespace SimpleStudentList
{
    enum Grade { Fail, Good, VeryGood, Excellent }

    class Student
    {
        public int id;
        public string studentname;
        public string thecity;
        public double exam1;
        public double exam2;
        public double average;
        public Grade grade;
    }

    class Node
    {
        public Student data;
        public Node next;
        public Node prev;
        public Node(Student s) { data = s; next = null; prev = null; }
    }

    class LinkedListt
    {
        public Node head;
        public Node tail;

        // دالة مشان تحديد التقدير تبع الطالب
        Grade GetGrade(double avg)
        {
            if (avg < 60) return Grade.Fail;
            else if (avg < 70) return Grade.Good;
            else if (avg < 85) return Grade.VeryGood;
            else return Grade.Excellent;
        }

        // مشان جهز ال بيانات تبع الطالب (المعدل والتقدير)
        private void PrepareStudent(Student s)
        {
            s.average = (s.exam1 + s.exam2) / 2;
            s.grade = GetGrade(s.average);
        }

        // 1. مشان الإضافة في نهاية القائمة
        public void AddLast(Student s)
        {
            PrepareStudent(s);
            Node newNode = new Node(s);
            if (head == null) { head = tail = newNode; }
            else
            {
                tail.next = newNode;
                newNode.prev = tail;
                tail = newNode;
            }
            Console.WriteLine("The item was added to the end of the list.");
        }

        // 2. مشان الإضافة في بداية القائمة
        public void AddFirst(Student s)
        {
            PrepareStudent(s);
            Node newNode = new Node(s);
            if (head == null) { head = tail = newNode; }
            else
            {
                newNode.next = head;
                head.prev = newNode;
                head = newNode;
            }
            Console.WriteLine("Added to the beginning of the list..");
        }

        // هي الدالة بستخدمها مشان الفرز (حسب اختيار المستخدم)
        public void Sort(int choice)
        {
            if (head == null || head.next == null) return;
            bool Switch;
            do
            {
                Switch = false;
                Node current = head;
                while (current.next != null)
                {
                    bool shouldSwitch = false;
                    if (choice == 1) // اسم A-Z
                        if (string.Compare(current.data.studentname, current.next.data.studentname) > 0) shouldSwitch = true;
                    if (choice == 2) // المعدل من الأدنى للأعلى 
                        if (current.data.average > current.next.data.average) shouldSwitch = true;

                    if (shouldSwitch)
                    {
                        Student temp = current.data;
                        current.data = current.next.data;
                        current.next.data = temp;
                        Switch = true;
                    }
                    current = current.next;
                }
            } while (Switch);
        }

        // امشان البحث العودي عن علامة محددة  
        public void SearchByAverage(Node current, double targetAvg, ref bool found)
        {
            if (current == null) return;
            if (current.data.average == targetAvg)
            {
                Console.WriteLine($"- We found the student.: {current.data.studentname} | The result: {current.data.average}");
                found = true;
            }
            SearchByAverage(current.next, targetAvg, ref found);
        }

        public void showy()
        {
            Node current = head;
            if (current == null) { Console.WriteLine("The list is empty.."); return; }
            Console.WriteLine("\n--- Current student list ---");
            while (current != null)
            {
                Console.WriteLine($"ID: {current.data.id} | nme: {current.data.studentname} | the result : {current.data.average} | The student's grade: {current.data.grade}");
                current = current.next;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            LinkedListt list = new LinkedListt();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Student Management System---");
                Console.WriteLine("1. add new student");
                Console.WriteLine("2. Sorting students and Show the menu");
                Console.WriteLine("3. اTo search for students with a specific GPA (retrospective search)");
                Console.WriteLine("4. exit");
                Console.Write("chose the operation number: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1":
                        try
                        {
                            Student s = new Student();
                            Console.Write("the id: ");
                            s.id = int.Parse(Console.ReadLine());

                            Console.Write("name: ");
                            s.studentname = Console.ReadLine();

                            // هون أضفنا التحقق من علامات الاختبار
                            Console.Write("Exam mark 1: ");
                            s.exam1 = Convert.ToDouble(Console.ReadLine());

                            Console.Write("Exam mark 2: ");
                            s.exam2 = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Where do you want to add it? (1: At the beginning, 2: At the end)");
                            int pos = int.Parse(Console.ReadLine());
                            if (pos == 1) list.AddFirst(s);
                            else list.AddLast(s);
                        }
                        catch (FormatException)
                        {
                            // هون هاد الجزء يتم تنفيذه في حال أدخل المستخدم حرفاً بدلاً من رقم
                            Console.WriteLine("\n!!! Error: You entered a character or symbol. This field requires a number.");
                        }
                        catch (Exception ex)
                        {
                            // لمشان اي أخطاء أخرى غير متوقعة
                            Console.WriteLine("\nAn error occurred: " + ex.Message);
                        }
                        break;

                    case "2":
                        // إهون كمان  لضمن ما يتعطل البرنامج عند اختيار طريقة الفرز
                        try
                        {
                            Console.WriteLine("Choose a sorting method (1: by name A-Z, 2: by total from lowest to highest))");
                            int sChoice = int.Parse(Console.ReadLine());
                            list.Sort(sChoice);
                            list.showy();
                        }
                        catch { Console.WriteLine("Invalid input for sorting choice."); }
                        break;

                    case "3":
                        try
                        {
                            Console.Write("Enter the result that you are looking for: ");
                            double target = Convert.ToDouble(Console.ReadLine());
                            bool found = false;
                            list.SearchByAverage(list.head, target, ref found);
                            if (!found) Console.WriteLine("There are no students with this score.");
                        }
                        catch { Console.WriteLine("Invalid input. Please enter a numerical GPA."); }
                        break;

                    case "4":
                        exit = true;
                        break;
                }
            }
        }
    }
}
