import { Component, type OnInit } from "@angular/core"
import { UserService } from "../services/user.service"
import { CommonModule } from "@angular/common"
import { FormBuilder } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import {
  ReactiveFormsModule,
  type FormGroup,
  Validators,
  type AbstractControl,
  type ValidationErrors,
} from "@angular/forms"


@Component({
  selector: "app-user-registration",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./user-registration.component.html",
  styleUrls: ["./user-registration.component.css"],
})
export class UserRegistrationComponent implements OnInit {
  registrationForm: FormGroup
  TipoIdentificacions = [
    { value: 1, label: 'Cédula' },
    { value: 2, label: 'Pasaporte' }
  ];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar,
    private router: Router 
  ) {
    this.registrationForm = this.fb.group({
      TipoIdentificacion: ["", Validators.required],
      NumeroIdentificacion: ["", [Validators.required, this.identificationValidator()]],
      username: ["", Validators.required],
      password: ["", [Validators.required, Validators.minLength(8)]],
      codigoVerificacion: ["", [Validators.required, Validators.pattern(/^\d+$/)]],
    })
  }

  ngOnInit(): void {
     this.initForm()
  }

private initForm(): void {
    this.registrationForm = this.fb.group({
      TipoIdentificacion: ["", Validators.required],
      NumeroIdentificacion: ["", [Validators.required, this.identificationValidator()]],
      username: ["", Validators.required],
      password: ["", [Validators.required, Validators.minLength(8)]],
      codigoVerificacion: ["", [Validators.required, Validators.pattern(/^\d+$/)]],
    })
  }

  private identificationValidator() {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value
      const type = this.registrationForm?.get("TipoIdentificacion")?.value

      if (!value) return null

      if (type === "Cédula" && !/^\d{10}$/.test(value)) {
        return { invalidCedula: true }
      }

      if (type === "Pasaporte" && !/^\d{13}$/.test(value)) {
        return { invalidPassport: true }
      }

      return null
    }
  }

 
   onSubmit(): void {
    if (this.registrationForm.valid) {
      const { NumeroIdentificacion, codigoVerificacion, ...userData } = this.registrationForm.value
      this.userService.createUser(NumeroIdentificacion, codigoVerificacion, userData).subscribe({
        next: (response) => {
          this.snackBar.open("Usuario registrado correctamente", "Cerrar"); //, { duration: 3000 });
          window.location.href = environment.loginUrl;
          //this.router.navigate(['/login']); // Redirige a la página de inicio de sesión
        },
        error: (error) => {
          let errorMessage = "Ocurrió un error inesperado. Intente nuevamente.";
          if (error.status === 400) {
            errorMessage = "Código de verificación no válido o persona no registrada o eliminada.";
          }
          if (error.status === 409) {
            errorMessage = "Usuario ya se encuentra registrado.";
          }

          this.snackBar.open(errorMessage, "Cerrar") 
        }
      });
    }
  } 
}
