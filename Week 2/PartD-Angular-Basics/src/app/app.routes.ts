import { Routes } from '@angular/router';
import { StudentList } from './student-list/student-list';
import { Registration } from './registration/registration';

export const routes: Routes = [

  {
    path: 'students',
    component: StudentList
  },

  {
    path: 'register',
    component: Registration
  },

  {
    path: '',
    redirectTo: 'students',
    pathMatch: 'full'
  }

];