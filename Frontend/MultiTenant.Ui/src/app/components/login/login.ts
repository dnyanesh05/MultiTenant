//import { Component } from '@angular/core';

//@Component({
//  selector: 'app-login',
//  imports: [],
//  templateUrl: './login.html',
//  styleUrl: './login.css'
//})
//export class Login {

//}

import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service' // 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  username = '';
  password = '';
  success = '';
  error = '';
  loading = false;

  constructor(private auth: AuthService, private router: Router) { }

  login() {
    this.loading = true;
    this.auth.login(this.username, this.password).subscribe({
      next: res => {
        this.auth.saveToken(res.token);
        this.success = 'Login successful! Redirecting...';

        // Redirect after short delay
        setTimeout(() => {
          this.router.navigate(['/user']);
        }, 1500);
        //this.router.navigate(['/user']); // or any other route
      },
      error: () => {
        this.error = 'Invalid credentials';
        this.loading = false;
      }
    });
  }
}

