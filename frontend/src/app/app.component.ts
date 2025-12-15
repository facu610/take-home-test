import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';

import { Loan } from './models/loan';
import { LoanService } from './services/loan.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = [
    'amount',
    'currentBalance',
    'applicantName',
    'status',
  ];

  loans: Loan[] = [];
  error: string | null = null;

  constructor(private loanService: LoanService) { }

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {

    this.loanService.getLoans().subscribe({
      next: (data: Loan[]) => {
        this.loans = data;
      },
      error: (err) => {
        this.error = 'Failed to load loans. Please try again later.';
      },
    });
  }

  statusLabel(status: number): string {
    return status === 1 ? 'paid' : 'active';
  }
}
