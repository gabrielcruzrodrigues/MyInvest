import {
  Component,
  OnInit,
  OnDestroy,
  Renderer2,
  HostBinding
} from '@angular/core';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AppService } from '@services/app.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent implements OnInit {
  public loginForm: UntypedFormGroup;
  public isAuthLoading = false;
  public isGoogleLoading = false;
  public isFacebookLoading = false;

  constructor(
    private renderer: Renderer2,
    private toastr: ToastrService,
    private appService: AppService
  ) { }

  ngOnInit() {
    this.loginForm = new UntypedFormGroup({
      email: new UntypedFormControl(null, [Validators.required, Validators.email]),
      password: new UntypedFormControl(null, Validators.required)
    });
  }

  loginByAuth() {
    if (this.loginForm.invalid) {
      const emailErrors = this.loginForm.get('email')?.errors;
      const passwordErrors = this.loginForm.get('password')?.errors;

      if (emailErrors?.['required']) {
        this.toastr.error('O campo de email é obrigatório!');
      } 
      
      if (emailErrors?.['email']) {
        this.toastr.error('Por favor, insira um email válido!');
      }

      if (passwordErrors?.['required']) {
        this.toastr.error('O campo de senha é obrigatório!');
      }
    }

    this.isAuthLoading = true;
    this.appService.loginWithEmail(
      this.loginForm.value.email,
      this.loginForm.value.password
    );

    this.isAuthLoading = false;
  }

  async loginByGoogle() {
    this.isGoogleLoading = true;
    await this.appService.signInByGoogle();
    this.isGoogleLoading = false;
  }
}
