import { Routes } from '@angular/router';
import { HomepageComponent } from './components/homepage/homepage.component';
import { CreateAccountComponent } from './components/create-account/create-account.component';
import { ViewPursesComponent } from './components/view-purses/view-purses.component';
import { CreatePurseComponent } from './components/create-purse/create-purse.component';

export const routes: Routes = [
     {
          path: '', component: HomepageComponent
     },
     {
          path: 'create-account', component: CreateAccountComponent
     },
     {
          path: 'purses', component: ViewPursesComponent
     },
     {
          path: 'create-purse', component: CreatePurseComponent
     }
];
