import { HttpResponse } from '@angular/common/http';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppService } from '@services/app.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-request-login-by-code',
  templateUrl: './request-login-by-code.component.html',
  styleUrl: './request-login-by-code.component.scss'
})
export class RequestLoginByCodeComponent implements OnInit {
  form: FormGroup;
  isLoading: boolean = false;

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder,
    private route: Router,
    private renderer: Renderer2,
    private appService: AppService
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    })
  }

  ngOnInit(): void {
    this.renderer.addClass(
      document.querySelector('app-root'),
      'register-page'
    );
  }

  loginByCode(): void {
    if (this.form.invalid) {
      const emailErrors = this.form.get('email')?.errors;

      if (emailErrors?.['required']) {
        this.toastr.error('O campo de email é obrigatório!');
      }

      if (emailErrors?.['email']) {
        this.toastr.error('Por favor, insira um email válido!');
      }
    }

    this.isLoading = true;
    this.appService.requestCodeToLogin(this.form.get('email')?.value).subscribe({
      next: (response: HttpResponse<any>) => {
        this.toastr.success("O código foi enviado para o seu email com sucesso!");
        this.prepareForReceiveCode();
      },
      error: (err) => {
        this.isLoading = false;
        this.toastr.error("Ocorreu um erro ao solicitar o código de acesso");
      }
    })
  }

  prepareForReceiveCode() {
    this.isLoading = false;
  }
}
