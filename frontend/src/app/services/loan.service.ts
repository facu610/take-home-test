import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Loan } from '../models/loan';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  private readonly baseUrl = '/api';

  constructor(private http: HttpClient) {}

  getLoans(): Observable<Loan[]> {
    return this.http.get<Loan[]>(`${this.baseUrl}/loans`);
  }
}