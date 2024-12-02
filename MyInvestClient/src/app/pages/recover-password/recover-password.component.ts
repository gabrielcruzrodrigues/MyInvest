import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AppService } from '@services/app.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-recover-password',
  templateUrl: './recover-password.component.html',
  styleUrl: './recover-password.component.scss'
})
export class RecoverPasswordComponent implements OnInit {
  isLoading: boolean = false;
  recoverForm: FormGroup;
  token: string;
  userEmail: string;

  constructor(
    private fb: FormBuilder,
    private toast: ToastrService,
    private appService: AppService,
    private activedRoute: ActivatedRoute,
    private router: Router
  ) {
    this.recoverForm = this.fb.group({
      userEmail: ['', [Validators.required]],
      token: ['', [Validators.required]],
      newPassword: ['', Validators.required],
      verifyPassword: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.activedRoute.queryParams.subscribe(params => {
      const email = params['email'] || '';
      const token = params['token'] || '';

      this.recoverForm.patchValue({
        userEmail: email,
        token: token
      });
    });
  }

  onSubmit(): void {
    if (this.verifyPassword()) {
      this.isLoading = true;
      this.appService.recoverPassword(this.recoverForm.value).subscribe({
        next: (response: HttpResponse<any>) => {
          this.isLoading = false;
          this.toast.success("Senha alterada com sucesso!");
          this.router.navigate(['/']);
          return;
        },
        error: (err) => {
          this.isLoading = false;
          console.log(err);
          this.toast.error("Houve um erro ao tentar atualizar a senha, tente mais tarde!");
        }
      })
    }
  }

  verifyPassword(): boolean {
    const newPassword = this.recoverForm.get('newPassword')?.value;
    let valid = true;

    if (newPassword === null) {
      this.toast.error("A senha não pode ser nula!");
      valid = false;
    }

    if (newPassword.length < 8 || newPassword === null) {
      this.toast.error("A senha deve ter mais que 8 caracteres!");
      valid = false;
    }

    if (!/[A-Z]/.test(newPassword)) {
      this.toast.error("A senha deve conter pelo menos uma letra maiúscula!");
      valid = false;
    }

    if (!/[0-9]/.test(newPassword)) {
      this.toast.error("A senha deve conter pelo menos um número!");
      valid = false;
    }

    if (!/[!@#$%^&*(),.?":{}|<>]/.test(newPassword)) {
      this.toast.error("A senha deve conter pelo menos um caractere especial!");
      valid = false;
    }

    if (this.recoverForm.get('verifyPassword')?.value !== newPassword) {
      this.toast.error("As senhas não são iguais!");
      valid = false;
    }

    if (valid) {
      this.recoverForm.patchValue({
        newPassword: newPassword
      });
      return true;
    }

    return false;
  }
}
