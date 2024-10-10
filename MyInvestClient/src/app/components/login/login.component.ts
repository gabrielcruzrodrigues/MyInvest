import { Component } from '@angular/core';
import { LoadingComponent } from '../layout/loading/loading.component';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [LoadingComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  form: FormGroup;
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: Router
  ) {
    this.form = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() : void
  {
    if (this.form.invalid)
    {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.authService.login(this.form.value).subscribe({
      next: (response: HttpResponse<any>) => {
        this.authService.configureLocalStorage(response.body);
        this.isLoading = false;
        alert("Login efetuado com sucesso!");
        this.route.navigate(["/purses"]);
        return;
      },
      error: (err) => {
        console.log(err);
        if (err.status === 401)
        {
          alert("Credenciais incorretas, tente novamente!");
          this.isLoading = false;
          return;
        }
        alert("Aconteceu um erro ao tentar fazer login!");
        this.route.navigate(["/"]);
      }
    })
  }
}
