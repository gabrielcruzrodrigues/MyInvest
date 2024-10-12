import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  @ViewChild('purses') purses!: ElementRef;
  @ViewChild('createAccount') createAccount!: ElementRef;
  @ViewChild('login') login!: ElementRef;
  @ViewChild('logout') logout!: ElementRef;

  constructor(
    private authService: AuthService, private router: Router, private cdr: ChangeDetectorRef
  ) 
  {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.initComponent();
      });
  }

  ngAfterViewInit(): void {
    this.initComponent();
  }

  initComponent(): void {
    if (this.authService.verifyIfUserIdLogged()) {
      this.purses.nativeElement.classList.remove('unauthenticated');
      this.createAccount.nativeElement.classList.add('unauthenticated');
      this.login.nativeElement.classList.add('unauthenticated');
      this.logout.nativeElement.classList.remove('unauthenticated');
    }
  }
}
