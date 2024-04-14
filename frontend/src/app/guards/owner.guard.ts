import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const ownerGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  return authService.hasRole('Owner');
};
