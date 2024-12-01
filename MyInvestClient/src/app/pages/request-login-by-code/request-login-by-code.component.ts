import { HttpResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
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
  @ViewChild('page0', { static: true }) page0!: ElementRef<HTMLDivElement>
  @ViewChild('page1', { static: true }) page1!: ElementRef<HTMLDivElement>

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder,
    private route: Router,
    private renderer: Renderer2,
    private appService: AppService
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      code: ['', Validators.required]
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
    this.page0.nativeElement.classList.add('hidden');
    this.page1.nativeElement.classList.remove('hidden');
  }

  verifyCode(): void {
    if (this.form.invalid) {
      const codeErrors = this.form.get('code').errors;

      if (codeErrors?.['required']) {
        this.toastr.error('Você precisa inserir um código válido!');
      }
    }

    this.isLoading = true;
    this.appService.sendCodeAndLogin(this.form.get('code')?.value).subscribe({
      next: (response: HttpResponse<any>) => {
        this.isLoading = false;
        this.toastr.success("Login efetuado com sucesso");
        this.isLoading = true;
        this.appService.clearLocalStorage();
        this.appService.configureLocalStorage(response.body);
        this.route.navigate(['/']);
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.toastr.error("Código inválido!");
        return;
      }
    });
  }
}
