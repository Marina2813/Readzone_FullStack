import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-forgot-password',
  standalone: false,
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {

  email = '';
  newPassword = '';
  confirmPassword = '';
  message = '';
  error = '';

  constructor(private http: HttpClient, private router: Router) {}

  resetPassword() {
    this.message = '';
    this.error = '';

    if (this.newPassword !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    const payload = {
      email: this.email,
      newPassword: this.newPassword
    };

    this.http.post('https://localhost:7216/api/auth/reset-password', payload).subscribe({
      next: () => {
        this.message = 'Password updated successfully! Redirecting to login...';
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: () => {
        this.error = 'Failed to reset password. Please try again.';
      }
    });
  }
}
