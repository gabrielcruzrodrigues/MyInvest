import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { AuthService } from './services/auth.service';
import { FooterComponent } from './components/layout/footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'MyInvestClient';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    var expirationTokenDate = this.authService.getExpirationTokenDate();
    
    if (!expirationTokenDate)
    {
      return;
    }

    const expirationDate = new Date(expirationTokenDate);
    const currentDate = new Date();

    if (currentDate >= expirationDate)
    {
      this.authService.NewAccessToken();
    }
  }
}
