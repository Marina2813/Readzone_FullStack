import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service'; 

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule],
  providers: [AuthService],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  username = "";
  email = '';
  password = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSignup() {
    const newUser = {
      email: this.email,
      passwordHash: this.password,
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


