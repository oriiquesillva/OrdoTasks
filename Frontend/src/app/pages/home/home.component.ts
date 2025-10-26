import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiService } from '@shared/service/api/api.service';
import { DashboardMetrics } from '@shared/service/api/DTOs/DashboardMetrics';
import { BehaviorSubject, firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, CommonModule],
  providers: [ApiService],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  private apiService = inject(ApiService);
  public dadosMetricas$ = new BehaviorSubject<DashboardMetrics | null>(null);

  async ngOnInit() {
    const dadosMetricas = await firstValueFrom(this.apiService.getMetricas());
    this.dadosMetricas$.next(dadosMetricas);
  }
}
