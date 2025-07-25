import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service'; 
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  providers: [AuthService],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  username = "";
  email = '';
  password = '';
  confirmPassword = '';
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  
  validateEmail(email: string): boolean {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email) && email.length <= 255;
  }

  validatePassword(password: string): boolean {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,50}$/;
    return regex.test(password);
  }

  onSignup() {

    if (!this.username.trim()) {
      this.errorMessage = 'Username cannot be empty.';
      return;
    }
    if (!this.validateEmail(this.email)) {
      this.errorMessage = 'Please enter a valid email (max 255 characters).';
      return;
    }
    if (!this.validatePassword(this.password)) {
      this.errorMessage =
        'Password must be 8-50 characters long and include uppercase, lowercase, number, and special character.';
      return;
    }
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    const newUser = {
      email: this.email,
      password: this.password,
      username: this.username
    };

    this.authService.register(newUser).subscribe({
      next: (res) => {
        alert('Registered successfully!');
        this.router.navigate(['/login']); 
      },
      error: (err) => {
        console.error('Registration error:', err);
        let message = 'Registration failed!';
        if (err.error) {
          if (typeof err.error === 'string') {
            message = err.error;
          }
          else if (err.error.message) {
            message = err.error.message;
          } 
    
        else if (typeof err.error === 'object') {
          message = JSON.stringify(err.error);
        }
  }
  alert(message);
      }
    });
  }
}


