import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {MainComponent} from '@modules/main/main.component';
import {ProfileComponent} from '@pages/profile/profile.component';
import {RegisterComponent} from '@modules/register/register.component';
import {NonAuthGuard} from '@guards/non-auth.guard';
import {ForgotPasswordComponent} from '@modules/forgot-password/forgot-password.component';
import { PursesListComponent } from '@pages/purses-list/purses-list.component';
import { ActivesListComponent } from '@pages/actives-list/actives-list.component';
import { CreatePurseComponent } from '@pages/create-purse/create-purse.component';
import { CreateActiveComponent } from '@pages/create-active/create-active.component';
import { UpdateActiveComponent } from '@pages/update-active/update-active.component';
import { LoginPageComponent } from '@pages/login-page/login-page.component';
import { EditPurseComponent } from '@pages/edit-purse/edit-purse.component';
import { RequestLoginByCodeComponent } from '@pages/request-login-by-code/request-login-by-code.component';
import { RecoverPasswordComponent } from '@pages/recover-password/recover-password.component';
import { RequestRecoverPasswordComponent } from '@pages/request-recover-password/request-recover-password.component';
import { ListSmtpProfilesComponent } from '@pages/list-smtp-profiles/list-smtp-profiles.component';
import { CreateSmtpProfilesComponent } from '@pages/create-smtp-profiles/create-smtp-profiles.component';
import { SmptPanelComponent } from '@pages/smpt-panel/smpt-panel.component';

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
                path: 'edit-purse/:id',
                component: EditPurseComponent
            },
            {
                path: 'update-active/:activeId/:name/:percentValue',
                component: UpdateActiveComponent
            },
            {
                path: 'recover-password',
                component: RecoverPasswordComponent
            },
            {
                path: 'request-recover-password',
                component: RequestRecoverPasswordComponent
            },
            {
                path: 'list-smtp-properties',
                component: ListSmtpProfilesComponent
            },
            {
                path: 'create-smtp-properties',
                component: CreateSmtpProfilesComponent
            },
            {
                path: 'smtp-panel',
                component: SmptPanelComponent
            }
        ]
    },
    {
        path: 'login',
        component: LoginPageComponent,
        canActivate: [NonAuthGuard]
    },
    {
        path: 'register',
        component: RegisterComponent,
        canActivate: [NonAuthGuard]
    },
    {
        path: 'request-login-by-code',
        component: RequestLoginByCodeComponent,
        // canActivate: [NonAuthGuard]
    },
    {
        path: 'forgot-password',
        component: ForgotPasswordComponent,
        canActivate: [NonAuthGuard]
    },
    {path: '**', redirectTo: ''}
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {})],
    exports: [RouterModule]
})
export class AppRoutingModule {}
