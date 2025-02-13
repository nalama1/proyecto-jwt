import { Routes } from "@angular/router"
import { UserRegistrationComponent } from "./user-registration/user-registration.component"
import { PersonRegistrationComponent } from "./person-registration/person-registration.component"
import { PersonaListaRegistrationComponent } from "./persona-lista-registration/persona-lista-registration.component"

export const routes: Routes = [
  { path: "crear-usuario", component: UserRegistrationComponent },
  { path: "crear-persona", component: PersonRegistrationComponent },
  { path: "listar-persona", component: PersonaListaRegistrationComponent },
  { path: "", redirectTo: "/crear-persona", pathMatch: "full" },
]
