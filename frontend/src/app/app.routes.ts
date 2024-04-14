import { Routes } from '@angular/router';
import { RestaurantsComponent } from './restaurants/restaurants.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { noAuthGuard } from './guards/no-auth.guard';
import { authGuard } from './guards/auth.guard';
import { AdminComponent } from './admin/admin.component';
import { OwnerComponent } from './owner/owner.component';
import { adminGuard } from './guards/admin.guard';
import { ownerGuard } from './guards/owner.guard';

export const routes: Routes = [
    { path: '', redirectTo: '/restaurants', pathMatch: 'full' },
    { path: 'restaurants', component: RestaurantsComponent },
    { path: 'profile', component: ProfileComponent, canActivate: [noAuthGuard] },
    { path: 'login', component: LoginComponent, canActivate: [authGuard] },
    { path: 'register', component: RegisterComponent, canActivate: [authGuard] },
    { path: 'admin', component: AdminComponent, canActivate: [adminGuard] },
    { path: 'owner', component: OwnerComponent, canActivate: [ownerGuard] }
];