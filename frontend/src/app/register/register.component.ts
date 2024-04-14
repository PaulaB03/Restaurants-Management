import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { passwordValidator, uniqueEmailValidator, uniqueUsernameValidator } from '../validators/register-validators';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  currentStep = 1;
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      account: this.fb.group({
        userName: ['', [Validators.required], [uniqueUsernameValidator(this.authService)]],
        email: ['', [Validators.required, Validators.email], [uniqueEmailValidator(this.authService)]],
        password: ['', [Validators.required, passwordValidator()]]
      }),
      personal: this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        phoneNumber: ['', Validators.required]
      }),
      address: this.fb.group({
        street: ['', Validators.required],
        floor: [''], // Optional
        number: ['', Validators.required],
        city: ['', Validators.required]
      })
    });
  }

  goToStep(step: number): void {
    this.currentStep = step;
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerData = {
        userName: this.registerForm.get('account.userName')?.value,
        email: this.registerForm.get('account.email')?.value,
        password: this.registerForm.get('account.password')?.value,
        firstName: this.registerForm.get('personal.firstName')?.value,
        lastName: this.registerForm.get('personal.lastName')?.value,
        phoneNumber: this.registerForm.get('personal.phoneNumber')?.value,
        address: {
          street: this.registerForm.get('address.street')?.value,
          floor: this.registerForm.get('address.floor')?.value,
          number: this.registerForm.get('address.number')?.value,
          city: this.registerForm.get('address.city')?.value
        }
      };

      this.authService.register(registerData).subscribe({
        next: response => this.router.navigate(['login']),
        error: error => {
          console.error('Registration failed:', error.error.text || error.message);
        }
      });
    } else {
      console.log('Form errors:', this.registerForm.errors);
    }
  }
}