import { Component, Inject , OnInit} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-editar-persona',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './editar-persona.component.html',
  styleUrls: ['./editar-persona.component.css']
})
export class EditarPersonaComponent  {
  editForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<EditarPersonaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    console.log('Datos recibidos en el popup:', data); 
    this.editForm = this.fb.group({
      nombres: [{ value: data.nombres , disabled: true}],   
      apellidos: [{ value: data.apellidos , disabled: true}],  
      email: [data.email]
      
    });
  }

  onCancelar() {
    this.dialogRef.close();
  }

  onEditar() {
    if (this.editForm.valid) {
      const updatedData = {
        numeroIdentificacion: this.data.numeroIdentificacion,
        email: this.editForm.value.email
      };
      this.dialogRef.close(updatedData);
    }
  }
}
