import { Component, signal } from '@angular/core';
import { Student } from './student';

@Component({
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html'
})
export class App {

  protected readonly title = signal('PartD-Angular-Student');

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

  selectStudent(student: Student): void {
    this.selectedStudent = student;
  }
}