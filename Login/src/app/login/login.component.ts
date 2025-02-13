import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';  
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  
  constructor(private snackBar: MatSnackBar) {}

  loginForm = this.fb.group({
    usuario: ['', [Validators.required]],
    password: ['', [Validators.required ]] 
  });

  onSubmit() {
    if (this.loginForm.valid) {
      const { usuario, password } = this.loginForm.value;

      const authRequest = {
        Usuario: usuario!,
        Password: password!
      };

      this.authService.login(authRequest).subscribe({
        next: () => {          
          this.snackBar.open("Ingreso exitoso!", "Cerrar"); 
          window.location.href = environment.loginUrl;
          //this.router.navigate(['/dashboard']);

        },
        error: (error) => {
          console.error('Login failed', error);
          let errorMessage = "Ocurrió un error inesperado. Intente nuevamente.";
          if (error.status === 400) {
            errorMessage = "Usuario o contraseña incorrecta.";
          }
          /* if (error.status === 409) {
            errorMessage = " .";
          } */
          this.snackBar.open(errorMessage, "Cerrar") 
          
        }
      });
    }
  }
}