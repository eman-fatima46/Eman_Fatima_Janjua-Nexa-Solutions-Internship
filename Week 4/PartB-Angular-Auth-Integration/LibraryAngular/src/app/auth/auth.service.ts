import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { environment } from '../../environments/environment';


interface LoginRequest {
  username: string;
  password: string;
}


interface LoginResponse {
  token: string;
}


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly tokenKey = 'library_jwt_token';


  constructor(private http: HttpClient) {}


  login(
    username: string,
    password: string
  ): Observable<LoginResponse> {

    const request: LoginRequest = {
      username,
      password
    };

    return this.http
      .post<LoginResponse>(
        `${environment.apiUrl}/api/Auth/login`,
        request
      )
      .pipe(
        tap(response => {
          localStorage.setItem(
            this.tokenKey,
            response.token
          );
        })
      );
  }


  logout(): void {
    localStorage.removeItem(
      this.tokenKey
    );
  }


  getToken(): string | null {
    return localStorage.getItem(
      this.tokenKey
    );
  }


  isLoggedIn(): boolean {

    const token = this.getToken();

    if (!token) {
      return false;
    }

    const payload = this.decodeToken();

    if (!payload) {
      return false;
    }


    if (payload.exp) {

      const expiry =
        payload.exp * 1000;

      if (Date.now() >= expiry) {

        this.logout();

        return false;
      }
    }

    return true;
  }


  getRole(): string | null {

    const payload = this.decodeToken();

    if (!payload) {
      return null;
    }


    return (
      payload.role
      ??
      payload[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ]
      ??
      null
    );
  }


  getUsername(): string | null {

    const payload = this.decodeToken();

    if (!payload) {
      return null;
    }


    return (
      payload.unique_name
      ??
      payload.name
      ??
      payload[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
      ]
      ??
      null
    );
  }


  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }


  private decodeToken(): any | null {

    const token = this.getToken();

    if (!token) {
      return null;
    }


    try {

      const payloadPart =
        token.split('.')[1];

      if (!payloadPart) {
        return null;
      }


      let base64 =
        payloadPart
          .replace(/-/g, '+')
          .replace(/_/g, '/');


      while (
        base64.length % 4 !== 0
      ) {
        base64 += '=';
      }


      const decodedPayload =
        atob(base64);


      return JSON.parse(
        decodedPayload
      );

    }
    catch {

      return null;
    }
  }
}