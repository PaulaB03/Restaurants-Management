import { AbstractControl, AsyncValidatorFn, ValidationErrors, ValidatorFn } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) return null;  // If no input, return null (no error)

    const value = control.value;
    const hasUpperCase = /[A-Z]/.test(value);
    const hasLowerCase = /[a-z]/.test(value);
    const hasNumeric = /\d/.test(value);
    const hasNonAlphanumeric = /\W/.test(value);  
    const isValidLength = value.length >= 10;

    const errors = {
      passwordStrength: 'Password must be at least 10 characters and include upper, lower, numeric, and special characters.'
    };

    return hasUpperCase && hasLowerCase && hasNumeric && hasNonAlphanumeric && isValidLength ? null : errors;
  };
}

export function uniqueEmailValidator(authService: AuthService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return authService.checkEmailExists(control.value).pipe(
      map(isUnique => isUnique ? null : { uniqueEmail: true }),
      catchError(() => of(null))
    );
  };
}

export function existingEmailValidator(authService: AuthService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return authService.checkEmailExists(control.value).pipe(
      map(isUnique => isUnique ? { uniqueEmail: true } : null),
      catchError(() => of(null))
    );
  };
}
  
export function uniqueUsernameValidator(authService: AuthService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return authService.checkUsernameExists(control.value).pipe(
      map(isUnique => isUnique ? null : { uniqueUsername: true }),
      catchError(() => of(null))
    );
  };
}