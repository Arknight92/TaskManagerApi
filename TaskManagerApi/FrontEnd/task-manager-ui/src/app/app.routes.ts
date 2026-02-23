import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { TaskListComponent } from './components/task-list/task-list';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: TaskListComponent, canActivate: [authGuard] }
];
