import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  @ViewChild('hamburger', { static: false }) hamburger!: ElementRef;
  @ViewChild('nav', { static: false }) nav!: ElementRef;
  @ViewChild('purses') purses!: ElementRef;
  @ViewChild('myaccount') myaccount!: ElementRef;
  @ViewChild('login') login!: ElementRef;
  @ViewChild('createAccount') createAccount!: ElementRef;

  constructor(
    private authService: AuthService, private router: Router
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
    this.hamburger.nativeElement.addEventListener('click', () => {
      this.nav.nativeElement.classList.toggle('active');
    });

    if (this.authService.verifyIfUserIdLogged()) {
      this.purses.nativeElement.classList.add('active');
      this.myaccount.nativeElement.classList.add('active');
      this.createAccount.nativeElement.classList.add('no-active');
      this.login.nativeElement.classList.add('no-active');
    }
  }
}
