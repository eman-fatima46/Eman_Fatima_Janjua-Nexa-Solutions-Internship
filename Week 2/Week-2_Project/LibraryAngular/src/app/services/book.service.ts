import { Injectable } from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  Book
} from '../book';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  private apiUrl =
    'http://localhost:5120/api/Books';

  constructor(
    private http: HttpClient
  ) {
  }

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(
      this.apiUrl
    );
  }

  getBook(
    id: number
  ): Observable<Book> {

    return this.http.get<Book>(
      `${this.apiUrl}/${id}`
    );
  }

  addBook(
    book: Book
  ): Observable<Book> {

    return this.http.post<Book>(
      this.apiUrl,
      book
    );
  }

  updateBook(
    book: Book
  ): Observable<Book> {

    return this.http.put<Book>(
      `${this.apiUrl}/${book.id}`,
      book
    );
  }

  deleteBook(
    id: number
  ): Observable<unknown> {

    return this.http.delete(
      `${this.apiUrl}/${id}`
    );
  }
}