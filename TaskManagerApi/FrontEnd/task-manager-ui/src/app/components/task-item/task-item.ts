import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from "@angular/common";
import { TaskItem } from "../../models/task-item";
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-item',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-item.html',
  styleUrl: './task-item.css',
})
export class TaskItemComponent {
  @Input() task!: TaskItem;
  @Output() toggle = new EventEmitter<TaskItem>();
  @Output() delete = new EventEmitter<number>();

  isEditing = false;
  editedTitle = '';

  startEdit() {
    this.editedTitle = this.task.title;
    this.isEditing = true;
  }

  saveEdit() {
    if (!this.editedTitle.trim()) return;

    const updatedTask = { ...this.task, title: this.editedTitle };
    this.toggle.emit(updatedTask); // on reutilise event update
    this.isEditing = false;
  }

  cancelEdit() {
    this.isEditing = false;
  }

  onToggle() {
    const updatedTask: TaskItem = {
      ...this.task,
      isCompleted: !this.task.isCompleted
    };

    this.toggle.emit(updatedTask);
  }

  onDelete() {
    this.delete.emit(this.task.id);
  }
}
