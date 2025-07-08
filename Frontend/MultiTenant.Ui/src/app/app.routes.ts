//import { Routes } from '@angular/router';

//export const routes: Routes = [];

import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { UserComponent } from './components/user/user';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'user', component: UserComponent }
];

