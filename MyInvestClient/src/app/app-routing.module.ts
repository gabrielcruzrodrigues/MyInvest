import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {MainComponent} from '@modules/main/main.component';
import {LoginComponent} from '@modules/login/login.component';
import {ProfileComponent} from '@pages/profile/profile.component';
import {RegisterComponent} from '@modules/register/register.component';
import {DashboardComponent} from '@pages/dashboard/dashboard.component';
import {AuthGuard} from '@guards/auth.guard';
import {NonAuthGuard} from '@guards/non-auth.guard';
import {ForgotPasswordComponent} from '@modules/forgot-password/forgot-password.component';
import {RecoverPasswordComponent} from '@modules/recover-password/recover-password.component';
import {SubMenuComponent} from '@pages/main-menu/sub-menu/sub-menu.component';
import { PursesListComponent } from '@pages/purses-list/purses-list.component';
import { ActivesListComponent } from '@pages/actives-list/actives-list.component';
import { CreatePurseComponent } from '@pages/create-purse/create-purse.component';
import { CreateActiveComponent } from '@pages/create-active/create-active.component';
import { UpdateActiveComponent } from '@pages/update-active/update-active.component';

const routes: Routes = [
    {
        path: '',
        component: MainComponent,
        // canActivate: [AuthGuard],
        // canActivateChild: [AuthGuard],
        children: [
            {
                path: 'profile',
                component: ProfileComponent
            },
            {
                path: 'purses',
                component: PursesListComponent
            },
            {
                path: 'view-actives/:purse',
                component: ActivesListComponent
            },
            {
                path: 'create-active/:purseId',
                component: CreateActiveComponent
            },
            {
                path: 'create-active',
                component: CreateActiveComponent
            },
            {
                path: 'create-purse',
                component: CreatePurseComponent
            },
            {
                path: 'update-active/:activeId/:name/:percentValue',
                component: UpdateActiveComponent
            },
        ]
    },
    {
        path: 'login',
        component: LoginComponent,
        canActivate: [NonAuthGuard]
    },
    {
        path: 'register',
        component: RegisterComponent,
        canActivate: [NonAuthGuard]
    },
    {
        path: 'forgot-password',
        component: ForgotPasswordComponent,
        canActivate: [NonAuthGuard]
    },
    {
        path: 'recover-password',
        component: RecoverPasswordComponent,
        canActivate: [NonAuthGuard]
    },
    {path: '**', redirectTo: ''}
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {})],
    exports: [RouterModule]
})
export class AppRoutingModule {}
