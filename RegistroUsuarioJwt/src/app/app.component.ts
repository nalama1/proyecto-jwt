import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { environment } from '../environments/environment';
import { Router, NavigationEnd } from '@angular/router';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  environment = environment;
  mostrarBotones: boolean = true;

  constructor(private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        // Ocultar los botones cuando este en persona-lista-registration
        this.mostrarBotones = event.url !== '/listar-persona';
      }
    });
  }
 
  navegar() {
    window.location.href = environment.loginUrl;  
  }

}