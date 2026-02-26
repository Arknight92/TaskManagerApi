import { TestBed } from '@angular/core/testing';

// import { Task } from './task';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskItem } from '../models/task-item';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'https://localhost:7021/api/Task';

  constructor(private http: HttpClient) { }
  getTasks(): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(this.apiUrl);
  }

  addTask(title: string, description?: string): Observable<TaskItem> {
    return this.http.post<TaskItem>(this.apiUrl, {
      title,
      description
    });
  }

  updateTask(task: TaskItem): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${task.id}`, {
      title: task.title,
      description: task.description ?? '',
      isCompleted: task.isCompleted
    });
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
