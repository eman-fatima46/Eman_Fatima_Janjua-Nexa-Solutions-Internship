Person person = new Person("Ahmed", 40);

Console.WriteLine("=== PERSON ===");
person.Introduce();

Student student = new Student(
    "Ali",
    21,
    1001,
    "Computer Science"
);

Console.WriteLine("\n=== STUDENT ===");
student.Introduce();
student.Study();
student.Print();

Teacher teacher = new Teacher(
    "Sara",
    35,
    "Programming"
);

Console.WriteLine("\n=== TEACHER ===");
teacher.Introduce();
teacher.Teach();