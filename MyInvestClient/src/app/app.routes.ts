import { Routes } from '@angular/router';
import { HomepageComponent } from './components/homepage/homepage.component';
import { CreateAccountComponent } from './components/create-account/create-account.component';
import { ViewPursesComponent } from './components/view-purses/view-purses.component';
import { CreatePurseComponent } from './components/create-purse/create-purse.component';
import { SearchTickerComponent } from './components/search-ticker/search-ticker.component';
import { ViewTickerComponent } from './components/view-ticker/view-ticker.component';

export const routes: Routes = [
     {
          path: '', component: SearchTickerComponent
     },
     {
          path: 'create-account', component: CreateAccountComponent
     },
     {
          path: 'purses', component: ViewPursesComponent
     },
     {
          path: 'create-purse', component: CreatePurseComponent
     },
     {
          path: 'search-ticker', component: SearchTickerComponent
     },
     {
          path: 'view-ticker/:name', component: ViewTickerComponent
     }
];
