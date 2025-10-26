import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { ProjectComponent } from './pages/projects/project.component';
import { TasksComponent } from './pages/tasks/tasks.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'project', component: ProjectComponent },
  { path: 'tasks/:id', component: TasksComponent },
];
