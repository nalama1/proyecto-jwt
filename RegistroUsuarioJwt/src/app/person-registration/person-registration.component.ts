import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, AbstractControl, ValidationErrors, Validators } from '@angular/forms';
import { PersonService } from '../services/person.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { CommonModule } from "@angular/common";


@Component({
  selector: "app-person-registration",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./person-registration.component.html",
  styleUrls: ["./person-registration.component.css"],
})
export class PersonRegistrationComponent implements OnInit {
  registrationPerson!: FormGroup;
  codigoVerificacion: string | null = null; // Variable para almacenar el código
  mostrarCodigo: boolean = false; // Variable para controlar la visibilidad
  mostrarFormGroup: boolean = false;

  TipoIdentificacions = [
    { value: 1, label: 'Cédula' },
    { value: 2, label: 'Pasaporte' }
  ];

  constructor(
    private fb: FormBuilder,
    private personService: PersonService,
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.registrationPerson = this.fb.group({
      TipoIdentificacionID: ["", Validators.required],
      NumeroIdentificacion: ["", [Validators.required, this.identificationValidator()]],
      Nombres: ["", Validators.required],
      Apellidos: ["", [Validators.required]],
      Email: ['', [Validators.required, Validators.pattern('[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]],
      codigoVerificacion: [''],
    });

  }

  get email() { return this.registrationPerson.get('Email'); }


  private initForm(): void {
    this.registrationPerson = this.fb.group({
      TipoIdentificacionID: ["", Validators.required],
      NumeroIdentificacion: ["", [Validators.required, this.identificationValidator()]],
      Nombres: ["", Validators.required],
      Apellidos: ["", [Validators.required]],
      Email: ["", [Validators.required, Validators.pattern(/^\d+$/)]],
      codigoVerificacion: [''],

    })
  }

  /* private identificationValidator() {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value
      const type = this.registrationPerson?.get("TipoIdentificacionID")?.value

      if (!value) return null

      if (type === "Cédula" && !/^\d{10}$/.test(value)) {
        return { invalidCedula: true }
      }

      if (type === "Pasaporte" && !/^\d{13}$/.test(value)) {
        return { invalidPassport: true }
      }

      return null
    }
  }  */

  private identificationValidator() {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;

      const isCedula = /^\d{10}$/.test(value);
      const isPasaporte = /^\d{13}$/.test(value);

      if (isCedula) return null;
      if (isPasaporte) return null;


      return {
        invalidCedula: value.length !== 10,
        invalidPassport: value.length !== 13
      };

    }
  }


  onSubmit(): void {
    if (this.registrationPerson.valid) {
      const { ...personData } = this.registrationPerson.value
      this.personService.createPerson(personData).subscribe({
        next: (response) => {
          this.snackBar.open("Persona registrada correctamente", "Cerrar"); //, { duration: 3000 });

          // mostrar el código de verificación de la respuesta de la API
          this.codigoVerificacion = response.codigoSecuencia;
          console.log("Código de verificación:", this.codigoVerificacion);

          this.mostrarCodigo = true; // Muestra el código
          this.mostrarFormGroup = true;

        },
        error: (error) => {
          this.mostrarCodigo = false;
          this.mostrarFormGroup = false;

          let errorMessage = "Ocurrió un error inesperado. Intente nuevamente.";
          if (error.status === 400) {
            errorMessage = "Error en la solicitud. Por favor, revise los datos ingresados.";
          }
          if (error.error && error.error.mensaje) {
            errorMessage = error.error.mensaje;
          } else if (error.status === 409) {
            errorMessage = error.error;
          } else {
            errorMessage = `Código de error: ${error.status}\nMensaje: ${error.message}`;
          }

          this.snackBar.open(errorMessage, "Cerrar") //, { duration: 3000 });
        }
      });
    }
  }
}
