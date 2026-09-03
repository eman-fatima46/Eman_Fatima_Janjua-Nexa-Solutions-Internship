import { Component } from '@angular/core';
import { Student } from './student';
import { StudentList } from './student-list/student-list';
import { StudentDetails } from './student-details/student-details';

@Component({
  selector: 'app-root',
  imports: [StudentList, StudentDetails],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  students: Student[] = [
    {
      id: 1001,
      name: 'Ali',
      department: 'Computer Science',
      marks: 85
    },
    {
      id: 1002,
      name: 'Sara',
      department: 'Software Engineering',
      marks: 92
    },
    {
      id: 1003,
      name: 'Ahmed',
      department: 'Computer Science',
      marks: 75
    },
    {
      id: 1004,
      name: 'Fatima',
      department: 'Artificial Intelligence',
      marks: 90
    },
    {
      id: 1005,
      name: 'Usman',
      department: 'Software Engineering',
      marks: 68
    }
  ];

  selectedStudent: Student | null = null;

  onStudentSelected(student: Student): void {
    this.selectedStudent = student;
  }
}