import {
  Component,
  OnInit
} from '@angular/core';

import {
  RouterLink
} from '@angular/router';

import {
  Book
} from '../book';

import {
  BookService
} from '../services/book.service';

@Component({
  selector: 'app-book-list',
  imports: [RouterLink],
  templateUrl: './book-list.html',
  styleUrl: './book-list.css'
})
export class BookList
  implements OnInit {

  books: Book[] = [];

  constructor(
    private bookService: BookService
  ) {
  }

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {

    this.bookService
      .getBooks()
      .subscribe({

        next: (books) => {
          this.books = books;
        },

        error: (error) => {
          console.error(
            'Could not load books.',
            error
          );
        }

      });
  }

  deleteBook(id: number): void {

    const confirmed =
      confirm(
        'Are you sure you want to delete this book?'
      );

    if (!confirmed) {
      return;
    }

    this.bookService
      .deleteBook(id)
      .subscribe({

        next: () => {
          this.loadBooks();
        },

        error: (error) => {
          console.error(
            'Could not delete book.',
            error
          );
        }

      });
  }
}