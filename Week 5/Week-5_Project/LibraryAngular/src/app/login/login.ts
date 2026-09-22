import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',

  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule
  ],

  templateUrl: './login.html',

  styleUrl: './login.css'
})
export class Login {

  isLoading = false;

  errorMessage = '';

  loginForm;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    this.loginForm =
      this.formBuilder.group({

        username: [
          '',
          Validators.required
        ],

        password: [
          '',
          Validators.required
        ]
      });
  }

  login(): void {

    if (
      this.loginForm.invalid
    ) {

      this.loginForm
        .markAllAsTouched();

      return;
    }

    const username =
      this.loginForm.value
        .username ?? '';

    const password =
      this.loginForm.value
        .password ?? '';

    this.isLoading = true;

    this.errorMessage = '';

    this.authService
      .login(
        username,
        password
      )
      .subscribe({

        next: () => {

          this.isLoading = false;

          this.router.navigate([
            '/books'
          ]);
        },

        error: () => {

          this.isLoading = false;

          this.errorMessage =
            'Invalid username or password.';
        }
      });
  }
}