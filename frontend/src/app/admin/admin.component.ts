import { Component } from '@angular/core';
import { ApiService } from '../services/api.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { existingEmailValidator } from '../validators/register-validators';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
  roleForm: FormGroup;
  removeForm: FormGroup;

  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthService) {
    this.roleForm = this.fb.group({
      email: ['', [Validators.required, Validators.email], [existingEmailValidator(this.authService)]],
      role: ['', Validators.required]
    });

    this.removeForm = this.fb.group({
      email: ['', [Validators.required, Validators.email], [existingEmailValidator(this.authService)]],
      role: ['', Validators.required]
    });
  }

  assignRole() {
    if (this.roleForm.valid) {
      this.apiService.assignRole(this.roleForm.value.email, this.roleForm.value.role).subscribe({
        next: (data) => {
          console.log('Role assigned successfully', data);
        },
        error: (error) => console.error('Error assigning role', error)
      });
    }
  }

  removeRole() {
    if (this.removeForm.valid) {
        this.apiService.removeRole(this.removeForm.value.email, this.removeForm.value.role).subscribe({
            next: (data) => {
                console.log('Role removed successfully', data);
            },
            error: (error) => console.log('This user does not have this role!')
        });
    }
  }
}
