import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'MyInvestClient';

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    var expirationTokenDate = this.authService.getExpirationTokenDate();
    if (expirationTokenDate)
    {
      const expirationDate = new Date(expirationTokenDate);
      const currentDate = new Date();

      if (currentDate >= expirationDate)
      {
        this.authService.getNewAccessToken();
      }
    }
  }
}
