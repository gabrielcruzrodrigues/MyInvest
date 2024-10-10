import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-back',
  standalone: true,
  imports: [],
  templateUrl: './back.component.html',
  styleUrl: './back.component.scss'
})
export class BackComponent {
  @Input() link: string = '';

  constructor(private router: Router) {}

  redirect(): void {
    this.router.navigate([this.link]);
  }
}
