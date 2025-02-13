import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { Observable } from "rxjs"
import { tap } from "rxjs/operators"
import { environment } from '../../environments/environment';
import { AuthResponse } from '../auth/auth-response.model';

@Injectable({
  providedIn: "root",
})
export class UserService {
  private apiUrl = environment.apiBaseUrl + '/Usuarios';

  constructor(private httpc: HttpClient) {}

  createUser(NumeroIdentificacion: string, codigoVerificacion: string, userData: any): Observable<any> {
    console.log("Datos enviados:", userData);

    return this.httpc.post<AuthResponse>(`${this.apiUrl}/${NumeroIdentificacion}/${codigoVerificacion}`, userData)
    .pipe(
      tap(response => {
        if(response && response.Token){
            localStorage.setItem('token', response.Token);
        }
      })
    )}
}

