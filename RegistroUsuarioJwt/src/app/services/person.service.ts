import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { Observable } from "rxjs"
import { tap } from "rxjs/operators"
import { environment } from '../../environments/environment';
import { AuthResponse } from '../auth/auth-response.model';

interface Persona {
  id?: number;
  nombres: string;
  apellidos: string;
  numeroIdentificacion: string;
  email: string;
  tipoIdentificacion: string;
  tipoIdentificacionID: number;
  fechaCreacion?: Date;
  eliminado?: string;
}

@Injectable({
  providedIn: "root",
})
export class PersonService {
  private apiUrl = environment.apiBaseUrl + '/Personas';

  constructor(private http: HttpClient) {}

  getPersonas(): Observable<Persona[]> {
    return this.http.get<Persona[]>(this.apiUrl);
  }

  getPersonaByNumero(numeroIdentificacion: string): Observable<Persona> {
    return this.http.get<Persona>(`${this.apiUrl}/${numeroIdentificacion}`);
  }

  actualizarPersona(numeroIdentificacion: string, email: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${numeroIdentificacion}/${email}`, {}); 
}

  eliminarPersona(numeroIdentificacion: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${numeroIdentificacion}`);
  }

  createPerson(personData: any): Observable<any> {
    console.log("Datos enviados:", personData);

    return this.http.post<AuthResponse>(`${this.apiUrl}`, personData)
    .pipe(
      tap(response => {
        if(response && response.Token){
            localStorage.setItem('token', response.Token);
        }
      })
    )}
}
