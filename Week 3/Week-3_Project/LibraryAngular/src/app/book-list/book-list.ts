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

  isLoading = false;

  errorMessage = '';

  constructor(
    private bookService: BookService
  ) {
  }

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {

    this.isLoading = true;

    this.errorMessage = '';

    this.bookService
      .getBooks()
      .subscribe({

        next: (books) => {
          this.books = books;
          this.isLoading = false;
        },

        error: (error) => {
          console.error(error);

          this.errorMessage =
            'Could not load books. Please make sure the backend API is running.';

          this.isLoading = false;
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
          console.error(error);

          this.errorMessage =
            'Could not delete the book.';
        }

      });
  }
}