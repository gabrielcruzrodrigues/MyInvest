import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from '../layout/navbar/navbar.component';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoadingComponent } from '../layout/loading/loading.component';

@Component({
  selector: 'app-create-account',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, LoadingComponent],
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.scss'
})
export class CreateAccountComponent {
  form: FormGroup;
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private route: Router
  ) {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, this.passwordStrengthValidator()]],
      verifyPassword: ['', Validators.required]
    }, { validators: this.passwordsMatchValidator() });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      const formErrors = this.getFormValidationErrors();
      alert(`Erros no formulário: \n${formErrors}`);
      return;
    }

    this.isLoading = true;
    this.authService.createAccount(this.form.value).subscribe({
      next: (response: HttpResponse<any>) => {
        if (response.status === 201) {
          this.authService.configureLocalStorage(response.body);
          this.isLoading = false;
          alert("Sua conta foi criada com sucesso!");
          this.route.navigate(["/purses"]);
          return;
        }
        alert("Uma resposta inédita foi recebida do servidor!");
      },
      error: (error) => {
        this.isLoading = false;
        console.log(error);
        if (error.status === 400) {
          alert(error.error.message);
          return;
        }
        alert("Ocorreu um erro ao tentar criar o usuário.");
      }
    })
  }

  passwordsMatchValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.get('password')?.value;
      const verifyPassword = control.get('verifyPassword')?.value;

      if (password && verifyPassword && password !== verifyPassword) {
        return { passwordsMismatch: true };
      }
      return null;
    };
  }

  passwordStrengthValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null;
      }

      const isValidLength = value.length >= 8;

      if (!isValidLength) {
        return { passwordStrength: true };
      }
      return null;
    }
  }

  getFormValidationErrors(): string {
    let errors: string[] = [];
  
    Object.keys(this.form.controls).forEach(key => {
      const controlErrors = this.form.get(key)?.errors;
      if (controlErrors) {
        Object.keys(controlErrors).forEach(errorKey => {
          switch (errorKey) {
            case 'required':
              errors.push(`${key}: Este campo é obrigatório.`);
              break;
            case 'minlength':
              const minLength = controlErrors['minlength'].requiredLength;
              errors.push(`${key}: O valor precisa ter pelo menos ${minLength} caracteres.`);
              break;
            case 'email':
              errors.push(`${key}: O formato do email está incorreto.`);
              break;
            case 'passwordStrength':
              errors.push(`password: A senha precisa ter pelo menos 8 caracteres.`);
              break;
            case 'passwordsMismatch':
              errors.push(`password: As senhas não coincidem.`);
              break;
            default:
              errors.push(`${key}: Erro desconhecido.`);
              break;
          }
        });
      }
    });
  
    return errors.join('\n');
  }
}
