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

  isSaving = false;

  errorMessage = '';

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
        Validators.required
      ]
    ),

    author: new FormControl(
      '',
      [
        Validators.required
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

          this.errorMessage =
            'Could not load the book.';

        }

      });
  }

  onSubmit(): void {

    if (this.bookForm.invalid) {

      this.bookForm.markAllAsTouched();

      return;
    }

    this.isSaving = true;

    this.errorMessage = '';

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

            this.isSaving = false;

            this.router.navigate(
              ['/books']
            );
          },

          error: () => {

            this.isSaving = false;

            this.errorMessage =
              'Could not update the book.';
          }

        });

    }
    else {

      this.bookService
        .addBook(book)
        .subscribe({

          next: () => {

            this.isSaving = false;

            this.router.navigate(
              ['/books']
            );
          },

          error: () => {

            this.isSaving = false;

            this.errorMessage =
              'Could not add the book.';
          }

        });
    }
  }
}