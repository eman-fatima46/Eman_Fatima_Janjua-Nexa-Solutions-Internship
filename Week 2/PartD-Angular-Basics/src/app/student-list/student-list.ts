import { Component } from '@angular/core';
import { Student } from '../student';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-student-list',
  imports: [],
  templateUrl: './student-list.html',
  styleUrl: './student-list.css'
})
export class StudentList {

  students: Student[] = [];

  constructor(
    private studentService: StudentService
  ) {
    this.students = this.studentService.getStudents();
  }
}