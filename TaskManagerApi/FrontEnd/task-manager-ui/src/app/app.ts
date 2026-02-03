import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TaskListComponent],
  template: '<app-task-list></app-task-list>'
})
export class App {
  protected readonly title = signal('task-manager-ui');
}
