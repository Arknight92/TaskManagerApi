import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../services/task.spec'
import { TaskItem } from '../../models/task-item'
import { TaskItemComponent } from '../task-item/task-item'

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
  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
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

    this.taskService.addTask(this.newTaskTitle).subscribe(() => {
      this.newTaskTitle = '';
      this.loadTasks();
    });
  }

  toggleTask(task: TaskItem) {
    task.isCompleted = !task.isCompleted;
    this.taskService.updateTask(task).subscribe();
  }

  deleteTask(id: number) {
    this.taskService.deleteTask(id).subscribe(() => this.loadTasks());
  }

  updateTask(task: TaskItem) {
    this.taskService.updateTask(task).subscribe(() => this.loadTasks());
  }

}
