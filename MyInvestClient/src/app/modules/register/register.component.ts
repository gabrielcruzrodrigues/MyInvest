import {
    Component,
    OnInit,
    Renderer2,
    OnDestroy,
    HostBinding
} from '@angular/core';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
import { AppService } from '@services/app.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
    @HostBinding('class') class = 'register-box';

    public registerForm: UntypedFormGroup;
    public isAuthLoading = false;
    public isGoogleLoading = false;
    public isFacebookLoading = false;

    constructor(
        private renderer: Renderer2,
        private toastr: ToastrService,
        private appService: AppService
    ) { }

    ngOnInit() {
        this.renderer.addClass(
            document.querySelector('app-root'),
            'register-page'
        );
        this.registerForm = new UntypedFormGroup({
            username: new UntypedFormControl(null, Validators.required),
            email: new UntypedFormControl(null, [Validators.required, Validators.email]),
            password: new UntypedFormControl(null, [Validators.required]),
            retypePassword: new UntypedFormControl(null, [Validators.required])
        });
    }

    async registerByAuth() {
        if (!this.verifyPassword(this.registerForm.value.password))
        {
            return;
        }
        
        if (this.registerForm.valid) {
            this.isAuthLoading = true;

            await this.appService.registerWithEmail(
                this.registerForm.value.username,
                this.registerForm.value.email,
                this.registerForm.value.password
            );
            this.isAuthLoading = false;
        } else {
            const emailErrors = this.registerForm.get('email')?.errors;
            const usernameErrors = this.registerForm.get('username')?.errors;

            if (usernameErrors?.['required']) {
                this.toastr.error('O campo de username é obrigatório!');
                return;
            }
            
            if (emailErrors?.['required']) {
                this.toastr.error('O campo de email é obrigatório!');
                return;
            }
            
            if (emailErrors?.['email']) {
                this.toastr.error('Por favor, insira um email válido!');
                return;
            }
        }
    }

    verifyPassword(password: string): boolean {
        if (password === null) {
            this.toastr.error("A senha não pode ser nula!");
            return false;
        }

        if (password.length < 8 || password === null) {
            this.toastr.error("A senha deve ter mais que 8 caracteres!");
            return false;
        }

        if (!/[A-Z]/.test(password)) {
            this.toastr.error("A senha deve conter pelo menos uma letra maiúscula!");
            return false;
        }

        if (!/[0-9]/.test(password)) {
            this.toastr.error("A senha deve conter pelo menos um número!");
            return false;
        }

        if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
            this.toastr.error("A senha deve conter pelo menos um caractere especial!");
            return false;
        }

        if (this.registerForm.value.retypePassword !== password)
        {
            this.toastr.error("As senhas não são iguais!");
            return false;
        }

        return true;
    }

    async registerByGoogle() {
        this.isGoogleLoading = true;
        await this.appService.signInByGoogle();
        this.isGoogleLoading = false;
    }

    ngOnDestroy() {
        this.renderer.removeClass(
            document.querySelector('app-root'),
            'register-page'
        );
    }
}
