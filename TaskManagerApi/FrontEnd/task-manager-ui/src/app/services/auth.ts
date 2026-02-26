import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private apiUrl = 'https://localhost:7021/api/Auth'; 

  // BehaviorSubject keeps last value
  private loggedIn$ = new BehaviorSubject<boolean>(this.isLoggedIn());
  constructor(private http: HttpClient) { }

  get isLoggedIn$() {
    return this.loggedIn$.asObservable();
  }


  login(username: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/login`, {
      username: username,
      password: password
    }).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        this.loggedIn$.next(true);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.loggedIn$.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
