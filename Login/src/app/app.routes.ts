import { Routes } from '@angular/router';
import { LoginComponent } from "./login/login.component"
import { authGuard } from '././security/auth.guard';
import { DashboardComponent } from "./dashboard/dashboard.component";
 
//export const routes: Routes = [];
 
export const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: "login", component: LoginComponent },
  { path: "", redirectTo: "/login", pathMatch: "full" },
];
