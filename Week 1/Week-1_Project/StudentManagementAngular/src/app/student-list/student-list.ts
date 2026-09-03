import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Student } from '../student';

@Component({
  selector: 'app-student-list',
  imports: [],
  templateUrl: './student-list.html',
  styleUrl: './student-list.css'
})
export class StudentList {

  @Input() students: Student[] = [];

  @Output() studentSelected = new EventEmitter<Student>();

  searchText: string = '';

  get filteredStudents(): Student[] {

    const search = this.searchText
      .toLowerCase()
      .trim();

    if (search === '') {
      return this.students;
    }

    return this.students.filter(student =>
      student.name.toLowerCase().includes(search) ||
      student.department.toLowerCase().includes(search)
    );
  }

  updateSearch(event: Event): void {

    const input = event.target as HTMLInputElement;

    this.searchText = input.value;
  }

  selectStudent(student: Student): void {
    this.studentSelected.emit(student);
  }
}