export interface Loan {
  id: number;
  amount: number;
  currentBalance: number;
  applicantName: string;
  status: number; // 0 = Active, 1 = Paid
  createdAt: string;
}