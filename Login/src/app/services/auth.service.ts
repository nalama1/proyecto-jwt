import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthResponse } from '../login/auth-response';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenSubject = new BehaviorSubject<string | null>(localStorage.getItem('token'));
  public token$ = this.tokenSubject.asObservable();
  private apiUrl = environment.apiBaseUrl + '/Usuarios/login';

  constructor(private http: HttpClient, private router: Router) {}

    login(authRequest: { Usuario: string; Password: string }): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}`, authRequest).pipe(
          tap(response => {
            if (response.exito === 1 && response.data) {
              this.setToken(response.data.token);
            } else {
              console.error("Error de autenticación:", response.mensaje);
            }
          })
        );
    }
 
  logout(): void {
    localStorage.removeItem('token'); // Elimina el token
    this.tokenSubject.next(null);
    localStorage.removeItem('usuario'); // Opcional: Elimina datos del usuario
    this.router.navigate(['/login']); 
  }

  private setToken(token: string): void {
    localStorage.setItem('token', token);
    this.tokenSubject.next(token);
  }

 /*  getToken(): string | null {
    return this.tokenSubject.value;
  }
 */
  getToken(): string | null {
    return localStorage.getItem('token');
  }


}