import {
  Component,
  OnInit
} from '@angular/core';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import {
  BookService
} from '../services/book.service';

import {
  Book
} from '../book';

@Component({
  selector: 'app-book-form',
  imports: [ReactiveFormsModule],
  templateUrl: './book-form.html',
  styleUrl: './book-form.css'
})
export class BookForm
  implements OnInit {

  isEditMode = false;

  bookId: number | null = null;

  bookForm = new FormGroup({

    id: new FormControl<number | null>(
      null,
      [
        Validators.required,
        Validators.min(1)
      ]
    ),

    title: new FormControl(
      '',
      [
        Validators.required,
        Validators.minLength(2)
      ]
    ),

    author: new FormControl(
      '',
      [
        Validators.required,
        Validators.minLength(2)
      ]
    ),

    category: new FormControl(
      '',
      [
        Validators.required
      ]
    )

  });

  constructor(
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  ngOnInit(): void {

    const id =
      this.route.snapshot.paramMap.get('id');

    if (id !== null) {

      this.bookId = Number(id);

      this.isEditMode = true;

      this.loadBook(
        this.bookId
      );
    }
  }

  loadBook(id: number): void {

    this.bookService
      .getBook(id)
      .subscribe({

        next: (book) => {

          this.bookForm.setValue({
            id: book.id,
            title: book.title,
            author: book.author,
            category: book.category
          });

          this.bookForm.controls.id.disable();
        },

        error: () => {
          alert(
            'Book was not found.'
          );

          this.router.navigate(
            ['/books']
          );
        }

      });
  }

  onSubmit(): void {

    if (this.bookForm.invalid) {

      this.bookForm.markAllAsTouched();

      return;
    }

    const rawValue =
      this.bookForm.getRawValue();

    const book: Book = {
      id: Number(rawValue.id),
      title: rawValue.title ?? '',
      author: rawValue.author ?? '',
      category: rawValue.category ?? ''
    };

    if (this.isEditMode) {

      this.bookService
        .updateBook(book)
        .subscribe({

          next: () => {

            alert(
              'Book updated successfully.'
            );

            this.router.navigate(
              ['/books']
            );
          },

          error: () => {
            alert(
              'Book could not be updated.'
            );
          }

        });

    }
    else {

      this.bookService
        .addBook(book)
        .subscribe({

          next: () => {

            alert(
              'Book added successfully.'
            );

            this.router.navigate(
              ['/books']
            );
          },

          error: () => {
            alert(
              'Book could not be added.'
            );
          }

        });
    }
  }
}