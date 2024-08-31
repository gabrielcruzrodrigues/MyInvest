import { Routes } from '@angular/router';
import { HomepageComponent } from './components/homepage/homepage.component';
import { CreateAccountComponent } from './components/create-account/create-account.component';
import { ViewPursesComponent } from './components/view-purses/view-purses.component';

export const routes: Routes = [
     {
          path: '', component: HomepageComponent
     },
     {
          path: 'create-account', component: CreateAccountComponent
     },
     {
          path: 'purses', component: ViewPursesComponent
     }
];
