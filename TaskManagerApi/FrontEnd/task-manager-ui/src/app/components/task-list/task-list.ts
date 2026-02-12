import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../services/task.spec'
import { TaskItem } from '../../models/task-item'
import { TaskItemComponent } from '../task-item/task-item'
import { ActivatedRoute, Router } from '@angular/router'

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule, TaskItemComponent],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})

export class TaskListComponent implements OnInit {

  tasks: TaskItem[] = [];
  newTaskTitle = '';
  loading = false;
  errorMessage = '';

  filter: 'all' | 'active' | 'completed' = 'all';

  constructor(private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
      const urlFilter = params['filter'];

      if (urlFilter === 'active' || urlFilter === 'completed') {
        this.filter = urlFilter;
      } else {
        this.filter = 'all';
      }
    });

    this.loadTasks();
  }

  loadTasks() {
    this.loading = true;
    this.errorMessage = '';

    this.taskService.getTasks().subscribe({
      next: data => {
        this.tasks = data;
        this.loading = false;
      },
      error: err => {
        this.errorMessage = "Impossible de charger les tâches";
        this.loading = false;
      }
    });
  }

  addTask() {
    if (!this.newTaskTitle.trim()) return;

    this.taskService.addTask(this.newTaskTitle).subscribe(createdTask => {
      this.tasks.push(createdTask);
      this.newTaskTitle = '';
    });
  }

  toggleTask(task: TaskItem) {
    task.isCompleted = !task.isCompleted;
    this.taskService.updateTask(task).subscribe();
  }

  deleteTask(id: number) {
    this.taskService.deleteTask(id).subscribe(() => {
      this.tasks = this.tasks.filter(t => t.id !== id);
    });
  }

  updateTask(task: TaskItem) {
    // this.taskService.updateTask(task).subscribe(() => this.loadTasks());
    // Update UI immediately
    this.tasks = this.tasks.map(t =>
      t.id === task.id ? { ...task } : t
    );

    // API call in background
    this.taskService.updateTask(task).subscribe({
      error: () => this.loadTasks() // rollback
    }
    );
  }

  get filteredTasks(): TaskItem[] {
    switch (this.filter) {
      case 'active':
        return this.tasks.filter(t => !t.isCompleted);
      case 'completed':
        return this.tasks.filter(t => t.isCompleted);
      default:
        return this.tasks;
    }
  }

  get remainingCount(): number {
    return this.tasks.filter(t => !t.isCompleted).length;
  }

  setFilter(value: 'all' | 'active' | 'completed') {
    this.filter = value;

    this.router.navigate([], {
      queryParams: { filter: value },
      queryParamsHandling: 'merge'
    })
  }
}
