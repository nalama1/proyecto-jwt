import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, AbstractControl, ValidationErrors, Validators } from '@angular/forms';
import { PersonService } from '../services/person.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { CommonModule } from "@angular/common";
import { MatDialog } from '@angular/material/dialog';
import { EditarPersonaComponent } from '../components/editar-persona/editar-persona.component';


interface Persona {
  id?: number;
  nombres: string;
  apellidos: string;
  numeroIdentificacion: string;
  email: string;
  tipoIdentificacion: string;
  tipoIdentificacionID: number;
  eliminado?: string;
}

@Component({
  selector: 'app-persona-lista-registration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './persona-lista-registration.component.html',
  styleUrls: ['./persona-lista-registration.component.css']
})
export class PersonaListaRegistrationComponent implements OnInit {
  searchForm!: FormGroup;
  personas: Persona[] = [];
  tipoIdentificacions = [
    { value: 1, label: 'Cédula' },
    { value: 2, label: 'Pasaporte' }
  ];

  constructor(
    private fb: FormBuilder,
    private personaService: PersonService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog

  ) {}

  ngOnInit(): void {
    this.searchForm = this.fb.group({
      tipoIdentificacionID: [''],
      numeroIdentificacion: ['']
    });
  }

  onMostrar(): void {
    const numeroIdentificacion = this.searchForm.get('numeroIdentificacion')?.value;
  
    if (numeroIdentificacion) {
      this.personaService.getPersonaByNumero(numeroIdentificacion).subscribe({
        next: (persona) => {
          this.personas = persona ? [persona] : [];
          if (!persona) {
            this.snackBar.open(`No se encontró la persona con número de identificación ${numeroIdentificacion}`, 'Cerrar', { duration: 3000 });
          }
        },
        error: (error) => {
          let errorMessage = "Ocurrió un error inesperado. Intente nuevamente.";
          if (error.status === 404) {
            errorMessage = `Persona con Número de Identificación ${numeroIdentificacion} no encontrado o eliminado`;  
          }
          if (error.status === 409) {
            errorMessage = "Error al consultar persona";
          }
          this.snackBar.open(errorMessage, "Cerrar");
        }
      });
    } else {
      this.personaService.getPersonas().subscribe({
        next: (data) => this.personas = data,
        error: () => this.snackBar.open('Error al cargar la lista', 'Cerrar', { duration: 3000 })
      });
    }
  }


  //Editar Persona:
  onEditar(numeroIdentificacion: string, persona: any) {
    console.log('Editando persona:', persona);
    const dialogRef = this.dialog.open(EditarPersonaComponent, {
      width: '400px',
      data: {
        numeroIdentificacion: persona.numeroIdentificacion,
        nombres: persona.nombres,
        apellidos: persona.apellidos,
        email: persona.email
      }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log("Datos editados:", result);
        this.actualizarPersona(result.numeroIdentificacion, result.email); 
      }
    });
  }

  actualizarPersona(numeroIdentificacion: string, email: string) {
    this.personaService.actualizarPersona(numeroIdentificacion, email)
      .subscribe({
        next: () => {
          this.snackBar.open('Persona actualizado correctamente', 'Cerrar', { duration: 3000 });
          this.onMostrar();
        },
        error: () => this.snackBar.open('Error actualizando persona', 'Cerrar', { duration: 3000 })
      });
  }

  //Eliminar Persona
  onEliminar(numeroIdentificacion: string): void {
    if (confirm('¿Está seguro de eliminar esta persona?')) {
      this.personaService.eliminarPersona(numeroIdentificacion).subscribe({
        next: () => {
          this.snackBar.open('Persona eliminada', 'Cerrar', { duration: 3000 });
          this.onMostrar();
        },
        error: () => this.snackBar.open('Error al eliminar persona', 'Cerrar', { duration: 3000 })
      });
    }
  }
}
