import { Routes } from '@angular/router';
import { Login } from './login/login';
import { BookList } from './book-list/book-list';
import { BookForm } from './book-form/book-form';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [

  {
    path: '',
    redirectTo: 'books',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: Login
  },

  {
    path: 'books',
    component: BookList
  },

  {
    path: 'books/add',
    component: BookForm,
    canActivate: [
      authGuard
    ]
  },

  {
    path: 'books/edit/:id',
    component: BookForm,
    canActivate: [
      authGuard
    ]
  },

  {
    path: '**',
    redirectTo: 'books'
  }

];