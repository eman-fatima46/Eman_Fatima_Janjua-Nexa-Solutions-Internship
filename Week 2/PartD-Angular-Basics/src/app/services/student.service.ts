import { Injectable } from '@angular/core';
import { Student } from '../student';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private students: Student[] = [
    {
      id: 1,
      name: 'Ali',
      email: 'ali@example.com',
      department: 'Computer Science'
    },
    {
      id: 2,
      name: 'Sara',
      email: 'sara@example.com',
      department: 'Software Engineering'
    },
    {
      id: 3,
      name: 'Ahmed',
      email: 'ahmed@example.com',
      department: 'Artificial Intelligence'
    }
  ];

  getStudents(): Student[] {
    return this.students;
  }
}