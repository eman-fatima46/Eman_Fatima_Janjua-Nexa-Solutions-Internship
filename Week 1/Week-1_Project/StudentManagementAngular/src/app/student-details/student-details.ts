import { Component, Input } from '@angular/core';
import { Student } from '../student';

@Component({
  selector: 'app-student-details',
  imports: [],
  templateUrl: './student-details.html',
  styleUrl: './student-details.css'
})
export class StudentDetails {

  @Input() student: Student | null = null;

}