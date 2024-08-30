import { Routes } from '@angular/router';
import { HomepageComponent } from './components/homepage/homepage.component';
import { CreateAccountComponent } from './components/create-account/create-account.component';

export const routes: Routes = [
     {
          path: '', component: HomepageComponent
     },
     {
          path: 'account', component: CreateAccountComponent
     }
];
