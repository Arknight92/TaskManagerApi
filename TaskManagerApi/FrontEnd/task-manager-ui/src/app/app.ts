import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth';
import { TaskListComponent } from './components/task-list/task-list';
import { Observable } from '../../node_modules/rxjs/dist/types/internal/Observable';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, CommonModule],
  templateUrl: './app.html'
})
export class App {
  protected readonly title = signal('task-manager-ui');

  isLoggedIn!: Observable<boolean>;

  constructor(private auth: AuthService) {
    this.isLoggedIn = this.auth.isLoggedIn$;
  }

  logout() {
    this.auth.logout();
  }
}
